/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour {

	#region Variables
	// Empty object for create empty material on area
	[SerializeField] private GameObject _emptyObject;
	// Prefabs of areas (every location has 3 prefabs)
	[SerializeField] private List<GameObject> _areas;
	// Prefabs of dangerous materials
	[SerializeField] private List<GameObject> _dangerousMaterials;
	// Prefabs of safety materials
	[SerializeField] private List<GameObject> _safetyMaterials;
	// Gold coin, sliver coin and etc
	[SerializeField] private List<GameObject> _coins;
	//R G B A color backround of each location
	[SerializeField] private List<int> _colorLocation;
	// 
	public static Dictionary<int, List<GameObject>> Areas { get; private set; }
	public static Dictionary<int, List<GameObject>> DangerousMaterials { get; private set; }
	public static Dictionary<int, List<GameObject>> SafetyMaterials { get; private set; }

	#endregion

	#region Unity Methods
	private void Awake()
	{
		
	}

	void Start () 
	{
		
	}
	
	
	void Update () 
	{
		
	}

	#endregion
}
