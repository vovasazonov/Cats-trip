using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataQuests {

    //values witch have to save to file 
    //[WARNING : IF YOU WILL CHANGE NAME OR DELETE VALUES - THE FILE DATA WILL GET DEFAULT VALUES!]
    public DateTime dateQuest = DateTime.Today;//the day of last using quests
    public bool wasTodayRewardVideo = false;

    public void SetDefoultData()
    {
        wasTodayRewardVideo = false;
        dateQuest = DateTime.Today;//the day of last using in ad
    }
}
