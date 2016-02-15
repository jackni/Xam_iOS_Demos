using System;
using Newtonsoft.Json;

namespace MyDemo.Core
{
    public class GoogleAPIRequestResult
    {
        #region Members

        #endregion

        #region Constructors

        public GoogleAPIRequestResult()
        {
        }

        #endregion

        #region Methods
        public static LocationPrediction LocationAutoComplete(string jsonResult)
        {
            LocationPrediction objLocationPredict = null;
            if(!string.IsNullOrEmpty(jsonResult)&& jsonResult!="Exception")
            {
                objLocationPredict= JsonConvert.DeserializeObject<LocationPrediction> (jsonResult);
            }
            return objLocationPredict;
        }

        #endregion
    }
}

