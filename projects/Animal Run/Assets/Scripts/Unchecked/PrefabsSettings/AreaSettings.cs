/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSettings : MonoBehaviour {

	#region Variables
	// Location that area is for.
	[SerializeField]
	private Location _location;
	// Side that area is in the line.
	[SerializeField, Tooltip("(Left)-(Center)-(Center)-(Center)-(Right)")]
	private SideArea _sideArea;
	#endregion

	/// <summary>
	/// Return location that area is for.
	/// </summary>
	/// <returns></returns>
	public Location GetLocation()
	{
		return _location;
	}
	/// <summary>
	/// Return side that area is in the line.
	/// </summary>
	/// <returns></returns>
	public SideArea GetSide()
	{
		return _sideArea;
	}
}
