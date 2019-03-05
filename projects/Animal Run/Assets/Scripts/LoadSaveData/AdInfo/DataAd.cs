using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataAd : IData{

    //values witch have to save to file 
    //[WARNING : IF YOU WILL CHANGE NAME OR DELETE VALUES - THE FILE DATA WILL GET DEFAULT VALUES!]
    public DateTime dateAd = DateTime.Today;//the day of last using in ad
    public int clicksOnAdFull = 0;
    public int clicksOnAdBanner = 0;
    public bool wasTodayRewardVideo = false;

    #region rate values
    public bool isRatedApp = false;
    public int clicksOnRestartButton = 0;
    #endregion

    /// <summary>
    /// set defoult data
    /// </summary>
    public void SetDefoultData()
    {
        #region rate values
        isRatedApp = false;
        clicksOnRestartButton = 0;
        #endregion

        dateAd = DateTime.Today;//the day of last using in ad
        clicksOnAdFull = 0;
        clicksOnAdBanner = 0;
    }

        /// <summary>
        /// clone object data
        /// </summary>
        /// <returns>clone object</returns>
        public object Clone()
    {
        return this.MemberwiseClone();
    }
}
