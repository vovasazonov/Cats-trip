/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*/

using UnityEngine;

/// <summary>
/// Don't destroy game object.
/// </summary>
public class DontDetstroy : MonoBehaviour {

	#region Variables
	private static DontDetstroy _instance;
	#endregion

	#region Unity Methods
	/// <summary>
	/// Destroy object if there are object
	/// that exist. Do not destroy first object.
	/// </summary>
	void Awake()
	{
		// There are not object yet.
		if (_instance == null)
		{
			_instance = this;
		}
		// There are object that exist.
		// Delete this object.
		else if (_instance != this)
		{
			Destroy(gameObject);
		}

		// On load another scene, don't destroy object.
		DontDestroyOnLoad(gameObject);
	}
	#endregion

}
