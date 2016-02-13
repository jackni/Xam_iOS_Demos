using System;
using UIKit;
using MyDemo.Data;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.iOS
{
    public class AMSDataTableSource : UITableViewSource
    {

        #region Members

        public string CellId { get; set; }

        public UIStoryboard OptStoryboard { get; set; }

        List<TodoItem> data;

        UITableViewController controller;

        #endregion

        public AMSDataTableSource(List<TodoItem> toDodata, UITableViewController controller, string cellId, UIStoryboard storyboard)
        {
            this.data = toDodata;
            this.controller = controller;
            this.CellId = cellId;
            this.OptStoryboard = storyboard;
        }

        #region Methods

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return data.Count;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override bool CanEditRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return true;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            UITableViewCell cell;

            var cellObj = data.ElementAt(indexPath.Row);
            cell = tableView.DequeueReusableCell(CellId);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellId);
            }
            cell.TextLabel.Text = cellObj.Text;
            //            cell.TextLabel.Text = cellObj.FirstName + " " + cellObj.LastName;
            //            if (string.IsNullOrEmpty(cellObj.AuthorisationCode))
            //            {
            //                cell.DetailTextLabel.Text = cellObj.MobilePhoneNumber;
            //            }
            //            else
            //            {
            //                cell.DetailTextLabel.Text = cellObj.MobilePhoneNumber + " (Authcode: " + cellObj.AuthorisationCode + " )";
            //            }

            return cell;
        }


        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var driverObj = data.ElementAt(indexPath.Row);
            if (this.OptStoryboard == null)
            {
                return;
            }
            //to Driver Save View
            //            var driverSaveView =
            //                OptStoryboard.InstantiateViewController("DriverSaveView") as DriverSaveViewController;
            //            driverSaveView.IsCreateDriver = false;
            //            driverSaveView.DriverObj = driverObj;
            //            driverSaveView.ProfileDataViewController = this.controller as OptProfileDataViewController;
            //            this.controller.NavigationController.PushViewController(driverSaveView, true);

        }


        #endregion
    }
}

