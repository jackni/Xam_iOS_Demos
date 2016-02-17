using System;
using System.Collections.Generic;

namespace MyDemo.Core
{
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class GoogleAddressResult
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public List<string> types { get; set; }
    }

    /// <summary>
    /// This class will be the return result from google GeoCoding
    /// </summary>
    public class GoogleAddressReturnResult
    {
        public List<GoogleAddressResult> results { get; set; }
        public string status { get; set; }
    }

    /// <summary>
    /// this class will be used to store client actual address including google geo data.
    /// </summary>
    public class SystemGeoLocationResult
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public string GoogleReturnAddress { get; set; }
    }

    public enum GoogleGeoCodingStatus
    {
        OK = 1
            ,ZERO_RESULTS = 2
            ,OVER_QUERY_LIMIT =3 
            ,REQUEST_DENIED = 4
            ,INVALID_REQUEST = 5
            ,UNKNOWN_ERROR = 6
    }
}

