using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    #region Variables
    public PauseState pauseState = PauseState.Playing; //Checks whether or not the game is paused
    public OptionState optionState = OptionState.Option;
    public GameObject pauseMenu;
    public GameObject options, main, mainBackground, background; //Creates reference for the pause menu
    public Settings optionsMenu;
    public Menu menu;
    public FadeController fade;
    public GameObject gameEvent;
    public UIEvents selectors;

    public GameManager gameManager;
    #endregion

    #region General
    public void Update() //Ensures the pause menu can function
    {
        if (Input.GetKeyDown(GameManager.keybind["Pause"])) //Show pause menu
        {
            switch (pauseState)
            {
                case PauseState.Playing:
                    PauseG();
                    break;
                case PauseState.Pause:
                    ResumeG();
                    break;
            }
        }
    }
    #endregion

    #region Pause
    public void ResumeG() //Trigger for resuming game and resume button
    {
        UpdatePause(false);
    }

    public void PauseG() //Trigger for pausing game
    {
        UpdatePause(true);
    }

    public void UpdatePause(bool pause)
    {
        switch (optionState)
        {
            case OptionState.Option:
                OptionsCall(false);
                break;
        }

        CheckEvent(pause);
        pauseMenu.SetActive(pause);
        background.SetActive(pause);

        switch (pause)
        {
            case true:
                pauseState = PauseState.Pause;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case false:
                pauseState = PauseState.Playing;
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                selectors.Visibility(false);
                break;
        }
    }

    public void CheckEvent(bool pause)
    {
        switch (pause)
        {
            case true:
                if (GameObject.FindWithTag("GameEvent"))
                {
                    gameEvent = GameObject.FindWithTag("GameEvent");
                    gameEvent.SetActive(false);
                }
                break;
            case false:
                if (gameEvent != null)
                {
                    gameEvent.SetActive(true);
                    gameEvent = null;
                }
                break;
        }

    }

    public void Menu() //Trigger for menu button
    {
        StartCoroutine("ChangeToMain");
    }

    IEnumerator ChangeToMain()
    {
        Time.timeScale = 1f;
        fade.FadeOut();
        yield return new WaitForSeconds(2);
        pauseState = PauseState.Playing;
        pauseMenu.SetActive(false);
        main.SetActive(true);
        mainBackground.SetActive(true);
        background.SetActive(false);
        selectors.Visibility(false);

        fade.FadeIn();
        menu.music.Play();
        gameManager.LeaveGame();
    }

    public void OptionsCall(bool toggle)
    {
        switch (toggle)
        {
            case true:
                optionState = OptionState.Option;
                break;
            case false:
                optionState = OptionState.Pause;
                optionsMenu.ChangeBetween(0);
                break;
        }

        optionsMenu.ToggleOptions(toggle, LastMenuState.PauseMenu);
    }
    #endregion
}

public enum PauseState
{
    Playing,
    Pause
}

public enum OptionState
{ 
    Pause,
    Option
}