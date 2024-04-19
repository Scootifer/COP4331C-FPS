using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;
    public GameObject pauseMenuUI;
    public TMP_Text ScoreText;
    public TMP_Text PauseVictoryLossText;

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                PauseGame();
            }
        }
    }

    public void SetScoreText(int score)
    {
        ScoreText.SetText($"Score: {score}");
    }

    public void SetPauseText(bool didPlayerWin)
    {
        if (didPlayerWin) PauseVictoryLossText.SetText("Victory!");
        else PauseVictoryLossText.SetText("Defeat!");
    }

    //make way to pause game timer
    public void ResumeGame()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        GamePaused = false;
    }

    public void PauseGame()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        GamePaused = true;
    }
    // 
    public void ExitGame()
    {
        //confirm exit game, closes application
        Application.Quit();
    }

    public void MainMenuConfirm()
    {
        // return to main menu
        SceneManager.LoadScene(0);
    }
}
