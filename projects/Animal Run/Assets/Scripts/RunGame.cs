using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class RunGame : MonoBehaviour {
    DataPlayer dataPlayer;                                   //data from file data
    DataPlayer dataPlayerOnScript;                           //data from script while game running
    float deltaTime;

    [SerializeField] private Text textScoreBlockBlack;

    [SerializeField] private List<GameObject> players;       //all animals in the game
    
    #region locations and their values
    const short countLoc = 2;                                //saying how many locations in the game

    [SerializeField] private float speed;                   //speed game

    [SerializeField] private GameObject emptyObj;            //empty object for create empty material on area

    [SerializeField] private List<GameObject> coins;         //gold coin, sliver coin and etc

    [SerializeField] private List<int> colorLocation;       //R G B A color backround of each location

    //if add location ADD case to 
    //method inputAreasListsToGeneralList
    [SerializeField] private List<GameObject> areasLoc1;     //prefabs of areas (every location has 3 prefabs)
    [SerializeField] private List<GameObject> matDanLoc1;    //prefabs of dangerous materials
    [SerializeField] private List<GameObject> matSafLoc1;    //prefabs of safety materials
    [SerializeField] private List<GameObject> areasLoc2;                            
    [SerializeField] private List<GameObject> matDanLoc2;                           
    [SerializeField] private List<GameObject> matSafLoc2;

    private int score;                                     //get score from start to gameOver                     
    static public int CountMoney { get; set; }             //get money from start to gameOver
    private int currentPlayer;                                //get the player that choose

    static public bool isGameOver { get; set; }              // get the value from another script if it necessery to stop the game
    static public bool isPause { get; set; }                 // get the value from another script if it necessery to stop the game
    private bool isSave;    //if the file was save it true

    //all locations area and materials in one 
    private List<List<GameObject>> areasLoc;
    private List<List<GameObject>> matDanLoc;
    private List<List<GameObject>> matSafLoc;

    private List<GameObject> piecesAreas = new List<GameObject>();          //List of pieces areas in the game
    private List<GameObject> piecesMaterials = new List<GameObject>();      //List of pieces materials in the game

    //private GameObject area;                                                //one piece of area
    private GameObject material;                                            //one piece of material
    private GameObject player;                                              //player on game

    private Vector2 posClear;                                               //position without dangerous elements
    private Vector3 pos;                                                    //position of one piece
    private short location;                                                 //number of location in current score
    private bool[] pieceColumn = new bool[5];                               //five last pieces that have to deleted, false - are not deleted yet
    #endregion

    void Start()
    {
        isPause = false;
        isGameOver = false;
        isSave = false;

        //load data player from file and set it to another object to equals its values in future
        dataPlayer = LoadSavePlayer.Load();
        dataPlayerOnScript = (DataPlayer)dataPlayer.Clone();

        speed = 3f;
        score = 0;
        CountMoney = 0;
        currentPlayer = dataPlayerOnScript.currentAnimal;
        player = Instantiate<GameObject>(players[currentPlayer]);
        player.transform.position = new Vector3(0, -2, 0);
        posClear = new Vector2(0, 0);
        InputAreasListsToGeneralList();

        //set music
        if (dataPlayer.isMusicMainMenu)
        {
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;
        }

        //set false that saying the last pieces are not deleting yet
        for (int i = 0; i != 5; i++)
        pieceColumn[i] = false;

        //game starts with first area location
        location = 1;

        //make a first pieces in game
        for (int line = -6; line != 7; line++)
        {
            AddAreaInLine(line, location);
            AddMaterialInLine(line, 1, ref posClear, false);
        }
    }

    void Update()
    {
        deltaTime = Time.deltaTime;

        //Debug.Log((Vector3.down * speed * deltaTime).y);

        if (isGameOver)
        {
            //saveData
            if (!isSave)
            {
                if (dataPlayerOnScript.score < score) dataPlayerOnScript.score = score;
                dataPlayerOnScript.coins += CountMoney;

                if (!EqualsTwoObjects()) SaveNewData();
                isSave = true;

                //do animation sit of animal
                player.GetComponent<Animator>().SetBool("StopToSit",true);

                //set off music
                GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;

                //load gameover window
                StartCoroutine(Wait(2f, () => 
                {
                    GameObject.Find("Canvas").transform.Find("GameOverWindow").Find("Score").
                    Find("TextGetScore").GetComponent<Text>().text = score.ToString();
                    GameObject.Find("Canvas").transform.Find("GameOverWindow").Find("Coin").
                    Find("TextGetCoin").GetComponent<Text>().text = CountMoney.ToString();

                    GameObject.Find("Canvas").transform.Find("GameOverWindow").gameObject.SetActive(true);
                } 
                ));
            }
        }
        else
        {
            //set text on block
            textScoreBlockBlack.text = score.ToString();

            //moving pieces -----------before1
            for (int i = 0; i != piecesAreas.Count; i++)
            {
                piecesAreas[i].transform.Translate(Vector3.down * speed * deltaTime);
                piecesMaterials[i].transform.Translate(Vector3.down * speed * deltaTime);
            }

            //destroying and deleting pieces - add new pieces
            for (int i = 0; i != piecesAreas.Count; i++)
            {
                pos = piecesAreas[i].transform.position;

                if (pos.y <= -7f)
                {
                    //delete line and add line
                    for (int  r = 0; r != 5; r++)
                    {
                        //delete piece of area
                        Destroy(piecesAreas[0]);
                        piecesAreas.RemoveAt(0);
                        //delete piece of material
                        Destroy(piecesMaterials[0]);
                        piecesMaterials.RemoveAt(0);


                        //add new pieces in line
                        if (!pieceColumn[0])
                        {
                            AddAreaInLine(pos.y + 6f + 7f, location);
                            AddMaterialInLine(pos.y + 6f + 7f, location, ref posClear);
                            pieceColumn[0] = true;
                            score++;            //add score

                            //increse speed
                            if (score % 50 == 0 && speed < 6)
                                speed += 0.3f;
                        }
                        else if (pieceColumn[0] && !pieceColumn[1])
                            pieceColumn[1] = true;
                        else if (pieceColumn[1] && !pieceColumn[2])
                            pieceColumn[2] = true;
                        else if (pieceColumn[2] && !pieceColumn[3])
                            pieceColumn[3] = true;
                        else if (pieceColumn[3] && !pieceColumn[4])
                        {
                            for (int j = 0; j != 5; j++)
                                pieceColumn[j] = false;
                        }
                    }
                }
            }

            //change location
            if (countLoc != location && score >= location * 150)
            {
                location++;

                //set default speed
                speed = (float)(3+0.5*(location-1))<4 ? (float)(3 + 0.5 * (location - 1)) : 4;

                //change background color of camera4
                Camera.main.GetComponent<Camera>().backgroundColor = new Color(
                    colorLocation[location * 4 - 4] / 255, 
                    colorLocation[location * 4 - 3] / 255, 
                    colorLocation[location * 4 - 2] / 255, 
                    colorLocation[location * 4 - 1] / 255);                           
            }

        }

        //check if player input escape menu or home
        if((Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu) || 
            Input.GetKeyDown(KeyCode.Home)) && !isGameOver)
        {
            GameObject.Find("Canvas").transform.Find("PauseWindow").gameObject.SetActive(true);

            //here will I add ads window random
            if (!isPause && Random.Range(0, 5) == Random.Range(0, 5))
            {
                //here show ad
                //AdScript.showAds(true);
            }

            isPause = true;
            Time.timeScale = 0;
        }
    }


    /// <summary>
    /// the delegate for send method like param in another method
    /// </summary>
    delegate void someMethod();
    /// <summary>
    /// wait some time before start some method
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
    /// input gameObject each location to generals Lists
    /// </summary>
    void InputAreasListsToGeneralList()
    {
        //all locations area and materials in one 
        areasLoc = new List<List<GameObject>>();
        matDanLoc = new List<List<GameObject>>();
        matSafLoc = new List<List<GameObject>>();

        //input gameObject each location to generals Lists
        for (int i = 0; i != countLoc; i++)
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
        short locInList = (short)(location-1);                      //number of location in list
        Vector3 posInMethod;                                        //position of piece                     
        GameObject area;                                            //piece of area

        //create a left pieces
        area = Instantiate<GameObject>(areasLoc[locInList][0]);      //create a new left piece
        posInMethod = new Vector3(-2, numberLine, 0);                       //set a position of piece
        area.transform.position = posInMethod;                              //transform piece
        piecesAreas.Add(area);                                      //add to list

        //create a center pieces
        for (int column = -1; column != 2; column++)
        {
            area = Instantiate<GameObject>(areasLoc[locInList][1]);
            posInMethod = new Vector3(column, numberLine, 0);
            area.transform.position = posInMethod;
            piecesAreas.Add(area);
        }

        //create a right pieces
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
    void AddMaterialInLine(float numberLine, short location, ref Vector2 posClear, bool withDanElem = true)
    {
        Vector3 posInMethod;                                //position of piece   
        bool[] danPosAdd = new bool[5];

        for (int i = 0; i != 5; i++) danPosAdd[i] = false;

        for (int column = -2; column != 3; column++)
        {
            //create safety material
            if (!withDanElem || posClear.x == column || Random.Range(0, 2) == 0)
            {
                material = GetRandomMaterial(false, true);

                if (posClear.y == 1 && posClear.x == column)
                    posClear.y = 0;
            }
            //create dangerous materials
            else
            {
                material = GetRandomMaterial(true, true);
                danPosAdd[column + 2] = true;

            }

            posInMethod = new Vector3(column, numberLine, 0);   //create position of material
            material.transform.position = posInMethod;          //transform material to position
            piecesMaterials.Add(material);                      //add new material to list
        }

        //create new position road where player can move safety
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
    /// create random material on area
    /// </summary>
    /// <param name="danElem"></param>
    /// <param name="withCoin"></param>
    /// <returns></returns>
    GameObject GetRandomMaterial(bool danElem, bool withCoin)
    {
        GameObject material = null;                     //material that will create
        bool alreadyMat = false;                        //position already has material or not (false - not)

        //add coins to game
        if (withCoin && Random.Range(0, 20) == 0)
        {
            if (Random.Range(0, 3) == 0)
            {
                //coin sliver
                material = Instantiate<GameObject>(coins[0]);
                alreadyMat = true;
            }
            else if (Random.Range(0, location < 10 ? 10 - location : 1) == 0)
            {
                //coin gold
                material = Instantiate<GameObject>(coins[1]);
                alreadyMat = true;
            }
        }
        //add empty object to game
        if(!alreadyMat && Random.Range(0, 20) == 0)
        {
            material = Instantiate<GameObject>(emptyObj);
            alreadyMat = true;
        }
        //add dangerous element to game
        if (danElem && !alreadyMat)
        {
            material = Instantiate<GameObject>(matDanLoc[location - 1]
                [Random.Range(0, matDanLoc[location - 1].Count)]);
            alreadyMat = true;
        }
        //add safety element to game
        else if(!alreadyMat)
        {
            material = Instantiate<GameObject>(matSafLoc[location - 1]
                [Random.Range(0, matSafLoc[location - 1].Count)]);
            alreadyMat = true;
        }

        return material;
    }
    /// <summary>
    /// return if the dataPlayer objects are the same
    /// </summary>
    /// <returns></returns>
    public bool EqualsTwoObjects()
    {
        if (dataPlayer == dataPlayerOnScript)
            return true;
        else
            return false;
    }
    /// <summary>
    /// Save new data
    /// </summary>
    public void SaveNewData()
    {
        LoadSavePlayer.Save(dataPlayerOnScript);
        dataPlayer = (DataPlayer)dataPlayerOnScript.Clone();
    }
}


