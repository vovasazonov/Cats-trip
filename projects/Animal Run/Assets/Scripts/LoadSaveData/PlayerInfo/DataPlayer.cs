using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataPlayer{

    //values witch have to save to file 
    //[WARNING : IF YOU WILL CHANGE NAME OR DELETE VALUES - THE FILE DATA WILL GET DEFAULT VALUES!]
    public int score;
    public int coins;
    public int currentAnimal;   //the animal that run in the game
    public List<int> boughtAnimals = new List<int>(); //all animals that player bought
    public bool isMusicMainMenu;
    
    //overide operators
    public static bool operator ==(DataPlayer first, DataPlayer second)
    {
        bool notSame = false;   //true if not same values

        if (first.score != second.score) notSame = true;
        if (first.coins != second.coins) notSame = true;

        if (notSame) return false;
        else return true;
    }
    public static bool operator !=(DataPlayer first, DataPlayer second)
    {
        //if (first.score != second.score) return true;
        //else return false;

        bool same = false;   //true if same values

        if (first.score == second.score) same = true;
        if (first.coins == second.coins) same = true;

        if (same) return false;
        else return true;
    }


    /// <summary>
    /// set defoult data
    /// </summary>
    public void SetDefoultData()
    {
        //set all field on defoult value
        isMusicMainMenu = true;
        score = 0;
        coins = 0;
        currentAnimal = (int)Animals.animals.grayCat;
        boughtAnimals = new List<int>();
        boughtAnimals.Clear();
        boughtAnimals.Add((int)Animals.animals.grayCat);
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
