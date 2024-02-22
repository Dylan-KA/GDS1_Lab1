using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button ResumeButton;
    [SerializeField] Button SwitchLevelButton;
    [SerializeField] Button MainMenuButton;
    [SerializeField] GameObject PausePanel;
    [SerializeField] AudioSource BGMusic;

    private bool bIsGamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!bIsGamePaused)
        {
            PauseGame();
        } else
        {
            ResumeGame();
        }
    }

    public void SwitchGameMode()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            SceneManager.LoadSceneAsync(2);
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    public bool IsGamePaused()
    {
        return bIsGamePaused;
    }

    private void ShowPauseMenu()
    {
        PausePanel.SetActive(true);
        ResumeButton.enabled = true;
        SwitchLevelButton.enabled = true;
        MainMenuButton.enabled = true;
    }

    private void HidePauseMenu()
    {
        PausePanel.SetActive(false);
        ResumeButton.enabled = false;
        SwitchLevelButton.enabled = false;
        MainMenuButton.enabled = false;
    }

    public void PauseGame()
    {
        bIsGamePaused = true;
        BGMusic.Pause();
        ShowPauseMenu();
        //Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        bIsGamePaused = false;
        BGMusic.UnPause();
        HidePauseMenu();
        //Debug.Log("Game Resumed");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
