using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPage : MonoBehaviour
{
    public TMP_Text resultText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetResult(bool isWin)
    {
        if (isWin)
        {
            resultText.SetText("WIN");
        }
        else
        {
            resultText.SetText("LOSE");
        }
    }
}
