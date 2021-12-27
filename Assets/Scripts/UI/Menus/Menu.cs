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
    public Dictionary<string, GameObject> elements;
    public UIEvents selectors;
    public FadeController fade;

    //Music:
    public AudioSource music;
    public Settings optionsMenu;

    public GameManager gameManager;

    public GameObject loadParent;
    public GameObject logo;
    public GameObject warning;
    public bool isLoadRunning;
    #endregion

    public void Awake() //Used to load resolutions and create list for the dropdown, collects both Width and Height seperately
    {
        GetReferences();
        StartCoroutine("LoadScreen");
    }

    public void GetReferences()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        fade = GameObject.FindWithTag("FadeController").GetComponent<FadeController>();
        elements = new Dictionary<string, GameObject>
        {
            { "Main", GameObject.Find("Elements_Main") },
            { "SaveLoad", GameObject.Find("Elements_SaveSelection") },
        };

        elements["SaveLoad"].SetActive(false);
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

    public void SaveSelection(bool toggle)
    {
        switch (toggle)
        {
            case true:
                elements["SaveLoad"].SetActive(true);
                elements["Main"].SetActive(false);
                break;
            case false:
                elements["Main"].SetActive(true);
                elements["SaveLoad"].SetActive(false);
                break;
        }
    }

    public void CallStart() //Trigger for Play Button
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        music.Stop();
        fade.FadeOut();
        yield return new WaitForSeconds(2);
        fade.FadeIn();
        gameManager.LoadGame();
    }

    public void CallQuit() //Trigger for Exit Button
    {
        StartCoroutine(QuitGame());
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