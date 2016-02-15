using System;
using System.Collections.Generic;

namespace MyDemo.Core
{
    public class Prediction
    {
        public string description { get; set; }
        public string id { get; set; }
        public List<MatchedSubstring> matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public List<Term> terms { get; set; }
        public List<string> types { get; set; }
    }
}

