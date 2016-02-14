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
using System.Linq;

namespace MyApp.iOS
{
    partial class AMSController : ReactiveTableViewController//UITableViewController
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
		}

        public override async void ViewDidLoad()
        {
            _storyboard = this.Storyboard;
            MoreMenuBtn.Clicked += MoreMenuBtn_Clicked;
            this.RefreshControl.ValueChanged += RefreshControl_ValueChanged;;
//            var notificationView = new GCDiscreetNotificationView (
//                text: "sync todo list",
//                activity: true,
//                presentationMode: GCDNPresentationMode.Top,
//                view: View
//            );
//            //notificationView.TintColor = UIColor.Blue;
//            notificationView.Show (animated: true);
//

            await LoadToDoList(false);
//            notificationView.Hide(true);

            base.ViewDidLoad();
        }

        async void RefreshControl_ValueChanged (object sender, EventArgs e)
        {
            RefreshControl.BeginRefreshing();
            //Do something
            await LoadToDoList(true);
            RefreshControl.EndRefreshing();
            return;     
        }



        void MoreMenuBtn_Clicked (object sender, EventArgs e)
        {
            MenuSetup();
        }

        void MenuSetup()
        {
            UIAlertController actionSheetAlert = UIAlertController
                .Create("Action", "Select an item from below", UIAlertControllerStyle.ActionSheet);
            // Add Actions
            actionSheetAlert.AddAction(UIAlertAction.Create("Add One Item",UIAlertActionStyle.Default, (action) => CreateNewTodo()));

            actionSheetAlert.AddAction(UIAlertAction.Create("Add 100 Items",UIAlertActionStyle.Default, (action) => DeleteAllTodo()));

            actionSheetAlert.AddAction(UIAlertAction.Create("Delete All",UIAlertActionStyle.Default, (action) => Console.WriteLine ("Item Three pressed.")));

            actionSheetAlert.AddAction(UIAlertAction.Create("Cancel",UIAlertActionStyle.Cancel, (action) => Console.WriteLine ("Cancel button pressed.")));

            // Required for iPad - You must specify a source for the Action Sheet since it is
            // displayed as a popover
            UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover!=null) {
                presentationPopover.SourceView = this.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            // Display the alert
            this.PresentViewController(actionSheetAlert,true,null);
                
        }

        async Task LoadToDoList(bool syncClould)
        {
            
            Expression<Func<TodoItem, bool>> predicate = (t => true);
            //Expression<Func<TodoItem, bool>> predicate = (t => t.Text.ToLower().Contains("stuff"));
            syncTaskId = UIApplication.SharedApplication.BeginBackgroundTask(
                () =>
                {
                    /*code here will be exeuated when the remaining time is really low*/
                    UIApplication.SharedApplication.EndBackgroundTask(syncTaskId); 
                    syncTaskId = -1;

                });
            datasouce = await _dataService.QueryDataAsync(predicate,"getTodo",syncClould);
            datasouce = datasouce.OrderByDescending(d => d.UpdatedAt).ThenByDescending(d => d.CreatedAt).ToList();
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

        void CreateNewTodo() 
        {
            //Create Alert
            var textInputAlertController = UIAlertController.Create("Create new Todo Item", "Hey, input some text", UIAlertControllerStyle.Alert);

            //Add Text Input
            textInputAlertController.AddTextField(textField => {
            });

            //Add Actions
            var cancelAction = UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine ("Cancel was Pressed"));
            var okayAction = UIAlertAction.Create("Okay", UIAlertActionStyle.Default
                , alertAction =>
                {
                    TodoItem newItem = new TodoItem{Text = textInputAlertController.TextFields[0].Text,Complete = false};

                    Task.WhenAll(_dataService.InsertDataAsync<TodoItem>(newItem,false)
                        ,LoadToDoList(false)
                        ,_dataService.PushToCloud());//push to cloud
                    
                });

            textInputAlertController.AddAction(cancelAction);
            textInputAlertController.AddAction(okayAction);

            //Present Alert
            PresentViewController(textInputAlertController, true, null);
        }

        void DeleteAllTodo()
        {
            var okCancelAlertController = UIAlertController.Create("Delete all Items", "Delete all Items", UIAlertControllerStyle.Alert);

            //Add Actions
            okCancelAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default
                , alert => {
                //remove all local data. default timeout is 1 mins, as this is is a demo
                foreach (var d in datasouce)
                {
                    _dataService.DeleteDataAsync(d,null,false);
                }
                Task.WhenAll(LoadToDoList(false)
                    ,_dataService.PushToCloud()
                );
            }));
            okCancelAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine ("Cancel was clicked")));

            //Present Alert
            PresentViewController(okCancelAlertController, true, null);

        }
       
	}
}
