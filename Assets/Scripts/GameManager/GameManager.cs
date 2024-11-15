using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        //GameEventsManager.Instance.UIEvents.OnTogglePauseMenuEvent += PauseUnpauseGame;
        GameEventsManager.Instance.UIEvents.OnQuitButtonPressedEvent += Quit;
    }
    
    private void OnDisable()
    {
        //GameEventsManager.Instance.UIEvents.OnTogglePauseMenuEvent -= PauseUnpauseGame;
        GameEventsManager.Instance.UIEvents.OnQuitButtonPressedEvent -= Quit;
    }

    private void PauseUnpauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void Quit()
    {
        Application.Quit();
    }


}