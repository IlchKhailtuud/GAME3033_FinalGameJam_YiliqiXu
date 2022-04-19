using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager instance;
    
    [Header("GameSettings")] 
    public float timeLimit;
    public int scoreGoal;
    public int tileRowNumber;
    public int tileColumnNumber;
    public int bounsNumber;

    private float timeCounter;
    public GameObject bonusPrefab;

    public GameManager Instance()
    {
        if (instance == null)
            instance = this;
        
        return instance;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBonus()
    {
        
    }
}
