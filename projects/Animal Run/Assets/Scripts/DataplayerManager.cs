/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;

/// <summary>
/// Give access to:
/// namefile,
/// class with data
/// </summary>
public class DataplayerManager : MonoBehaviour {

	#region Variables
    public static DataplayerManager Instance;

	// Data that keeping information.
	public DataPlayer Data { get; set; }
	// Name of file.
	private string _nameFile = "playerInfo.dat";
	public string NameFile
	{
		get
		{
			return _nameFile;
		}
	}
	#endregion
}
