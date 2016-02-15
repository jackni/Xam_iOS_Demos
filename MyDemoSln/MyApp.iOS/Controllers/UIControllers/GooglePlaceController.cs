using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using ReactiveUI;
using Plugin.Connectivity;

namespace MyApp.iOS
{
    partial class GooglePlaceController : ReactiveTableViewController//UITableViewController
	{
		#region Members
        public UITableViewController CallerController { get; set; }

        public string SelectedAddress { get; set; }

        GooglePlaceHelper _googlePlace;

        public OnAddressSelected OnAddressSelected { get; set; }
        #endregion

        #region Constructors
        public GooglePlaceController (IntPtr handle) : base (handle)
		{
            _googlePlace = new GooglePlaceHelper();
		}

        #endregion

        #region Methods
        public override void ViewDidLoad()
        {

            AddressSearchBar.TextChanged += AddressSearchBar_TextChanged;
            AddressSearchBar.SearchButtonClicked += AddressSearchBar_SearchButtonClicked;
            this.AddressSearchBar.BecomeFirstResponder();
            base.ViewDidLoad();
        }

        void AddressSearchBar_SearchButtonClicked (object sender, EventArgs e)
        {
            this.AddressSearchBar.ResignFirstResponder();
        }

        async void AddressSearchBar_TextChanged (object sender, UISearchBarTextChangedEventArgs e)
        {
            if (AddressSearchBar.Text.Length < 5)
            {
                return;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                //await PerformAddressSearch(AddressSearchBar.Text);
                var result = await _googlePlace.PerformAddressSearch(AddressSearchBar.Text);
                TableView.Source = new AddressLookupTableSource(this,result,"addressCell",OnAddressSelected);
                TableView.ReloadData();
            }
        }
        #endregion
	}
}
