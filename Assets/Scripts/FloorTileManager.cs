using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FloorTileManager : MonoBehaviour
{
    private static FloorTileManager instance;

    public static FloorTileManager Instance
    {
        get => instance;
    }
    
    
    
    [SerializeField] 
    private GameObject floorTilePre;
    public int roundNum;
    public float roundInterval;
    public float roundTimeRatio;
    public float fallTileRatio;
    
    private float roundTimeCounter;
    private int fallTileNum;
    private float colorChangeInterval;
    private bool isGameOver = false;
    
    private GameObject [,] tilesArr;
    private List<GameObject> tileList;
    private List<int> randomRowIndexes;
    private List<int> randomColIndexes;
    private List<Bonus> bonusList;

    private bool canCountDown;
    private bool canChangeColor;
    
    [Header("GameSettings")] 
    public int tileRowNumber;
    public int tileColumnNumber;
    public float timeLimit;
    public int scoreGoal;
    public int bounsNumber;

    private float timeCounter;
    private int bonusGet;
    public GameObject bonusPrefab;
    
    [Header("UI")]
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public GameObject result;
    public GameObject pauseMenu;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        tilesArr = new GameObject[tileRowNumber, tileColumnNumber];
        tileList = new List<GameObject>();
        bonusList = new List<Bonus>();
        
        InitMap();
        
        randomRowIndexes = new List<int>();
        randomColIndexes = new List<int>();
        
        canCountDown = true;
        canChangeColor = true;
        
        fallTileNum = Mathf.RoundToInt(tilesArr.Length * fallTileRatio);
        colorChangeInterval = roundInterval * roundTimeRatio;

        timeCounter = timeLimit;
        timeText.SetText(Mathf.Round(timeCounter).ToString());
        scoreText.SetText(bonusGet.ToString()+"/"+scoreGoal.ToString());
        
        AudioManager.instance.Play(SoundType.MUSIC,"GameMusic");
    }
    
    void Update()
    {
        if(isGameOver)
            return;

        if (canCountDown && roundNum > 0)
        {
            fallTimeRoundLoop();
        }
            

        timeCounter -= Time.deltaTime;
        timeText.SetText(Mathf.Round(timeCounter).ToString());

        if (bonusGet == scoreGoal)
        {
            EndGame(true);
        }

        if (timeCounter <= 0)
        {
            EndGame(false);
        }
    }

    private void InitMap()
    {
        for (int i = 0; i < tileRowNumber; i++)
        {
            for (int j = 0; j < tileColumnNumber; j++)
            {
                Vector3 tilePos = new Vector3(i, 0f, j);
                //Debug.Log($"should {tilePos}");
                GameObject tileGO = Instantiate(floorTilePre, transform);
                tilesArr[i, j] = tileGO;
                tileList.Add(tileGO);
                
                tileGO.transform.position = tilePos;
                //Debug.Log($"real {tileGO.transform.position}");
                tileGO.gameObject.GetComponent<Rigidbody>().useGravity = false;
                tileGO.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    private void fallTimeRoundLoop()
    {
        roundTimeCounter += Time.deltaTime;
        
        if (roundTimeCounter >= colorChangeInterval && canChangeColor)
        {
            canChangeColor = false;
            
            GenerateRandomTileCoordinate();
            TileChangeColor();
            CreateBonus();
        }

        if (roundTimeCounter >= roundInterval)
        {
            canCountDown = false;
            
            TileFall();
            RoundEndHandle();
        }
    }

    private void GenerateRandomTileCoordinate()
    {
        int tempFallTileNum = fallTileNum;
        
        while (tempFallTileNum > 0)
        {
            int randomRowIndex = UnityEngine.Random.Range(0, tileColumnNumber);
            int randomColIndex = UnityEngine.Random.Range(0, tileRowNumber);

            if (tilesArr[randomRowIndex, randomColIndex] != null)
            {
                randomRowIndexes.Add(randomRowIndex);
                randomColIndexes.Add(randomColIndex);
                tempFallTileNum--;
            }
        }
    }

    private void TileChangeColor()
    {
        Debug.Log("Changing color");
        
        for (int i = 0; i < fallTileNum; i++)
        {
            tilesArr[randomRowIndexes[i],randomColIndexes[i]].gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void TileFall()
    {
        for (int i = 0; i < fallTileNum; i++)
        {
            tilesArr[randomRowIndexes[i],randomColIndexes[i]].gameObject.GetComponent<Rigidbody>().useGravity = true;
            tilesArr[randomRowIndexes[i],randomColIndexes[i]].gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void RoundEndHandle()
    {
        for (int i = 0; i < fallTileNum; i++)
        {
            GameObject tempTile = tilesArr[randomRowIndexes[i], randomColIndexes[i]].gameObject;
            tileList.Remove(tempTile);
            Destroy(tempTile);
        }

        foreach (var bonus in bonusList)
        {
            Destroy(bonus.gameObject);
        }
        bonusList.Clear();

        canCountDown = true;
        canChangeColor = true;
        
        roundNum--;
        roundTimeCounter = 0;
        
        randomRowIndexes.Clear();
        randomColIndexes.Clear();
    }

    private void CreateBonus()
    {
        List<int> tempIndex = new List<int>();
        for (int i = 0; i < bounsNumber; i++)
        {
            int x = Random.Range(0, tileList.Count - 1);
            foreach (int index in tempIndex)
            {
                while (x == index)
                {
                    x = Random.Range(0, tileList.Count - 1);
                }
            }

            tempIndex.Add(x);
            GameObject tempBonus = Instantiate(bonusPrefab,
                tileList[x].transform.position + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.identity);
            //Debug.Log("Bonus!");
            bonusList.Add(tempBonus.GetComponent<Bonus>());
        }
    }

    public void RemoveBonus(Bonus bonus)
    {
        bonusList.Remove(bonus);
    }

    public void Score()
    {
        bonusGet++;
        scoreText.SetText(bonusGet.ToString()+"/"+scoreGoal.ToString());
        AudioManager.instance.Play(SoundType.SFX,"Ding");
    }

    public void EndGame(bool isWin)
    {
        isGameOver = true;
        result.SetActive(true);
        result.GetComponent<ResultPage>().SetResult(isWin);
        Time.timeScale = 0.0f;
    }
    
    public void Pause()
    {
        Debug.Log("Pause");
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }
}
