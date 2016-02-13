using System;
using MyDemo.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace MyApp.iOS
{
    public class AMSProxy
    {
        #region Members

        //private static AMSService _current;

        private static AMSProxy _current;
        public static AMSProxy Current 
        {
            get
            {
                if (_current == null)
                {
                    _current = new AMSProxy();
                }
                return _current;
            }    
        }

        public AMSService AMSPclServiceInstance { get; set; }

        #endregion

        #region Constructors
        public AMSProxy()
        {
            CurrentPlatform.Init();
            SQLitePCL.CurrentPlatform.Init();
            AMSPclServiceInstance = AMSService.Current;
        }

        #endregion

        #region Methods
        //local datastore reset.
        #endregion
    }
}

