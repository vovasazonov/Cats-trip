/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Move pieces of materials.
/// </summary>
public class RunGame : MonoBehaviour {

	// Temporary data for using in script
	private DataPlayer _temporaryDataPlayer;
	// Set link of data player from dataplayermanager
	private DataPlayer _dataPlayer;
	// The time between updates
	private float _deltaTime;
	// Text of score in the bottom of game 
	// (drag gameobject text score from inspector).
    [SerializeField] private Text _textScoreBlockBlack;
	// All animals in the game
	[SerializeField] private List<GameObject> players;

	#region locations and their values
	// Count of locations in the game
	const short _countLoc = 2;                                
	// Speed of player.
    [SerializeField] private float _speed;                   
	// Empty object for create empty material on area
    [SerializeField] private GameObject _emptyObj;
	// Gold coin, sliver coin and etc
	[SerializeField] private List<GameObject> _coins;         
	//R G B A color backround of each location
    [SerializeField] private List<int> _colorLocation;       

    //if add location ADD case to 
    //method inputAreasListsToGeneralList
    [SerializeField] private List<GameObject> areasLoc1;     //prefabs of areas (every location has 3 prefabs)
    [SerializeField] private List<GameObject> matDanLoc1;    //prefabs of dangerous materials
    [SerializeField] private List<GameObject> matSafLoc1;    //prefabs of safety materials
    [SerializeField] private List<GameObject> areasLoc2;                            
    [SerializeField] private List<GameObject> matDanLoc2;                           
    [SerializeField] private List<GameObject> matSafLoc2;

	// Score from start game to game over  
	private int _score;
	// Count of money from start to gameOver
	static public int CountMoney { get; set; }
	// Skin that user choosed
	private int _currentPlayer;

    static public bool isGameOver { get; set; }             
    static public bool isPause { get; set; }      
	// True if the file alreday saved
    private bool _isSave;

    // All locations area and materials in one 
    private List<List<GameObject>> areasLoc;
    private List<List<GameObject>> matDanLoc;
    private List<List<GameObject>> matSafLoc;

	//List of pieces areas in the game
	private List<GameObject> piecesAreas = new List<GameObject>();
	//List of pieces materials in the game
	private List<GameObject> piecesMaterials = new List<GameObject>();

	// Piece of material
	private GameObject _material;
	//player on game.
	private GameObject _player;

	// Position without dangerous elements
	private Vector2 _posClear;
	// Position of one piece
	private Vector3 _pos;
	// Number of location in current score
	private short _location;
	// Five last pieces that have to deleted, false - are not deleted yet
	private bool[] _pieceColumn = new bool[5];                               
    #endregion

    void Start()
    {
		_dataPlayer = DataplayerManager.Instance.Data;

		isPause = false;
        isGameOver = false;
        _isSave = false;

		// Clone data to temporary data.
		_temporaryDataPlayer = (DataPlayer)_dataPlayer.Clone();

		_speed = 3f;
        _score = 0;
        CountMoney = 0;
        _currentPlayer = _temporaryDataPlayer.CurrentAnimal;
        _player = Instantiate<GameObject>(players[_currentPlayer]);
        _player.transform.position = new Vector3(0, -2, 0);
        _posClear = new Vector2(0, 0);
        InputAreasListsToGeneralList();

        // Set music
        if (_dataPlayer.IsMusicMainMenu)
        {
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;
        }

		// Set false that saying the last pieces 
		// are not deleting yet.
        for (int i = 0; i != 5; i++)
        _pieceColumn[i] = false;

        // Game starts with first area location
        _location = 1;

        // Instantiate first pieces in game.
        for (int line = -6; line != 7; line++)
        {
            AddAreaInLine(line, _location);
            AddMaterialInLine(line, 1, ref _posClear, false);
        }
    }

