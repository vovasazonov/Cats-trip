/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

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
	public static QuestsManager instance;

	// Data that keeping information.
	public DataQuests Data { get; private set; }
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

}
