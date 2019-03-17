/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections.Generic;
using System;

/// <summary>
/// Data about dataplayer interaction
/// </summary>
[Serializable]
public class DataPlayer : IData{
    //[WARNING : IF YOU WILL CHANGE NAME OR DELETE VALUES - THE FILE DATA WILL GET DEFAULT VALUES!]

	// Max score
    public int Score { get; set; }
	// Amount of coins
    public int Coins { get; set; }
	// The animal that run in the game
	public int CurrentAnimal { get; set; }
	// All animals that player bought
	private List<int> _boughtAnimals;
	public List<int> BoughtAnimals
	{
		get
		{
			return _boughtAnimals;
		}

		set
		{
			_boughtAnimals = value;
		}
	}
	// True if there are music in main menu
	public bool IsMusicMainMenu { get; set; }

	// Overide operators
	public static bool operator ==(DataPlayer first, DataPlayer second)
    {
		// True if not same values
		bool notSame = false;   

        if (first.Score != second.Score) notSame = true;
        if (first.Coins != second.Coins) notSame = true;

        if (notSame) return false;
        else return true;
    }
    public static bool operator !=(DataPlayer first, DataPlayer second)
    {
		// True if same values
		bool same = false;  

        if (first.Score == second.Score) same = true;
        if (first.Coins == second.Coins) same = true;

        if (same) return false;
        else return true;
    }

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		DataPlayer d = obj as DataPlayer;
		if (d as DataPlayer == null)
			return false;

		return this == d;
	}

	public bool Equals(DataPlayer obj)
	{
		if (obj == null)
		{
			return false;
		}

		return this == obj;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	/// <summary>
	/// Set default data like user just 
	/// in first enter to game.
	/// </summary>
	public void SetDefoultData()
    {
        IsMusicMainMenu = true;
        Score = 0;
        Coins = 0;
        CurrentAnimal = (int)Animal.GrayCat;
        BoughtAnimals = new List<int>();
        BoughtAnimals.Clear();
        BoughtAnimals.Add((int)Animal.GrayCat);
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