    void Update()
    {
        _deltaTime = Time.deltaTime;

        if (isGameOver)
        {
            //saveData
            if (!_isSave)
            {
                // Do animation sit of animal
                _player.GetComponent<Animator>().SetBool("StopToSit",true);

                // Set off music
                GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;

                if (_temporaryDataPlayer.Score < _score)
					_temporaryDataPlayer.Score = _score;

                _temporaryDataPlayer.Coins += CountMoney;

                if (!EqualsTwoDataPlayer())
					SaveNewData();

                _isSave = true;

                // Show gameover window
                StartCoroutine(Wait(2f, () => 
                {
                    GameObject.Find("Canvas").transform.Find("GameOverWindow").Find("Score").
                    Find("TextGetScore").GetComponent<Text>().text = _score.ToString();
                    GameObject.Find("Canvas").transform.Find("GameOverWindow").Find("Coin").
                    Find("TextGetCoin").GetComponent<Text>().text = CountMoney.ToString();

                    GameObject.Find("Canvas").transform.Find("GameOverWindow").gameObject.SetActive(true);
                } 
                ));
            }
        }
        else
        {
            // Set text on block
            _textScoreBlockBlack.text = _score.ToString();

            // Moving pieces -----------before1
            for (int i = 0; i != piecesAreas.Count; i++)
            {
                piecesAreas[i].transform.Translate(Vector3.down * _speed * _deltaTime);
                piecesMaterials[i].transform.Translate(Vector3.down * _speed * _deltaTime);
            }

            // Destroying and deleting pieces, add new pieces
            for (int i = 0; i != piecesAreas.Count; i++)
            {
                _pos = piecesAreas[i].transform.position;

                if (_pos.y <= -7f)
                {
                    // Delete line and add line
                    for (int  r = 0; r != 5; r++)
                    {
                        //Delete first piece of area in list
                        Destroy(piecesAreas[0]);
                        piecesAreas.RemoveAt(0);
                        // Delete first piece of material in list
                        Destroy(piecesMaterials[0]);
                        piecesMaterials.RemoveAt(0);


                        // Add new pieces in line
                        if (!_pieceColumn[0])
                        {
                            AddAreaInLine(_pos.y + 6f + 7f, _location);
                            AddMaterialInLine(_pos.y + 6f + 7f, _location, ref _posClear);
                            _pieceColumn[0] = true;
							
							// Add score
							_score++;            

                            // Increse speed
                            if (_score % 50 == 0 && _speed < 6)
                                _speed += 0.3f;
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
            }

            // Change location
            if (_countLoc != _location && _score >= _location * 150)
            {
                _location++;

                // Set new speed for location
                _speed = (float)(3+0.5*(_location-1))<4 ? (float)(3 + 0.5 * (_location - 1)) : 4;

                // Change background color of camera4
                Camera.main.GetComponent<Camera>().backgroundColor = new Color(
                    _colorLocation[_location * 4 - 4] / 255, 
                    _colorLocation[_location * 4 - 3] / 255, 
                    _colorLocation[_location * 4 - 2] / 255, 
                    _colorLocation[_location * 4 - 1] / 255);                           
            }

        }

        // Check if player input escape menu or home (on android)
        if((Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu) || 
            Input.GetKeyDown(KeyCode.Home)) && !isGameOver)
        {
            GameObject.Find("Canvas").transform.Find("PauseWindow").gameObject.SetActive(true);

            isPause = true;
            Time.timeScale = 0;
        }
    }


    /// <summary>
    /// The delegate for send method like param in another method
    /// </summary>
    delegate void someMethod();
    /// <summary>
    /// Wait some time before start some method
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    IEnumerator Wait(float seconds, someMethod method)
    {
        yield return new WaitForSeconds(seconds);
        method();
    }    
    /// <summary>
    /// Add gameObjects each location to generals Lists
    /// </summary>
    void InputAreasListsToGeneralList()
    {
        // All locations area and materials in one 
        areasLoc = new List<List<GameObject>>();
        matDanLoc = new List<List<GameObject>>();
        matSafLoc = new List<List<GameObject>>();

        for (int i = 0; i != _countLoc; i++)
        {
            switch (i+1)
            {
                case 1:
                    areasLoc.Add(areasLoc1);
                    matDanLoc.Add(matDanLoc1);
                    matSafLoc.Add(matSafLoc1);
                    break;
                case 2:
                    areasLoc.Add(areasLoc2);
                    matDanLoc.Add(matDanLoc2);
                    matSafLoc.Add(matSafLoc2);
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
		short locInList = (short)(location-1);
		// Position of piece  
		Vector3 posInMethod;
		// Piece of area
		GameObject area;                                            

        // Create a left pieces
        area = Instantiate<GameObject>(areasLoc[locInList][0]);
		// Set a position of piece
		posInMethod = new Vector3(-2, numberLine, 0);
		// Transform piece
		area.transform.position = posInMethod;
		// Add to list
		piecesAreas.Add(area);                                      

        // Create a center pieces
        for (int column = -1; column != 2; column++)
        {
            area = Instantiate<GameObject>(areasLoc[locInList][1]);
            posInMethod = new Vector3(column, numberLine, 0);
            area.transform.position = posInMethod;
            piecesAreas.Add(area);
        }

        // Create a right pieces
        area = Instantiate<GameObject>(areasLoc[locInList][2]);
        posInMethod = new Vector3(2, numberLine, 0);
        area.transform.position = posInMethod;
        piecesAreas.Add(area);
    }
    /// <summary>
    /// Add five pieces of materials in line
    /// </summary>
    /// <param name="numberLine"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    void AddMaterialInLine(float numberLine, short location, 
		ref Vector2 posClear, bool withDanElem = true)
    {
		// Position of piece
		Vector3 posInMethod;    
		// True if added
        bool[] danPosAdd = new bool[5];

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
			// Add new material to list with exist materials
            piecesMaterials.Add(_material);      
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
    void RandomClearRoad (ref Vector2 posClear, ref bool[] danPosAdd)
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
    /// <param name="danElem">Create dangerous elements</param>
    /// <param name="withCoin">Create coins</param>
    /// <returns></returns>
    GameObject GetRandomMaterial(bool danElem, bool withCoin)
    {
		// Material to create.
		GameObject material = null;
		// True if position already has material
		bool alreadyMaterial = false;                        

        // Add coins to game
        if (withCoin && Random.Range(0, 20) == 0)
        {
            if (Random.Range(0, 3) == 0)
            {
                // Add coin sliver
                material = Instantiate(_coins[0]);

                alreadyMaterial = true;
            }
            else if (Random.Range(0, _location < 10 ? 10 - _location : 1) == 0)
            {
                // Add coin gold
                material = Instantiate(_coins[1]);

                alreadyMaterial = true;
            }
        }
        // Add empty object to game.
        if(!alreadyMaterial && Random.Range(0, 20) == 0)
        {
            material = Instantiate(_emptyObj);

            alreadyMaterial = true;
        }

		// Add dangerous element according to the location
		if (danElem && !alreadyMaterial)
        {
			material = Instantiate(matDanLoc[_location - 1][Random.Range(
				0, matDanLoc[_location - 1].Count)]);

            alreadyMaterial = true;
        }
		// Add safety element according to the location
		else if (!alreadyMaterial)
        {
            material = Instantiate(matSafLoc[_location - 1][Random.Range(
				0, matSafLoc[_location - 1].Count)]);

            alreadyMaterial = true;
        }

        return material;
    }
    /// <summary>
    /// Return true if the dataPlayer objects are the same
    /// </summary>
    /// <returns></returns>
    public bool EqualsTwoDataPlayer()
    {
        if (_dataPlayer == _temporaryDataPlayer)
            return true;
        else
            return false;
    }
    /// <summary>
    /// Save new data
    /// </summary>
    public void SaveNewData()
    {
		// Save data.
		LoadSave.Save(_temporaryDataPlayer, DataplayerManager.Instance.NameFile);

		// Clone data to static class of data.
        _dataPlayer = (DataPlayer)_temporaryDataPlayer.Clone();
    }
}


