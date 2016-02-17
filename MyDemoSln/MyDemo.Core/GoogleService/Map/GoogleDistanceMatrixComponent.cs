using System;
using System.Collections.Generic;

namespace MyDemo.Core
{
    public class Distance
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class Duration
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class GoogleDistanceMatrixElement
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string Status { get; set; }
    }

    public class GoogleDistanceMatrixRow
    {
        public List<GoogleDistanceMatrixElement> Elements { get; set; }
    }

    public class GoogleDistanceMatrixResult
    {
        public List<string> Destination_Address { get; set; }
        public List<string> Origin_Address { get; set; }
        public List<GoogleDistanceMatrixRow> Rows { get; set; }
        public string Status { get; set; }
    }

    public class GoogleDistanceQuery
    {
        public double OriginLat { get; set; }
        public double OriginLng { get; set; }
        public string DestAddress {get;set;}
    }

    public class GoogleDistanceQuery1
    {
        public string OriginAddress { get; set; }
        public string DestAddress {get;set;}
    }

    public class GoogleDistanceResult
    {
        public string Distance { get; set; }
        public string Duration { get; set; }
    }
}

