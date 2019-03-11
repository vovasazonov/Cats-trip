/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System;

/// <summary>
/// Data about quests interaction
/// </summary>
[Serializable]
public class DataQuests : IData{
	//[WARNING : IF YOU WILL CHANGE NAME OR DELETE VALUES - THE FILE DATA WILL GET DEFAULT VALUES!]

	// The day of last using quests
	private DateTime _dateQuest = DateTime.Today;
	// True if user saw video today
	private bool _wasTodayRewardVideo = false;

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

	public DateTime DateQuest
	{
		get
		{
			return _dateQuest;
		}

		set
		{
			_dateQuest = value;
		}
	}

	/// <summary>
	/// Set default data like user just 
	/// in first enter to game.
	/// </summary>
	public void SetDefoultData()
    {
        WasTodayRewardVideo = false;
        DateQuest = DateTime.Today;//the day of last using in ad
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
