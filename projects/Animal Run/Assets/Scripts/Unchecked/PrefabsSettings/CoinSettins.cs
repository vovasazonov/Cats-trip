/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollectEvent : UnityEvent<CoinSettins>
{
}

public class CoinSettins : MonoBehaviour {

	#region Variables
	[SerializeField] private int _cost;
	static public CollectEvent CollectCoin;
	public int Cost
	{
		get
		{
			return _cost;
		}
		set
		{
			_cost = value;
		}
	}
	#endregion

	#region Unity Methods
	void Start () 
	{
		if (CollectCoin == null)
		{
			CollectCoin = new CollectEvent();
		}
	}

	#endregion
}
