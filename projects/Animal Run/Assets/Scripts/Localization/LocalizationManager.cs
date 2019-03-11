/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Manage with languages in
/// the game.
/// </summary>
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
	
	// Dictionary with key and word for it
    private Dictionary<string, string> _localizedText;
	
    private string _missingTextString = "Localized text not found";

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			_localizedText = new Dictionary<string, string>(0);
		}
	}
	
	/// <summary>
	/// Load localized text from file
	/// </summary>
	/// <param name="fileName"></param>
	public void LoadLocalizedText(string fileName)
    {       
        _localizedText = new Dictionary<string, string>();

        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string dataAsJson;

        if (File.Exists(filePath))
        {
#if UNITY_EDITOR
            dataAsJson = File.ReadAllText(filePath);
#elif UNITY_ANDROID && !UNITY_EDITOR
            WWW reader = new WWW(filePath);
            while (!reader.isDone){}
            dataAsJson = reader.text;
#else
            Debug.LogError("unexpected_platform");
#endif
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                _localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + _localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

	/// <summary>
	/// Get value by key of word
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public string GetLocalizedValue(string key)
    {
        string result = _missingTextString;

        if (_localizedText.ContainsKey(key))
        {
            result = _localizedText[key];
        }

        return result;
    }

}