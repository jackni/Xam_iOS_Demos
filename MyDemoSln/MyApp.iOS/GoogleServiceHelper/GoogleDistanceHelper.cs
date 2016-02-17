using System;
using MyDemo.Core;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace MyApp.iOS
{
    public class GoogleDistanceHelper
    {
        public static string ErrMsg { get; set;}

        /// <summary>
        /// Gets the google map geo data from address.
        /// </summary>
        /// <param name="addressLine">The address line.</param>
        /// <returns></returns>
        public static SystemGeoLocationResult GetGoogleMapGeoDataFromAddress(string addressLine)
        {
            ErrMsg = string.Empty;

            SystemGeoLocationResult _result = new SystemGeoLocationResult();
           
            var _restClient = new RestClient(GoogleServiceConfig._GOOGLE_MAP_GEOCODING_SERVICE_URL);
            var _request = new RestRequest();
            addressLine = addressLine.Replace(" ", "+");
            _request.Parameters.Add(new Parameter { Name = "address", Value = addressLine, Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "key", Value = "googleAPIkey", Type = ParameterType.GetOrPost });           
            _request.Method = Method.GET;
            _request.Credentials = CredentialCache.DefaultNetworkCredentials;
            try
            {
                var _jsonString = _restClient.Execute(_request).Content;
                var _googleResult = JsonConvert.DeserializeObject<GoogleAddressReturnResult>(_jsonString);
                if (_googleResult.status != "OK")
                {
                    ErrMsg = _googleResult.status;
                    _result = null;
                    return _result;
                }
                // in this case always get the first one becuase the address query will always contains one address line
                GoogleAddressResult geoData = _googleResult.results.FirstOrDefault();
                _result.GoogleReturnAddress = geoData.formatted_address;
                _result.lat = geoData.geometry.location.lat;
                _result.lng = geoData.geometry.location.lng;

                return _result;
            }
            catch (Exception ex)
            {
                _result = null;
                ErrMsg = ex.Message;
            }

            return _result;
        }

        /// <summary>
        /// Gets the google map distance matrix result.
        /// </summary>
        /// <returns></returns>
        public static GoogleDistanceMatrixResult GetGoogleMapDistanceMatrixResult(GoogleDistanceQuery distanceQuery)
        {
            ErrMsg = string.Empty;
            GoogleDistanceMatrixResult _result = new GoogleDistanceMatrixResult();
            var _restClient = new RestClient(GoogleServiceConfig._GOOLE_MAP_DISTANCE_SERVICE_URL);
            var _request = new RestRequest();
            string destAddressLine = string.Empty;
            if (distanceQuery.DestAddress != null)
                destAddressLine = distanceQuery.DestAddress.Replace(" ", "+");
            string originLatLng = distanceQuery.OriginLat.ToString() + @"," + distanceQuery.OriginLng.ToString();
            _request.Parameters.Add(new Parameter { Name = "origins", Value = originLatLng, Type = ParameterType.GetOrPost });
            _request.Parameters.Add(new Parameter { Name = "destinations", Value = destAddressLine, Type = ParameterType.GetOrPost });
            _request.Parameters.Add(new Parameter { Name = "mode", Value = "driving " });
            _request.Parameters.Add(new Parameter { Name = "language", Value = "en-AU" });
            _request.Method = Method.GET;
            _request.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                var _jsonString = _restClient.Execute(_request).Content;
                var _googleResult = JsonConvert.DeserializeObject<GoogleDistanceMatrixResult>(_jsonString);
                if (_googleResult.Status != "OK")
                {
                    ErrMsg = _googleResult.Status;
                    _result = null;
                    return _result;
                }               
                _result = _googleResult;
            }
            catch (Exception ex)
            {
                _result = null;
                ErrMsg = ex.Message;
            }


            return _result;
        }


        /// <summary>
        /// Gets the google map disatance matrix result list.
        /// </summary>
        /// <param name="originAddress">The origin address.</param>
        /// <param name="destAddressList">The dest address list.</param>
        /// <returns></returns>
        public static GoogleDistanceMatrixResult GetGoogleMapDisatanceMatrixResultList(string originAddress, List<string> destAddressList)
        {
            ErrMsg = string.Empty;
            GoogleDistanceMatrixResult _result = new GoogleDistanceMatrixResult();
            var _restClient = new RestClient(GoogleServiceConfig._GOOLE_MAP_DISTANCE_SERVICE_URL);
            var _request = new RestRequest();
            string orginAddressLine = string.Empty;
            string destAddressLine = string.Empty;
            if (destAddressList != null)
            {
                foreach (var d in destAddressList)
                {
                    destAddressLine = destAddressLine + d.Replace(@" ", @"+") + @"|";
                }
            }
            destAddressLine.Remove(destAddressLine.Length - 1);//remove last charater "|"
            orginAddressLine = originAddress.Replace(@" ", @"+");

            _request.Parameters.Add(new Parameter { Name = "origins", Value = orginAddressLine, Type = ParameterType.GetOrPost });
            _request.Parameters.Add(new Parameter { Name = "destinations", Value = destAddressLine, Type = ParameterType.GetOrPost });
            _request.Parameters.Add(new Parameter { Name = "mode", Value = "driving " });
            _request.Parameters.Add(new Parameter { Name = "language", Value = "en-AU" });
            _request.Method = Method.GET;
            _request.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                var _jsonString = _restClient.Execute(_request).Content;
                var _googleResult = JsonConvert.DeserializeObject<GoogleDistanceMatrixResult>(_jsonString);
                if (_googleResult.Status != "OK")
                {
                    ErrMsg = _googleResult.Status;
                    _result = null;
                    return _result;
                }

                _result = _googleResult;
            }
            catch (Exception ex)
            {
                _result = null;
                ErrMsg = ex.Message;
            }

            return _result;

        }

    }
}

