/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSettings : MonoBehaviour {

	#region Variables
	// Location that material is for
	[SerializeField] private Location _location;
	// True if material is dangerous.
	// Dangerous materials do game over 
	// when player collide with them.
	[SerializeField] private bool _isDangerous;
	#endregion

	/// <summary>
	/// Return location that material is for.
	/// </summary>
	/// <returns></returns>
	public Location GetLocation()
	{
		return _location;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public bool IsDangerous()
	{
		return _isDangerous;
	}
}
