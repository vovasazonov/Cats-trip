/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System;

/// <summary>
/// Data about ad interaction
/// </summary>
[Serializable]
public class DataAd : IData{
	//[WARNING : IF YOU WILL CHANGE NAME OR DELETE VALUES - THE FILE DATA WILL GET DEFAULT VALUES!]

	// The day of last using in ad
	private DateTime _dateAd = DateTime.Today;
	// Count that was clicked on ad full
	private int _clicksOnAdFull = 0;
	// Count that was clicked on ad banner
	private int _clicksOnAdBanner = 0;
	// True if there was rewarded video today
	private bool _wasTodayRewardVideo = false;

	public DateTime DateAd
	{
		get
		{
			return _dateAd;
		}

		set
		{
			_dateAd = value;
		}
	}

	public int ClicksOnAdFull
	{
		get
		{
			return _clicksOnAdFull;
		}

		set
		{
			_clicksOnAdFull = value;
		}
	}

	public int ClicksOnAdBanner
	{
		get
		{
			return _clicksOnAdBanner;
		}

		set
		{
			_clicksOnAdBanner = value;
		}
	}

	public bool WasTodayRewardVideo
	{
		get
		{
			return _wasTodayRewardVideo;
		}

		set
		{
			_wasTodayRewardVideo = value;
		}
	}


	#region rate values
	private bool _isRatedApp = false;
	private int _clicksOnRestartButton = 0;

	public bool IsRatedApp
	{
		get
		{
			return _isRatedApp;
		}

		set
		{
			_isRatedApp = value;
		}
	}

	public int ClicksOnRestartButton
	{
		get
		{
			return _clicksOnRestartButton;
		}

		set
		{
			_clicksOnRestartButton = value;
		}
	}

	#endregion

	/// <summary>
	/// Set default data like user just 
	/// in first enter to game.
	/// </summary>
	public void SetDefoultData()
    {
        #region rate values
        IsRatedApp = false;
        ClicksOnRestartButton = 0;
        #endregion

        DateAd = DateTime.Today;
        ClicksOnAdFull = 0;
        ClicksOnAdBanner = 0;
    }

    /// <summary>
    /// Clone object data
    /// </summary>
    /// <returns>clone object</returns>
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
