using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instruction;
    public GameObject credit;
    
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        instruction.SetActive(false);
        credit.SetActive(false);
        AudioManager.instance.Play(SoundType.MUSIC,"MenuMusic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SpotLight");
    }
    
    public void ShowMenu()
    {
        mainMenu.SetActive(true);
        instruction.SetActive(false);
        credit.SetActive(false);
    }

    public void ShowInstruction()
    {
        mainMenu.SetActive(false);
        instruction.SetActive(true);
        credit.SetActive(false);
    }
    
    public void ShowCredit()
    {
        mainMenu.SetActive(false);
        instruction.SetActive(false);
        credit.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
