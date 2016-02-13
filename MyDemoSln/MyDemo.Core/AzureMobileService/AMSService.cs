using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading.Tasks;

namespace MyDemo.Core
{
    public class AMSService
    {
        #region Members
        static AMSService _current;
        public static AMSService Current 
        {
            get
            {
                if (_current == null)
                {
                    _current = new AMSService();
                }
                return _current;
            }
        }

        const string applicationURL = @"https://chauffeurupmobileservice.azure-mobile.net/";
        const string applicationKey = @"bBbFuSsuNOnsxASXdDMPEbzAxQkJEK48";
        //local storage name.
        public string localDbPath = "chauffeurUpstore.db";

        public MobileServiceClient AzureServiceClient;

        #endregion

        #region Constructors

        public AMSService()
        {
            AzureServiceClient = AMSDataSetup.MobileServiceStoreSetup(applicationURL, applicationKey);
        }

        #endregion

        #region Methods

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


                    AMSDataSetup.SetupLocalDataStoreTables(store);
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
                        AMSDataSetup.SetupLocalDataStoreTables(store);
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

        #endregion
    }
}

