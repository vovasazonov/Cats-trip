using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    //return value isReady
    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }

    //when click on button change languege
    public void LoadLocalizedText(string fileName)
    {       
        localizedText = new Dictionary<string, string>();

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
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    //set localized value on start
    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;

    }

}