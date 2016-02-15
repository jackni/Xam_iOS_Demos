using System;
using UIKit;
using MyDemo.Core;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MyApp.iOS
{
    public delegate void OnAddressSelected (string selectedAddress);

    public class AddressLookupTableSource : UITableViewSource
    {
        #region Member
        public string CellId { get; set; }

        List<Prediction> data = new List<Prediction>();

        UITableViewController controller;

        public OnAddressSelected OnAddressSelected{ get; set;}
        #endregion

        #region Constructors

        public AddressLookupTableSource(UITableViewController lookupController, List<Prediction> data, string cellId,OnAddressSelected onAddressSelected)
        {
            this.CellId = cellId;
            this.controller = lookupController;
            this.data = data;
            this.OnAddressSelected = onAddressSelected;
        }

        #endregion

        #region Methods
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return data.Count;
            //return 1;
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
            cell.TextLabel.Text = cellObj.description;

            return cell;
        }


        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
           
            Prediction addressobj = data.ElementAt(indexPath.Row);
            Debug.WriteLine("selected address is --- " + addressobj.description);

            if (this.OnAddressSelected != null)
            {
                this.OnAddressSelected.Invoke(addressobj.description);
            }

            this.controller.NavigationController.PopViewController(true);
        }
        #endregion
    }
}

