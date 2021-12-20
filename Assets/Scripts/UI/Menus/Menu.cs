using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    #region Variables
    //General: 
    public GameObject main, mainBackground; //Allows for reference to GameObjects Meny and Options
    public AudioMixer masterMixer;                   //public bool toggle = false; //Toggle for switching between settings and main
                                                     //public int option = 0;      //Changes between the 4 main screens in options.
    public UIEvents selectors;
    public FadeController fade;

    public bool quitTimer = false; //Check whether or not the exit button has been pressed
    public int qTimer = 0; //Timer for transition - exit
    public bool startTimer = false; //Checks whether or not the play button has been pressed
    public int sTimer = 0; //Timer for transition - load game

    //Music:
    public AudioSource music;
    public Settings optionsMenu;

    public GameManager gameManager;

    public GameObject loadParent;
    public GameObject logo;
    public GameObject warning;
    public bool isLoadRunning;
    #endregion

    public void Start() //Used to load resolutions and create list for the dropdown, collects both Width and Height seperately
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine("LoadScreen");
    }

    public void Update()
    {
        switch (isLoadRunning)
        {
            case true:
                if (Input.GetKeyDown(GameManager.keybind["Pause"]))
                {
                    StopCoroutine("LoadScreen");
                    logo.SetActive(false);
                    warning.SetActive(false);
                    loadParent.SetActive(false);
                    fade.FadeOut();
                    fade.FadeIn();
                    music.Play();
                    isLoadRunning = false;
                }
                break;
        }
    }

    public void StartGame() //Trigger for Play Button
    {
        StartCoroutine("StartCall");
    }

    IEnumerator StartCall()
    {
        music.Stop();
        fade.FadeOut();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(2);
        main.SetActive(false);
        mainBackground.SetActive(false);
        selectors.Visibility(false);

        fade.FadeIn();
        gameManager.StartGame();
    }

    public void Quit() //Trigger for Exit Button
    {
        StartCoroutine("QuitGame");
    }

    IEnumerator QuitGame()
    {
        fade.FadeOut();
        yield return new WaitForSeconds(2);
        Application.Quit();
    }

    public void OptionsCall(bool toggle)
    {
        optionsMenu.ToggleOptions(toggle, LastMenuState.MainMenu);
    }

    IEnumerator LoadScreen() //Called upon to show that the player has died; Makes the player un-hittable and dead.
    {
        isLoadRunning = true;
        fade.FadeOut();
        yield return new WaitForSeconds(1);
        logo.SetActive(true);
        fade.FadeIn();
        yield return new WaitForSeconds(3.5f);
        fade.FadeOut();
        yield return new WaitForSeconds(1);
        logo.SetActive(false);
        warning.SetActive(true);
        fade.FadeIn();
        yield return new WaitForSeconds(5);
        fade.FadeOut();
        yield return new WaitForSeconds(1);
        warning.SetActive(false);
        loadParent.SetActive(false);
        fade.FadeIn();
        music.Play();
        isLoadRunning = false;
    }
}