using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using MyDemo.Data;
using MyDemo.Core;
using Splat;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GCDiscreetNotification;
using MBProgressHUD;
using ReactiveUI;

namespace MyApp.iOS
{
    partial class AMSController :UITableViewController// ReactiveTableViewController//UITableViewController
	{
        #region Members
        List<TodoItem> datasouce = new List<TodoItem>();
        static readonly string dataCellId = "ProfileDataCell";

        //UIStoryboard optStoryboard = StoryBoardFactory.StoryBoardInstance.OperatorStoryBoard;
        UIStoryboard _storyboard;
        IDataService _dataService;

        nint syncTaskId = -1;
        #endregion

        public AMSController (IntPtr handle) : base (handle)
		{
            
            _dataService = Locator.Current.GetService<IDataService>();
            _storyboard = this.Storyboard;
		}

        public override async void ViewDidLoad()
        {
            var notificationView = new GCDiscreetNotificationView (
                text: "sync todo list",
                activity: true,
                presentationMode: GCDNPresentationMode.Top,
                view: View
            );
            //notificationView.TintColor = UIColor.Blue;
            notificationView.Show (animated: true);



            notificationView.Hide(true);

            base.ViewDidLoad();
        }

        async Task LoadToDoList()
        {
            

            syncTaskId = UIApplication.SharedApplication.BeginBackgroundTask(
                () =>
                {
                    /*code here will be exeuated when the remaining time is really low*/
                    UIApplication.SharedApplication.EndBackgroundTask(syncTaskId); 
                    syncTaskId = -1;

                });
            //Expression<Func<TodoItem, bool>> predicate = (t => t.Text.ToLower().Contains("stuff"));
            Expression<Func<TodoItem, bool>> predicate = (t => ture);
            datasouce = await _dataService.QueryDataAsync(predicate,"getTodo",true);
            InvokeOnMainThread(()=>{
                TableView.Source = new AMSDataTableSource(datasouce
                    , this
                    , dataCellId
                    , _storyboard
                );
                TableView.ReloadData();
            });

            UIApplication.SharedApplication.EndBackgroundTask(syncTaskId);

        }
	}
}
