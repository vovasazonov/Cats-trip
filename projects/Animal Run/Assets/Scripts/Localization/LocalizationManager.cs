/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/
using System.Collections;
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

	private bool _isReady = false;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			_localizedText = new Dictionary<string, string>(0);
		}
	}
	
	/// <summary>
	/// Load localized text from file in the editor.
	/// </summary>
	/// <param name="fileName"></param>
	public void LoadLocalizedText(string fileName)
    {       
        _localizedText = new Dictionary<string, string>();

        string filePath = Path.Combine(Application.streamingAssetsPath+"/", fileName);
		Debug.Log("localized text path:" + filePath);

        if (File.Exists(filePath))
        {
			string dataAsJson;
			dataAsJson = File.ReadAllText(filePath);

			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                _localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + _localizedText.Count + " entries");

			_isReady = true;
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
	}
	/// <summary>
	/// Load localized text from file in the android.
	/// </summary>
	/// <param name="fileName"></param>
	IEnumerator LoadLocalizedTextOnAndroid(string fileName)
	{
		_localizedText = new Dictionary<string, string>();
		string filePath;
		filePath = Path.Combine(Application.streamingAssetsPath, fileName);
		string dataAsJson;

		UnityEngine.Networking.UnityWebRequest www = 
			UnityEngine.Networking.UnityWebRequest.Get(filePath);
		yield return www.SendWebRequest();
		if(www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			dataAsJson = www.downloadHandler.text;
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

			for (int i = 0; i < loadedData.items.Length; i++)
			{
				_localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
			}

			_isReady = true;
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

	/// <summary>
	/// Set language in dependence file language.
	/// </summary>
	/// <param name="fileName"></param>
	public void SetLanguage (string fileName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		StartCoroutine("LoadLocalizedTextOnAndroid", fileName);
#elif UNITY_EDITOR
		LoadLocalizedText(fileName);
#endif
	}

	public bool GetIsReady()
	{
		return _isReady;
	}

}