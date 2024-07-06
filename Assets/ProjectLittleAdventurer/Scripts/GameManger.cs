using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManger : MonoBehaviour
{
    public Character playerCharacter;

    private bool gameIsOver;

    public GameUI_Manger UI_Manger;
    private void Awake()
    {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void GameOver()
    {
        UI_Manger.ShowGameOver();
    }

    public void GameIsOver()
    {
        UI_Manger.ShowGameIsFinished();
    }
    private void Update()
    {
        if (gameIsOver)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            UI_Manger.TogglePauseUI();
        }
        if (playerCharacter.CurrentState==Character.CharacterState.Dead)
        {
            gameIsOver = true;
            GameOver();
        }
    }

    public void ReturnToTheMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
    }

}
