using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace MyDemo.Core
{
    public class AMSDataService : IDataService
    {
        const int _opertionTimeoutMs = 30000; //30sec.
        const int _longRunOperationTimeout = 60000; //1min;
        MobileServiceClient _serviceClient;
        public MobileServiceClient AMSClient
        {
            get 
            { 
                if (_serviceClient == null)
                {
                    _serviceClient = AMSService.Current.AzureServiceClient;
                }
                return _serviceClient;
            }
        }
        CancellationTokenSource _cts;

        public AMSDataService()
        {
            _serviceClient = AMSService.Current.AzureServiceClient;
            //_serviceClient.SyncContext.InitializeAsync();
        }

        #region IDataService implementation

        public async Task<List<T>> QueryDataAsync<T>(Expression<Func<T, bool>> predicate,string queryId,bool syncCloud)
        {
            //throw new NotImplementedException();
            List<T> result = new List<T>();
            _cts = new CancellationTokenSource(_opertionTimeoutMs);
            try
            {
                IMobileServiceSyncTable<T> syncTable = _serviceClient.GetSyncTable<T>();

                if (!CrossConnectivity.Current.IsConnected)
                {
                    //throw new Exception("No network connection");
                    result = await syncTable.Where(predicate).ToListAsync();
                    Debug.WriteLine("No network connection");
                    //TODO: need to have an delegation
                    return result;
                }

                if (syncCloud)
                {
                    await syncTable.PullAsync(queryId, syncTable.Where(predicate),_cts.Token);
                }

                result = await syncTable.Where(predicate).ToListAsync();
            }
            catch (MobileServicePushFailedException e)
            {
                Debug.WriteLine(@"Sync Failed: {0}, ReustSet {1}", e.Message, e.PushResult.ToString());
                //ErrMsg = string.Format(@"Sync Failed: {0}, ReustSet {1}", e.Message, e.PushResult.ToString());
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync Failed: {0}", e.Message);
                throw;
            }
            return result;

        }

        public async Task<T> UpdateDataAsync<T>(T updateData,string queryId,bool syncCloud)
        {
            //throw new NotImplementedException();
            _cts = new CancellationTokenSource(_opertionTimeoutMs);
            try
            {
                IMobileServiceSyncTable<T> syncTable = _serviceClient.GetSyncTable<T>();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    //throw new Exception("No network connection");
                    Debug.WriteLine("No network connection");
                    await syncTable.UpdateAsync(updateData);
                    return updateData;
                }
                await syncTable.UpdateAsync(updateData);
                if (syncCloud)
                {
                    Debug.WriteLine("UpdateDataAsync pushing to cloud");
                    await PushToCloud(_cts.Token);
                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                //ErrMsg = string.Format(@"Update {0} Failed: {1}", typeof(T).ToString(), e.Message);
                Debug.WriteLine(@"Update {0} Failed: {1}", typeof(T).ToString(), e.Message);
                updateData = default(T); //return null as reference type.
                throw;
            }
            catch(OperationCanceledException cancelEx)
            {
                Debug.WriteLine(@"Update {0} Failed: {1}", typeof(T).ToString(), "Stack Trace = " + cancelEx.StackTrace.ToString());
                updateData = default(T); //return null as reference type.
                throw;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(@"Update {0} Failed: {1}", typeof(T).ToString(), "Stack Trace = " + ex.StackTrace.ToString());
                updateData = default(T); //return null as reference type.
                throw;
            }

            return updateData;
        }

        public async Task<T> InsertDataAsync<T>(T insertData, bool syncCloud)
        {
            _cts = new CancellationTokenSource(_opertionTimeoutMs);
            try
            {
                IMobileServiceSyncTable<T> syncTable = _serviceClient.GetSyncTable<T>();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    //throw new Exception("No network connection");
                    Debug.WriteLine("No network connection");
                    await syncTable.InsertAsync(insertData);
                    return insertData;
                }

                await syncTable.InsertAsync(insertData);
                if (syncCloud)
                {
                    Debug.WriteLine("UpdateDataAsync pushing to cloud");
                    await PushToCloud(_cts.Token);
                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                //ErrMsg = string.Format(@"Update {0} Failed: {1}", typeof(T).ToString(), e.Message);
                Debug.WriteLine(@"insert {0} Failed: {1}", typeof(T).ToString(), e.Message);
                insertData = default(T); //return null as reference type.
                throw;
            }
            catch(OperationCanceledException cancelEx)
            {
                Debug.WriteLine(@"insert {0} Failed: {1}", typeof(T).ToString(), "Stack Trace = " + cancelEx.StackTrace.ToString());
                insertData = default(T); //return null as reference type.
                throw;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(@"insert {0} Failed: {1}", typeof(T).ToString(), "Stack Trace = " + ex.StackTrace.ToString());
                insertData = default(T); //return null as reference type.
                throw;
            }
            return insertData;
        }

        public async Task DeleteDataAsync<T>(T deleteData,string queryId,bool syncCloud)
        {
            //throw new NotImplementedException();
            _cts = new CancellationTokenSource(_opertionTimeoutMs);
            try
            {
                IMobileServiceSyncTable<T> syncTable = _serviceClient.GetSyncTable<T>();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    //throw new Exception("No network connection");
                    Debug.WriteLine("No network connection");
                    await syncTable.DeleteAsync(deleteData);
                    return;

                }

                await syncTable.DeleteAsync(deleteData);
                if (syncCloud)
                {
                    Debug.WriteLine("DeleteAsync pushing to cloud");
                    await PushToCloud(_cts.Token);
                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                //ErrMsg = string.Format(@"Update {0} Failed: {1}", typeof(T).ToString(), e.Message);
                Debug.WriteLine(@"delete {0} Failed: {1}", typeof(T).ToString(), e.Message);
                throw;
            }
            catch(OperationCanceledException cancelEx)
            {
                Debug.WriteLine(@"delete {0} Failed: {1}", typeof(T).ToString(), "Stack Trace = " + cancelEx.StackTrace.ToString());
                throw;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(@"delete {0} Failed: {1}", typeof(T).ToString(), "Stack Trace = " + ex.StackTrace.ToString());
                throw;
            }
        }

        public async Task PushToCloud(CancellationToken callectionToken)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    await _serviceClient.SyncContext.PushAsync(callectionToken);

                }
                catch(MobileServicePushFailedException pushEx)
                {
                    Debug.WriteLine("An error occured during push.  Stack Trace = " + pushEx.StackTrace.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    //var r = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync("We encountered an issue synchronising data.  Please sync manually", "Resync", "OK", null);
                    Debug.WriteLine("An error occured during sync.  Stack Trace = " + e.StackTrace.ToString());
                    throw;
                }
            }
        }
        #endregion
    }
}

