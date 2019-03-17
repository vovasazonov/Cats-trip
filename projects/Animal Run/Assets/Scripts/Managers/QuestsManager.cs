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
public class QuestsManager : MonoBehaviour {

	#region Variables
	public static QuestsManager Instance;

	// Data that keeping information.
	public DataQuests Data { get; set; }
	// Name of file.
	private string _nameFile = "questsInfo.dat";
	public string NameFile
	{
		get
		{
			return _nameFile;
		}
	}
	#endregion

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			Instance.Data = new DataQuests();
		}
	}
}
