using System;
using System.Text;
using MyDemo.Core;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace MyApp.iOS
{
    public class GooglePlaceHelper
    {
        string strAutoCompleteQuery;

        async Task<string> CallGooglePlaceService(string strURL)
        {
            using (var client = new WebClient())
            {
                string strResult;
                try
                {
                    strResult=await client.DownloadStringTaskAsync (new Uri(strURL));
                }
                catch(Exception ex)
                {
                    strResult="Exception";
                }

                return strResult;
            }
        }

        public async Task<List<Prediction>> PerformAddressSearch(string addressSearch)
        {
            List<Prediction> result = new List<Prediction>();

            if (string.IsNullOrWhiteSpace(addressSearch))
            {
                return result;
            }

            StringBuilder builderLocationAutoComplete = new StringBuilder(GoogleServiceConfig.PlacesAutofillUrl);
            builderLocationAutoComplete.Append ("?input={0}").Append("&key=").Append(GoogleServiceConfig.GooglePlaceAPILey);
            strAutoCompleteQuery = builderLocationAutoComplete.ToString ();

            string strFullURL = string.Format (strAutoCompleteQuery,addressSearch);

            var searchResult = await CallGooglePlaceService(strFullURL);

            var LookupResult = GoogleAPIRequestResult.LocationAutoComplete(searchResult);

            if (LookupResult != null && LookupResult.status == @"OK")
            {
                if (LookupResult.predictions.Count > 0)
                {
                    result = LookupResult.predictions;
                }

            }

            builderLocationAutoComplete.Clear();

            return result;
        }
    }
}

