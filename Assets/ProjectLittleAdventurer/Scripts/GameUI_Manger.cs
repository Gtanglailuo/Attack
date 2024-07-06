using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI_Manger : MonoBehaviour
{
    public Slider slider;
    public GameManger GM;
    public TMPro.TextMeshProUGUI CoinText;

    public GameObject UI_Pause;
    public GameObject UI_GameOver;
    public GameObject UI_GameIsFinished;

    private enum GameUI_State
    { 
    GamePlay,Pause,GameOver,GameIsFinished
    }
    GameUI_State currentState;
    private void Awake()
    {
        SwitchUIState(GameUI_State.GamePlay);
    }

    private void Update()
    {
        

        CoinText.text = GM.playerCharacter.Coin.ToString();
        slider.value = GM.playerCharacter.GetComponent<Health>().CurrentHealthPercentage;
       
    }
    private void SwitchUIState(GameUI_State state)
    {
        UI_Pause.SetActive(false);
        UI_GameOver.SetActive(false);
        UI_GameIsFinished.SetActive(false);


        Time.timeScale = 1.0f;

        switch (state)
        {
            case GameUI_State.GamePlay:
                Time.timeScale = 1.0f;
                break;
            case GameUI_State.Pause:
                UI_Pause.SetActive(true);
                Time.timeScale = 0.0f;
                break;
            case GameUI_State.GameOver:
                UI_GameOver.SetActive(true);
                break;
            case GameUI_State.GameIsFinished:
                UI_GameIsFinished.SetActive(true);
                break;
            default:
                break;
        }

        currentState = state;
    }


    public void TogglePauseUI()
    {
        if (currentState==GameUI_State.GamePlay)
        {
            SwitchUIState(GameUI_State.Pause);
        }
        else if (currentState == GameUI_State.Pause)
        {
            SwitchUIState(GameUI_State.GamePlay);
        }
    }

    public void Button_MainMenu()
    {
        GM.ReturnToTheMainMenu();
    }

    public void Buttton_Restart()
    {
        GM.Restart();

    }

    public void ShowGameOver()
    {

        SwitchUIState(GameUI_State.GameOver);
    }
    public void ShowGameIsFinished()
    {
        SwitchUIState(GameUI_State.GameIsFinished);
    }



}
