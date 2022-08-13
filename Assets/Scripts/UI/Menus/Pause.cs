using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    #region Variables
    [Header("General")]
    public PauseState pauseState = PauseState.Playing; //Checks whether or not the game is paused
    public GameObject pauseMenu, background;
    public FadeController fade;

    [Header("Selector")]
    public GameObject pauseFirst;
    public UIEvents selectors;
    #endregion

    #region General
    public void Awake()
    {
        fade = GameObject.FindWithTag("FadeController").GetComponent<FadeController>();
        selectors = GameObject.FindWithTag("Selectors").GetComponent<UIEvents>();
        pauseMenu.SetActive(false);
        background.SetActive(false);
        selectors.Visibility(false);
    }

    public void Update() //Ensures the pause menu can function
    {
        if (Input.GetKeyDown(InputManager.instance.keybind["Pause"])) //Show pause menu
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
        pauseMenu.SetActive(pause);
        background.SetActive(pause);

        switch (pause)
        {
            case true:
                pauseState = PauseState.Pause;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(pauseFirst);
                break;
            case false:
                pauseState = PauseState.Playing;
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }

        Cursor.visible = pause;
        selectors.Visibility(pause);
    }

    public void CallMenu() //Trigger for menu button
    {
        StartCoroutine("ChangeToMain");
    }

    IEnumerator ChangeToMain()
    {
        Time.timeScale = 1f;
        yield return fade.FadeOut();
        GameManager.instance.QuitGame();
    }

    public void OptionsCall(bool toggle)
    {
        GameManager.instance.optionsMenu.ToggleOptions(toggle);
    }
    #endregion
}

public enum PauseState
{
    Playing,
    Pause
}