/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMenu : MonoBehaviour {

    // Contains all animals in the project
    private Animals _animals = new Animals();

	#region values of script moving location in backround of menu
	// Count of location
	const short countLoc = 1;                                
	// Player speed
    [SerializeField] private float _speed;
	// Empty object for create empty material on area
	[SerializeField] private GameObject _emptyObj;

	//if add location ADD case to 
	//method inputAreasListsToGeneralList

	// Prefabs of areas (every location has 3 prefabs)
	[SerializeField] private List<GameObject> _areasLoc1;
	// Prefabs of dangerous materials
	[SerializeField] private List<GameObject> _matDanLoc1;
	// Prefabs of safety materials
	[SerializeField] private List<GameObject> _matSafLoc1;    
      
    // All locations area and materials in one 
    private List<List<GameObject>> _areasLoc;
    private List<List<GameObject>> _matDanLoc;
    private List<List<GameObject>> _matSafLoc;

	// List of pieces areas in the game
	private List<GameObject> _piecesAreas = new List<GameObject>();
	// List of pieces materials in the game
	private List<GameObject> _piecesMaterials = new List<GameObject>();
	
	// One piece of material
	private GameObject _material;

	// Position without dangerous elements
	private Vector2 _posClear;
	// Position of one piece
	private Vector3 _pos;
	// Number of location in current score
	private short _location;
	// Five last pieces that have to deleted, false - are not deleted yet
	private bool[] _pieceColumn = new bool[5];                               
    #endregion

    private void Awake()
    {
        //dataAd = LoadSaveAd.Load();

        //show rate window
        //if (!dataAd.isRatedApp && dataAd.clicksOnRestartButton > 30)
        //{
        //    //show rate window
        //    GameObject.Find("CanvasClassic").transform.
        //        Find("WindowRate").gameObject.SetActive(true);
        //}
    }

    void Start()
    {
        #region initialization values script moving location in backround of menu
        _speed = 1f;
        _posClear = new Vector2(0, 0);
        InputAreasListsToGeneralList();

        // Set false that saying the last pieces are not deleting yet
        for (int i = 0; i != 5; i++)
            _pieceColumn[i] = false;

        // Game starts with first area location
        _location = 1;

        // Make a first pieces in game
        for (int line = -6; line != 7; line++)
        {
            AddAreaInLine(line, _location);
            AddMaterialInLine(line, 1, ref _posClear, false);
        }
        #endregion

        // Set values in menu where it necessery
        SetValuesInStart(_animals);
    }

    void Update()
    {
        #region run script moving location in backround of menu
        float deltaTime = Time.deltaTime;

        // Moving pieces
        for (int i = 0; i != _piecesAreas.Count; i++)
        {
            _piecesAreas[i].transform.Translate(Vector3.down * _speed * deltaTime);
            _piecesMaterials[i].transform.Translate(Vector3.down * _speed * deltaTime);
        }

        // Destroying and deleting pieces - add new pieces
        for (int i = 0; i != _piecesAreas.Count; i++)
        {
            _pos = _piecesAreas[i].transform.position;

            if (_pos.y <= -7f)
            {
                // Delete pieces
                Destroy(_piecesAreas[i]);
                _piecesAreas.RemoveAt(i);

                Destroy(_piecesMaterials[i]);
                _piecesMaterials.RemoveAt(i);

                // Add new pieces
                if (!_pieceColumn[0])
                {
                    AddAreaInLine(_pos.y + 6f + 7f, _location);
                    AddMaterialInLine(_pos.y + 6f + 7f, _location, ref _posClear);
                    _pieceColumn[0] = true;
                }
                else if (_pieceColumn[0] && !_pieceColumn[1])
                    _pieceColumn[1] = true;
                else if (_pieceColumn[1] && !_pieceColumn[2])
                    _pieceColumn[2] = true;
                else if (_pieceColumn[2] && !_pieceColumn[3])
                    _pieceColumn[3] = true;
                else if (_pieceColumn[3] && !_pieceColumn[4])
                {
                    for (int j = 0; j != 5; j++)
                        _pieceColumn[j] = false;
                }
            }
        }
        #endregion

    }

    #region methods that helps to run script moving location in backround of menu
    /// <summary>
    /// Input gameObject each location to generals Lists
    /// </summary>
    void InputAreasListsToGeneralList()
    {
        // All locations area and materials in one 
        _areasLoc = new List<List<GameObject>>();
        _matDanLoc = new List<List<GameObject>>();
        _matSafLoc = new List<List<GameObject>>();

        // Input gameObject each location to generals Lists
        for (int i = 0; i != countLoc; i++)
        {
            switch (i + 1)
            {
                case 1:
                    _areasLoc.Add(_areasLoc1);
                    _matDanLoc.Add(_matDanLoc1);
                    _matSafLoc.Add(_matSafLoc1);
                    break;
                default:
                    Debug.LogError("Case i < countLoc! Go to inputAreasListsToGeneralList Method");
                    break;
            }
        }
    }
    /// <summary>
    /// Add five pieces of area in line
    /// </summary>
    /// <param name="numberLine"></param>
    /// <param name="location"></param>
    void AddAreaInLine(float numberLine, short location)
    {
		// Number of location in list
		short locInList = (short)(location - 1);
		// Position of piece 
		Vector3 posInMethod;
		// Piece of area
		GameObject area;                                            

        // Create a left pieces
        area = Instantiate(_areasLoc[locInList][0]);      
        posInMethod = new Vector3(-2, numberLine, 0);                       
        area.transform.position = posInMethod;                             
        _piecesAreas.Add(area);                                     

        // Create a center pieces
        for (int column = -1; column != 2; column++)
        {
            area = Instantiate(_areasLoc[locInList][1]);
            posInMethod = new Vector3(column, numberLine, 0);
            area.transform.position = posInMethod;
            _piecesAreas.Add(area);
        }

        // Create a right pieces
        area = Instantiate(_areasLoc[locInList][2]);
        posInMethod = new Vector3(2, numberLine, 0);
        area.transform.position = posInMethod;
        _piecesAreas.Add(area);
    }
	/// <summary>
	/// Add five pieces of materials in line
	/// </summary>
	/// <param name="numberLine"></param>
	/// <param name="location"></param>
	/// <param name="posClear"></param>
	/// <param name="withDanElem"></param>
	void AddMaterialInLine(float numberLine, short location, ref Vector2 posClear, bool withDanElem = true)
    {
		// Position of piece 
		Vector3 posInMethod;                                  
        bool[] danPosAdd = new bool[5];

		for (int i = 0; i < 5; i++)
		{
			danPosAdd[i] = false;
		}

        for (int column = -2; column != 3; column++)
        {
            // Create safety material
            if (!withDanElem || posClear.x == column || Random.Range(0, 2) == 0)
            {
                _material = GetRandomMaterial(false, true);

                if (posClear.y == 1 && posClear.x == column)
                    posClear.y = 0;
            }
            // Create dangerous materials
            else
            {
                _material = GetRandomMaterial(true, true);
                danPosAdd[column + 2] = true;

            }
			// Create position of material
			posInMethod = new Vector3(column, numberLine, 0);
			// Transform material to position
			_material.transform.position = posInMethod;
			// Add new material to list
			_piecesMaterials.Add(_material);                      
        }

        // Create new position road where player can move safety
        RandomClearRoad(ref posClear, ref danPosAdd);
    }
    /// <summary>
    /// Create random road that
    /// gamer can run without
    /// dangerous materials
    /// </summary>
    /// <param name="posClear"></param>
    /// <param name="danPosAdd"></param>
    void RandomClearRoad(ref Vector2 posClear, ref bool[] danPosAdd)
    {
        int maxChanceY = 5;

        switch ((int)posClear.x)
        {
            case -2:
                if (Random.Range(0, maxChanceY) == 0)
                    posClear.y = 1;
                else
                {
                    if (!danPosAdd[1])
                        posClear.x = -1;
                    else
                        posClear.y = 1;
                }
                break;

            case 2:
                if (Random.Range(0, maxChanceY) == 0)
                    posClear.y = 1;
                else
                {
                    if (!danPosAdd[3])
                        posClear.x = 1;
                    else
                        posClear.y = 1;

                }
                break;

            case 1:
                if (Random.Range(0, maxChanceY) == 0)
                {
                    posClear.y = 1;
                }
                else if (Random.Range(0, 3) == 0)
                {
                    if (!danPosAdd[4])
                        posClear.x = 2;
                }
                else if (Random.Range(0, 3) == 0)
                    if (!danPosAdd[2])
                        posClear.x = 0;
                    else
                        posClear.y = 1;
                break;

            case -1:
                if (Random.Range(0, maxChanceY) == 0)
                    posClear.y = 1;

                else if (Random.Range(0, 3) == 0)
                {
                    if (!danPosAdd[0])
                        posClear.x = -2;
                }
                else if (Random.Range(0, 2) == 0)
                    if (!danPosAdd[2])
                        posClear.x = 0;
                    else
                        posClear.y = 1;
                break;

            case 0:
                if (Random.Range(0, maxChanceY) == 0)
                    posClear.y = 1;
                else if (Random.Range(0, 3) == 0)
                {
                    if (!danPosAdd[1])
                    {
                        posClear.x = -1;
                    }
                }
                else if (Random.Range(0, 2) == 0)
                    if (!danPosAdd[3])
                        posClear.x = 1;
                    else
                        posClear.y = 1;
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// Create random material on area
    /// </summary>
    /// <param name="danElem"></param>
    /// <param name="withCoin"></param>
    /// <returns></returns>
    GameObject GetRandomMaterial(bool danElem, bool withCoin)
    {
        GameObject material = null;
		// True if position already has material
		bool alreadyMat = false;                        

        if (!alreadyMat && Random.Range(0, 3) == 0)
        {
            material = Instantiate(_emptyObj);
            alreadyMat = true;
        }

        if (danElem && !alreadyMat)
        {
            material = Instantiate(_matDanLoc[_location - 1][
				Random.Range(0, _matDanLoc[_location - 1].Count)]);
            alreadyMat = true;
        }
        else if (!alreadyMat)
        {
            material = Instantiate(_matSafLoc[_location - 1][
				Random.Range(0, _matSafLoc[_location - 1].Count)]);
            alreadyMat = true;
        }

        return material;
    }
    #endregion

    /// <summary>
    /// Set data values in window menu when it necessery
    /// </summary>
    /// <param name="animals">inclde an object with cost and id of animals</param>
    static public void SetValuesInStart(Animals animals)
    {
        // Check if the list has animals
        if (DataplayerManager.Instance.Data.BoughtAnimals == null)
        {

			DataplayerManager.Instance.Data.BoughtAnimals = new List<int>();
			DataplayerManager.Instance.Data.BoughtAnimals.Add((int)Animal.GrayCat);
			DataplayerManager.Instance.Data.CurrentAnimal = (int)Animal.GrayCat;

			// Save information
			LoadSave.Save(DataplayerManager.Instance.Data);//, 
				//DataplayerManager.Instance.NameFile,
				//true);
        }
        
        // Set the best score in block in main menu
        GameObject.Find("TextScoreNum").GetComponent<Text>().text =
			DataplayerManager.Instance.Data.Score.ToString();

        //// Set music
        //if (DataplayerManager.Instance.Data.IsMusicMainMenu)
        //{
        //    GameObject.Find("CanvasClassic").transform.Find("ButtonMusic").gameObject.
        //    GetComponent<Toggle>().isOn = true;
        //    GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = true;
        //}
        //else
        //{
        //    GameObject.Find("CanvasClassic").transform.Find("ButtonMusic").gameObject.
        //    GetComponent<Toggle>().isOn = false;
        //    GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;
        //}

        #region set values in shop
        // Set count of money in block in shop
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").Find("BlockShowMoney").
            Find("Text").GetComponent<Text>().text = 
			DataplayerManager.Instance.Data.Coins.ToString();

        // Find section of animals in canvas classic
        GameObject sectionAnimals = GameObject.Find("CanvasClassic").
            transform.Find("WindowShop").Find("SectionAnimals").gameObject;

        // Show && hide blocks with cost and another things with animal's icon in shop
        for(int i = 0; i != animals.AnimalsInShop.Count; i++)
        {
            GameObject anAnimal = sectionAnimals.transform.
				Find(animals.AnimalsInShop[i].ToString()).gameObject;

            // Set cost of animal, set Vi (if bought)
            if (DataplayerManager.Instance.Data.
				BoughtAnimals.Contains(animals.AnimalsInShop[i]))
            {
                // Costumer see vi
                anAnimal.transform.Find("Vi").gameObject.SetActive(true);
                // Do that costumer can't do click
                anAnimal.GetComponent<Toggle>().interactable = false;
                // Hide blocks with cost
                anAnimal.transform.Find("BlockGreen").gameObject.SetActive(false);
                anAnimal.transform.Find("BlockRed").gameObject.SetActive(false);

            }
            else
            {
                // Hide vi (without vi it says that the animal not bought yet)
                anAnimal.transform.Find("Vi").gameObject.SetActive(false);
                // Costumer can do click
                anAnimal.GetComponent<Toggle>().interactable = true;

                // Show cost of animal with green block if costumer has enogh money
                if (DataplayerManager.Instance.Data.Coins >= animals.CostAnimals[i])
                {
                    // Show green block
                    anAnimal.transform.Find("BlockGreen").gameObject.SetActive(true);
                    // Set cost of animal
                    anAnimal.transform.Find("BlockGreen").Find("Text").GetComponent<Text>().
                        text = animals.CostAnimals[i].ToString();

                    // Hide red block
                    anAnimal.transform.Find("BlockRed").gameObject.SetActive(false);
                }
                // Show cost of animal with red block
                else
                {
                    // Show red block
                    anAnimal.transform.Find("BlockRed").gameObject.SetActive(true);
                    // Set cost of animal
                    anAnimal.transform.Find("BlockRed").Find("Text").GetComponent<Text>().
                        text = animals.CostAnimals[i].ToString();

                    // Hide green block
                    anAnimal.transform.Find("BlockGreen").gameObject.SetActive(false);
                }
            }
        }
        #endregion

        #region set values in profile
        // Find section animals in profile
        GameObject sectionAnimalsProfile = GameObject.Find("CanvasClassic").
            transform.Find("WindowProfile").Find("SectionAnimals").gameObject;

        for (int i = 0; i != animals.AnimalsInShop.Count; i++)
        {
            // Find an animal icon to do some monipulations with it
            GameObject anAnimal = sectionAnimalsProfile.transform.
				Find(animals.AnimalsInShop[i].ToString()).gameObject;

            // Do monipulations if the animal bought
            if (DataplayerManager.Instance.Data.BoughtAnimals.Contains(i))
            {
                // Show icon of animal in profile
                anAnimal.SetActive(true);

                // Set vi and another when the icon is current animal
                if (DataplayerManager.Instance.Data.CurrentAnimal == i)
                {
                    anAnimal.GetComponent<Toggle>().isOn = true;
                    anAnimal.GetComponent<Toggle>().interactable = false;
                    anAnimal.transform.Find("Vi").gameObject.SetActive(true);
                }
                else
                {
                    anAnimal.GetComponent<Toggle>().isOn = false;
                    anAnimal.GetComponent<Toggle>().interactable = true;
                    anAnimal.transform.Find("Vi").gameObject.SetActive(false);
                }
            }
            else
            {
                // Hide icon of animal in profile
                anAnimal.SetActive(false);
                // Hide vi
                anAnimal.transform.Find("Vi").gameObject.SetActive(false);
            }
        }

		#endregion

		#region set values in quests
		DataQuests dataQuests = QuestsManager.Instance.Data;

        if (dataQuests.WasTodayRewardVideo)
        {
            // Hide button
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("ButtonRewardVideo").gameObject.SetActive(false);
            // Show text that there are not rewarded
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("NoGiftsText").gameObject.SetActive(true);
        }
        else
        {
            // Show button
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("ButtonRewardVideo").gameObject.SetActive(true);
            // Hide text that there are not rewarded
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("NoGiftsText").gameObject.SetActive(false);
        }
        #endregion
    }


}
