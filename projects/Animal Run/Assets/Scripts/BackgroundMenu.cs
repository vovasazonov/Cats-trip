using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMenu : MonoBehaviour {

    //object that keep information about customer
    //static public DataPlayer dataPlayer;

    //keep information about rating app
    //private DataAd dataAd = new DataAd();

    //contains all animals in the project
    private Animals animals = new Animals();

    #region values of script moving location in backround of menu
    const short countLoc = 1;                                //saying how many locations in the game

    [SerializeField] private float speed;                    //speed game

    [SerializeField] private GameObject emptyObj;            //empty object for create empty material on area

    //if add location ADD case to 
    //method inputAreasListsToGeneralList
    [SerializeField] private List<GameObject> areasLoc1;     //prefabs of areas (every location has 3 prefabs)
    [SerializeField] private List<GameObject> matDanLoc1;    //prefabs of dangerous materials
    [SerializeField] private List<GameObject> matSafLoc1;    //prefabs of safety materials
      
    //all locations area and materials in one 
    private List<List<GameObject>> areasLoc;
    private List<List<GameObject>> matDanLoc;
    private List<List<GameObject>> matSafLoc;

    private List<GameObject> piecesAreas = new List<GameObject>();          //List of pieces areas in the game
    private List<GameObject> piecesMaterials = new List<GameObject>();      //List of pieces materials in the game

    private GameObject material;                                            //one piece of material

    private Vector2 posClear;                                               //position without dangerous elements
    private Vector3 pos;                                                    //position of one piece
    private short location;                                                 //number of location in current score
    private bool[] pieceColumn = new bool[5];                               //five last pieces that have to deleted, false - are not deleted yet
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
        speed = 1f;
        posClear = new Vector2(0, 0);
        InputAreasListsToGeneralList();

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
        #endregion

        //set values in menu where it necessery
        setValuesInStart(animals);
    }

    void Update()
    {
        #region run script moving location in backround of menu
        float deltaTime = Time.deltaTime;

        //moving pieces
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
                //delete pieces
                Destroy(piecesAreas[i]);
                piecesAreas.RemoveAt(i);

                Destroy(piecesMaterials[i]);
                piecesMaterials.RemoveAt(i);

                //add new pieces
                if (!pieceColumn[0])
                {
                    AddAreaInLine(pos.y + 6f + 7f, location);
                    AddMaterialInLine(pos.y + 6f + 7f, location, ref posClear);
                    pieceColumn[0] = true;
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
        #endregion

    }

    #region methods that helps to run script moving location in backround of menu
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
            switch (i + 1)
            {
                case 1:
                    areasLoc.Add(areasLoc1);
                    matDanLoc.Add(matDanLoc1);
                    matSafLoc.Add(matSafLoc1);
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
        short locInList = (short)(location - 1);                      //number of location in list
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
        //bool clearPosAdd = false;
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
    /// create random material on area
    /// </summary>
    /// <param name="danElem"></param>
    /// <param name="withCoin"></param>
    /// <returns></returns>
    GameObject GetRandomMaterial(bool danElem, bool withCoin)
    {
        GameObject material = null;
        bool alreadyMat = false;                        //position already has material

        if (!alreadyMat && Random.Range(0, 3) == 0)
        {
            material = Instantiate<GameObject>(emptyObj);
            alreadyMat = true;
        }

        if (danElem && !alreadyMat)
        {
            material = Instantiate<GameObject>(matDanLoc[location - 1][Random.Range(0, matDanLoc[location - 1].Count)]);
            alreadyMat = true;
        }
        else if (!alreadyMat)
        {
            material = Instantiate<GameObject>(matSafLoc[location - 1][Random.Range(0, matSafLoc[location - 1].Count)]);
            alreadyMat = true;
        }

        return material;
    }
    #endregion

    /// <summary>
    /// set data values in window menu when it necessery
    /// </summary>
    /// <param name="animals">inclde an object with cost and id of animals</param>
    static public void setValuesInStart(Animals animals)
    {
        
        //check if the list has animals
        if (DataplayerManager.Instance.Data.BoughtAnimals == null)
        {

            DataplayerManager.Instance.Data.BoughtAnimals = new List<int>();
            DataplayerManager.Instance.Data.BoughtAnimals.Add((int)Animals.animals.grayCat);
            DataplayerManager.Instance.Data.CurrentAnimal = (int)Animals.animals.grayCat;

            LoadSavePlayer.Save(DataplayerManager.Instance.Data);
            DataplayerManager.Instance.Data = LoadSavePlayer.Load();
        }
        
        //set the best score in block in main menu
        GameObject.Find("TextScoreNum").GetComponent<Text>().text = DataplayerManager.Instance.Data.Score.ToString();

        //set music
        if (DataplayerManager.Instance.Data.IsMusicMainMenu)
        {
            GameObject.Find("CanvasClassic").transform.Find("ButtonMusic").gameObject.
            GetComponent<Toggle>().isOn = true;
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            GameObject.Find("CanvasClassic").transform.Find("ButtonMusic").gameObject.
            GetComponent<Toggle>().isOn = false;
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;
        }

        #region set values in shop
        //set count of money in block in shop
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").Find("BlockShowMoney").
            Find("Text").GetComponent<Text>().text = DataplayerManager.Instance.Data.Coins.ToString();

        //find section of animals in canvas classic
        GameObject sectionAnimals = GameObject.Find("CanvasClassic").
            transform.Find("WindowShop").Find("SectionAnimals").gameObject;

        //show && hide blocks with cost and another things with animal's icon in shop
        for(int i = 0; i != animals.AnimalsInShop.Count; i++)
        {
            GameObject anAnimal = sectionAnimals.transform.Find(animals.AnimalsInShop[i].ToString()).gameObject;

            //set cost of animal, set Vi (if bought)
            if (DataplayerManager.Instance.Data.BoughtAnimals.Contains(animals.AnimalsInShop[i]))
            {
                //costumer see vi
                anAnimal.transform.Find("Vi").gameObject.SetActive(true);
                //do that costumer can't do click
                anAnimal.GetComponent<Toggle>().interactable = false;
                //hide blocks with cost
                anAnimal.transform.Find("BlockGreen").gameObject.SetActive(false);
                anAnimal.transform.Find("BlockRed").gameObject.SetActive(false);

            }
            else
            {
                //hide vi (without vi it says that the animal not bought yet
                anAnimal.transform.Find("Vi").gameObject.SetActive(false);
                //costumer can do click
                anAnimal.GetComponent<Toggle>().interactable = true;

                //show cost of animal with green block if costumer has enogh money
                if (DataplayerManager.Instance.Data.Coins >= animals.CostAnimals[i])
                {
                    //show green block
                    anAnimal.transform.Find("BlockGreen").gameObject.SetActive(true);
                    //set cost of animal
                    anAnimal.transform.Find("BlockGreen").Find("Text").GetComponent<Text>().
                        text = animals.CostAnimals[i].ToString();

                    //hide red block
                    anAnimal.transform.Find("BlockRed").gameObject.SetActive(false);
                }
                //show cost of animal with red block
                else
                {
                    //show red block
                    anAnimal.transform.Find("BlockRed").gameObject.SetActive(true);
                    //set cost of animal
                    anAnimal.transform.Find("BlockRed").Find("Text").GetComponent<Text>().
                        text = animals.CostAnimals[i].ToString();

                    //hide green block
                    anAnimal.transform.Find("BlockGreen").gameObject.SetActive(false);
                }
            }
        }


        #endregion

        #region set values in profile
        //find section animals in profile
        GameObject sectionAnimalsProfile = GameObject.Find("CanvasClassic").
            transform.Find("WindowProfile").Find("SectionAnimals").gameObject;

        for (int i = 0; i != animals.AnimalsInShop.Count; i++)
        {
            //find an animal icon to do some monipulations with it
            GameObject anAnimal = sectionAnimalsProfile.transform.Find(animals.AnimalsInShop[i].ToString()).gameObject;

            //do monipulations if the animal bought
            if (DataplayerManager.Instance.Data.BoughtAnimals.Contains(i))
            {
                //show icon of animal in profile
                anAnimal.SetActive(true);

                //set vi and another when the icon is current animal
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
                //hide icon of animal in profile
                anAnimal.SetActive(false);
                //hide vi
                anAnimal.transform.Find("Vi").gameObject.SetActive(false);
            }
        }

        #endregion

        #region set values in quests
        DataQuests dataQuests = LoadSaveQuests.Load();

        if (dataQuests.wasTodayRewardVideo)
        {
            //hide button
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("ButtonRewardVideo").gameObject.SetActive(false);
            //show text that there are not rewarded
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("NoGiftsText").gameObject.SetActive(true);
        }
        else
        {
            //show button
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("ButtonRewardVideo").gameObject.SetActive(true);
            //hide text that there are not rewarded
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("NoGiftsText").gameObject.SetActive(false);
        }
        #endregion
    }


}
