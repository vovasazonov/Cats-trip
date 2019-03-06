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
	public DateTime dateQuest = DateTime.Today;
	// True if user saw video today
    public bool wasTodayRewardVideo = false;

	/// <summary>
	/// Set default data like user just 
	/// in first enter to game.
	/// </summary>
    public void SetDefoultData()
    {
        wasTodayRewardVideo = false;
        dateQuest = DateTime.Today;//the day of last using in ad
    }
}
