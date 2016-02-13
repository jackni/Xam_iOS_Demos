using System;
using ReactiveUI;
using System.Threading.Tasks;
using System.Reactive;
using Splat;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading;

namespace MyDemo.Core
{
    public static class Bootstrap
    {
        const int _opertionTimeoutMs = 30000; //30sec.
        const int _longRunOperationTimeout = 60000; //1min;

        //CancellationTokenSource _cts;



        public static void Init()
        {
            RxApp.DefaultExceptionHandler = Observer.Create((Exception e) => {
                if (e is TaskCanceledException)
                    e = new Exception("Timeout waiting for server to respond!");
                //Locator.Current.GetService<IAlertDialogFactory>().Alert("Error", e.Message);
            });
            var resolver = Locator.CurrentMutable;

            resolver.Register(() => new AMSDataService(), typeof(IDataService));

        }
    }
}

