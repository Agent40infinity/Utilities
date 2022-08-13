using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    #region Variables
    [Header("General")]          
    public Dictionary<string, GameObject> elements;
    public FadeController fade;
    public bool saveFileSelection;

    [Header("Audio")]
    public AudioSource music;

    [Header("Selection")]
    public GameObject mainFirst;
    public GameObject saveLoadFirst;

    [Header("Intro")]
    public GameObject loadParent;
    public GameObject logo;
    public GameObject warning;
    public bool isLoadRunning;
    #endregion

    public void Awake() //Used to load resolutions and create list for the dropdown, collects both Width and Height seperately
    {
        GetReferences();

        switch (GameManager.instance.introPlayed)
        {
            case true:
                StartCoroutine(SkipIntro(GameManager.instance.introPlayed));
                break;
            case false:
                StartCoroutine("IntroScreen");
                break;
        }
    }

    public void GetReferences()
    {
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
                if (Input.GetKeyDown(InputManager.instance.keybind["Pause"]))
                {
                    StartCoroutine(SkipIntro(GameManager.instance.introPlayed));
                }
                break;
        }

        switch (saveFileSelection)
        {
            case true:
                if (Input.GetKeyDown(InputManager.instance.keybind["Pause"]))
                {
                    SaveSelection(false);
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
                EventSystem.current.SetSelectedGameObject(saveLoadFirst);
                break;
            case false:
                elements["Main"].SetActive(true);
                elements["SaveLoad"].SetActive(false);
                EventSystem.current.SetSelectedGameObject(mainFirst);
                break;
        }

        saveFileSelection = toggle;
    }

    public void CallStart() //Trigger for Play Button
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        music.Stop();
        yield return fade.FadeOut();
        GameManager.instance.LoadGame();
    }

    public void CallQuit() //Trigger for Exit Button
    {
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        yield return fade.FadeOut();
        Application.Quit();
    }

    public void OptionsCall(bool toggle)
    {
        GameManager.instance.optionsMenu.ToggleOptions(toggle);
    }

    IEnumerator IntroScreen() //Called upon to show that the player has died; Makes the player un-hittable and dead.
    {
        isLoadRunning = true;
        yield return fade.FadeOut();
        logo.SetActive(true);
        fade.FadeIn();
        yield return new WaitForSeconds(3.5f);
        fade.FadeOut();
        yield return fade.FadeOut();
        logo.SetActive(false);
        warning.SetActive(true);
        fade.FadeIn();
        yield return new WaitForSeconds(5);
        fade.FadeOut();
        yield return fade.FadeOut();
        warning.SetActive(false);
        loadParent.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainFirst);
        fade.FadeIn();
        music.Play();
        GameManager.instance.introPlayed = true;
        isLoadRunning = false;
    }

    IEnumerator SkipIntro(bool played)
    {
        switch (played)
        {
            case false:
                StopCoroutine("IntroScreen");
                isLoadRunning = false;
                GameManager.instance.introPlayed = true;

                yield return fade.FadeOut();
                fade.FadeIn();
                break;
        }

        logo.SetActive(false);
        warning.SetActive(false);
        loadParent.SetActive(false);
        music.Play();
    }
}