/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this script to text component.
/// It translate the language in depences 
/// of langueage in settings.
/// </summary>
public class LocalizedText : MonoBehaviour
{
    public string key;

    void Start()
    {
        Text text = GetComponent<Text>();
        text.text = LocalizationManager.Instance.GetLocalizedValue(key);
    }
}