using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text remainingTimeText;
    public TMP_Text goalTimeText;
    
    [Header("GameSettings")] 
    
    public float timeLimit = 30.0f;
    public float timeGoal = 10.0f;
    private float timeCounter = 0.0f;

    public bool isStartCount;
    private bool isGameOver;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit -= Time.deltaTime;
        remainingTimeText.text = timeLimit.ToString("0");

        if (isStartCount)
        {
            timeCounter += Time.deltaTime;
        }
        
        goalTimeText.text = timeCounter.ToString("0");

        if (timeCounter >= timeGoal)
        {
            timeCounter = 10;
            goalTimeText.text = timeCounter.ToString("0");
            SceneManager.LoadScene("Win");
        }

        if (timeLimit <= 0)
        {
            timeLimit = 0;
            remainingTimeText.text = timeLimit.ToString("0");
            SceneManager.LoadScene("Lose");
        }
    }
}
