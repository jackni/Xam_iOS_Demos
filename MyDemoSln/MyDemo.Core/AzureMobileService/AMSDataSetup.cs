using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MyDemo.Data;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace MyDemo.Core
{
    public class AMSDataSetup
    {
        public MobileServiceClient AzureServiceClient;

        public AMSDataSetup()
        {
        }

        //Custom table objects need to be put here, like local seed method
        public static void SetupSyncTableReferences(MobileServiceClient azureServiceClient)
        {
            IMobileServiceSyncTable<TodoItem> todoTable = azureServiceClient.GetSyncTable<TodoItem>();

        }

        public static void SetupLocalDataStoreTables(MobileServiceSQLiteStore localStore)
        {
            localStore.DefineTable<TodoItem>();


        }

        public static MobileServiceClient MobileServiceStoreSetup(string AppUrl, string AppKey)
        {
            MobileServiceClient AzureServiceClient = new MobileServiceClient(AppUrl, AppKey);

            SetupSyncTableReferences(AzureServiceClient);

            return AzureServiceClient;
        }
    }
}

