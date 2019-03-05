/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*/

using UnityEngine;

/// <summary>
/// Don't destroy object.
/// </summary>
public class DontDetstroy : MonoBehaviour {

	#region Variables
	public static DontDetstroy instance;
	#endregion

	#region Unity Methods
	/// <summary>
	/// Destroy object if there are object
	/// that exist. Do not destroy first object.
	/// </summary>
	void Awake()
	{
		// There are not object yet.
		if (instance == null)
		{
			instance = this;
		}
		// There are object that exist.
		// Delete this object.
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		// On load another scene, don't destroy object.
		DontDestroyOnLoad(gameObject);
	}
	#endregion

}
