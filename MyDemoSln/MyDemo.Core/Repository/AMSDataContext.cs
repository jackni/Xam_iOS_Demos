using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MyDemo.Data;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading.Tasks;

namespace MyDemo.Core
{
    public class AMSDataContext //:AMSDataService
    {
        const string applicationURL = @"https://chauffeurupmobilepocjs.azure-mobile.net/";
        const string applicationKey = @"dBIOsushfAuBaQtpyGEtJprYdTaTPe23";
        private static string localDbPath = "chauffeurUpstore.db";

        public MobileServiceClient AzureServiceClient;

        private static AMSDataContext _current;
        public static AMSDataContext Current 
        {
            get 
            { 
                if (_current == null)
                {
                    _current = new AMSDataContext();   
                }
                return _current;
            }
        }

        public AMSDataContext()
        {
            AzureServiceClient = new MobileServiceClient(applicationURL, applicationKey);
            SetupSyncTableReferences(AzureServiceClient);

        }

        public static void SetupSyncTableReferences(MobileServiceClient azureServiceClient)
        {
            IMobileServiceSyncTable<TodoItem> todoTable = azureServiceClient.GetSyncTable<TodoItem>();
        }

        private static void SetupLocalDataStoreTables(MobileServiceSQLiteStore localStore)
        {
            localStore.DefineTable<TodoItem>();
        }

        public async Task InitializeStoreAsync(bool IsDbStructureChanged, string localStorePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(localStorePath))
                {
                    localDbPath = localStorePath;
                }

                //var syncHandler = new ChauffeurUpSyncHandler(AzureServiceClient, SyncConflictResoverDialog);

                if (IsDbStructureChanged)
                {

                    var store = new MobileServiceSQLiteStore(localStorePath);


                    SetupLocalDataStoreTables(store);
                    // Uses the default conflict handler, which fails on conflict
                    // To use a different conflict handler, pass a parameter to InitializeAsync. 
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
                    //await AzureServiceClient.SyncContext.InitializeAsync(store, syncHandler);
                    await AzureServiceClient.SyncContext.InitializeAsync(store);
                }
                else
                {
                    if (!AzureServiceClient.SyncContext.IsInitialized)
                    {
                        var store = new MobileServiceSQLiteStore(localDbPath);
                        SetupLocalDataStoreTables(store);
                        // Uses the default conflict handler, which fails on conflict
                        // To use a different conflict handler, pass a parameter to InitializeAsync. 
                        // For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
                        //await AzureServiceClient.SyncContext.InitializeAsync(store, syncHandler);
                        await AzureServiceClient.SyncContext.InitializeAsync(store);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

