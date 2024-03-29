﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("General")]
    public bool optionsActive;
    public GameObject options, background;
    public List<GameObject> sectionContent;

    [Header("Selection")]
    public GameObject settingsFirst;

    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown; //Creates reference for the resolution dropdown 
    public Vector2 minimumResolution;
    Resolution[] resolutions; //Creates reference for all resolutions within Unity

    [Header("Controls")]
    public TextMeshProUGUI moveLeft, moveRight, lookUp, lookDown, jump, dash, attack, heal, interact, pause;
    private GameObject currentKey;

    [Header("Audio")]
    public List<TextMeshProUGUI> audioVisuals = new List<TextMeshProUGUI>();
    public AudioMixer masterMixer;

    public int AudioRound(float volume)
    {
        return Mathf.RoundToInt(volume * 100);
    }

    public void Awake()
    {
        GameManager.instance.optionsMenu = this;

        resolutions = Screen.resolutions;
        Debug.Log(resolutions.Length);
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> res = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) //Load possible resolutions into list
        {
            if (resolutions[i].width >= minimumResolution.x && resolutions[i].height >= minimumResolution.y)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                res.Add(option);
            }

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //Makes sure the resolution is correctly applied
            {
                currentResolutionIndex = i;
            }
        }

        SetKeybinds();

        resolutionDropdown.AddOptions(res);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        options.SetActive(false);
        background.SetActive(false);
    }

    public void Update()
    {
        switch (optionsActive)
        {
            case true:
                if (Input.GetKeyDown(InputManager.instance.keybind["Pause"]))
                {
                    ToggleOptions(false);
                }
                break;
        }
    }

    public void SetKeybinds()
    {
        moveLeft.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["MoveLeft"]);
        moveRight.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["MoveRight"]);
        lookUp.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["LookUp"]);
        lookDown.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["LookDown"]);
        jump.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["Jump"]);
        dash.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["Dash"]);
        attack.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["Attack"]);
        heal.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["Heal"]);
        interact.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["Interact"]);
        pause.text = KeycodeAlias.CheckSpecial(InputManager.instance.keybind["Pause"]);
    }

    public void OptionsCall(bool toggle)
    {
        ToggleOptions(toggle);
    }

    public void ToggleOptions(bool toggle) //Trigger for Settings - sets active layer/pannel
    {
        options.SetActive(toggle);
        background.SetActive(toggle);

        switch (toggle)
        {
            case false:
                DataManager.instance.SaveSettings();
                break;
            case true:
                EventSystem.current.SetSelectedGameObject(settingsFirst);
                break;
        }

        optionsActive = toggle;
        //selectors.Visibility(false);
    }

    public void ChangeBetween(Transform button) //Trigger for Settings - sets active layer/pannel
    {
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < sectionContent.Count; i++)
        {
            if (i == index)
            {
                sectionContent[i].SetActive(true);
            }
            else
            {
                sectionContent[i].SetActive(false);
            }
        }

        //selectors.Visibility(false);
    }

    public void MasterVolume(float volume) //Trigger for changing volume of game's master channel
    {
        GameManager.instance.masterMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        UpdateAudioVisual(AudioRound(volume), 0);
    }

    public void MusicVolume(float volume) //Trigger for changing volume of game's music channel
    {
        GameManager.instance.masterMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        UpdateAudioVisual(AudioRound(volume), 1);
    }

    public void EffectsVolume(float volume) //Trigger for changing volume of game's sfx channel
    {
        GameManager.instance.masterMixer.SetFloat("Effects", Mathf.Log10(volume) * 20);
        UpdateAudioVisual(AudioRound(volume), 2);
    }

    public void AmbienceVolume(float volume) //Trigger for changing volume of game's music channel
    {
        GameManager.instance.masterMixer.SetFloat("Ambience", Mathf.Log10(volume) * 20);
        UpdateAudioVisual(AudioRound(volume), 3);
    }

    public void UpdateAudioVisual(float volume, int index)
    {
        audioVisuals[index].text = volume.ToString();
    }

    public void ToggleFullscreen(int option) //Trigger for applying fullscreen
    {
        switch (option)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void ChangeResolution(int resIndex) //Trigger for changing and applying resolution based on list
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void OnGUI()
    {
        if (currentKey != null) //Checks whether or not there is a Keycode saved to 'currentKey'
        {
            Event keypress = Event.current; //Creates an event called keypress

            if (keypress.shift)
            {
                InputManager.instance.keybind[currentKey.name] = KeyCode.LeftShift;

                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeycodeAlias.CheckSpecial(keypress.keyCode); //Changes the text to match that of the keycode replacing the previous one
                currentKey = null; //resets the currentKey putting it back to null
            }
            else if (keypress.isKey) //Checks whether or not the event "keypress" contains a keycode
            {
                InputManager.instance.keybind[currentKey.name] = keypress.keyCode; //Saves the keycode from the event as the keycode attached to the keybind dictionary

                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeycodeAlias.CheckSpecial(keypress.keyCode); //Changes the text to match that of the keycode replacing the previous one
                currentKey = null; //resets the currentKey putting it back to null
            }
        }
    }

    public void changeControls(GameObject clicked) //Trigger for changing any one of the keybinds
    {
        currentKey = clicked;
    }
}