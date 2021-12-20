using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public GameObject mainMenu, options, general, video, audioRef, controls;
    public AudioMixer masterMixer;
    public UIEvents selectors;

    Resolution[] resolutions; //Creates reference for all resolutions within Unity
    public Dropdown resolutionDropdown; //Creates reference for the resolution dropdown 

    //public Text up, down, left, right, jump, attack, dash;
    public TextMeshProUGUI moveLeft, moveRight, moveUp, moveDown, shootLeft, shootRight, shootUp, shootDown, attack, heal, trueSight, interact, switchWeapon, pause;
    private GameObject currentKey;

    public LastMenuState lastMenuState;
    public GameObject pauseMenu;

    public void Start()
    { 

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) //Load possible resolutions into list
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //Makes sure the resolution is correctly applied
            {
                currentResolutionIndex = i;
            }
        }

        SetKeybinds();

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetKeybinds()
    {
        moveLeft.text = CheckSpecial(GameManager.keybind["MoveLeft"]);
        moveRight.text = CheckSpecial(GameManager.keybind["MoveRight"]);
        moveUp.text = CheckSpecial(GameManager.keybind["MoveUp"]);
        moveDown.text = CheckSpecial(GameManager.keybind["MoveDown"]);
        shootLeft.text = CheckSpecial(GameManager.keybind["ShootLeft"]);
        shootRight.text = CheckSpecial(GameManager.keybind["ShootRight"]);
        shootUp.text = CheckSpecial(GameManager.keybind["ShootUp"]);
        shootDown.text = CheckSpecial(GameManager.keybind["ShootDown"]);
        attack.text = CheckSpecial(GameManager.keybind["Melee"]);
        heal.text = CheckSpecial(GameManager.keybind["Heal"]);
        trueSight.text = CheckSpecial(GameManager.keybind["TrueSight"]);
        interact.text = CheckSpecial(GameManager.keybind["Interact"]);
        switchWeapon.text = CheckSpecial(GameManager.keybind["SwitchWeapon"]);
        pause.text = CheckSpecial(GameManager.keybind["Pause"]);
    }

    public static string CheckSpecial(KeyCode keybind)
    {
        if (keybind == KeyCode.LeftArrow)
        {
            return "←";
        }
        else if (keybind == KeyCode.RightArrow)
        {
            return "→";
        }
        else if (keybind == KeyCode.UpArrow)
        {
            return "↑";
        }
        else if (keybind == KeyCode.DownArrow)
        {
            return "↓";
        }
        else if (keybind == KeyCode.LeftShift)
        {
            return "LS";
        }
        else if (keybind == KeyCode.RightShift)
        {
            return "RS";
        }
        else if (keybind == KeyCode.Escape)
        {
            return "Esc";
        }
        else
        {
            return keybind.ToString();
        }

    }

    public void OptionsCall(bool toggle)
    {
        ToggleOptions(toggle, lastMenuState);
    }

    public void ToggleOptions(bool toggle, LastMenuState lastState) //Trigger for Settings - sets active layer/pannel
    {
        lastMenuState = lastState;

        if (toggle == true)
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            options.SetActive(true);
        }
        else
        {
            switch (lastMenuState)
            {
                case LastMenuState.MainMenu:
                    mainMenu.SetActive(true);
                    break;
                case LastMenuState.PauseMenu:
                    pauseMenu.SetActive(true); 
                    break;
            }
            options.SetActive(false);
        }

        selectors.Visibility(false);
    }

    public void ChangeBetween(int option) //Trigger for Settings - sets active layer/pannel
    {
        switch (option)
        {
            case 0:
                general.SetActive(true);
                video.SetActive(false);
                audioRef.SetActive(false);
                controls.SetActive(false);

                SystemConfig.SaveSettings();
                break;
            case 1:
                general.SetActive(false);
                video.SetActive(true);
                audioRef.SetActive(false);
                controls.SetActive(false);
                break;
            case 2:
                general.SetActive(false);
                video.SetActive(false);
                audioRef.SetActive(true);
                controls.SetActive(false);
                break;
            case 3:
                general.SetActive(false);
                video.SetActive(false);
                audioRef.SetActive(false);
                controls.SetActive(true);
                break;
        }

        selectors.Visibility(false);
    }

    public void MasterVolume(float volume) //Trigger for changing volume of game's master channel
    {
        GameManager.masterMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void EffectsVolume(float volume) //Trigger for changing volume of game's sfx channel
    {
        GameManager.masterMixer.SetFloat("Effects", Mathf.Log10(volume) * 20);
    }

    public void MusicVolume(float volume) //Trigger for changing volume of game's music channel
    {
        GameManager.masterMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void AmbienceVolume(float volume) //Trigger for changing volume of game's music channel
    {
        GameManager.masterMixer.SetFloat("Ambience", Mathf.Log10(volume) * 20);
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
                GameManager.keybind[currentKey.name] = KeyCode.LeftShift;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CheckSpecial(keypress.keyCode);
                currentKey = null; 
            }
            else if (keypress.isKey) //Checks whether or not the event "keypress" contains a keycode
            {
                GameManager.keybind[currentKey.name] = keypress.keyCode; //Saves the keycode from the event as the keycode attached to the keybind dictionary
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CheckSpecial(keypress.keyCode); //Changes the text to match that of the keycode replacing the previous one
                currentKey = null; //resets the currentKey putting it back to null
            }            
        }
    }

    public void changeControls(GameObject clicked) //Trigger for changing any one of the keybinds
    {
        currentKey = clicked;
    }
}

public enum LastMenuState
{
    MainMenu,
    PauseMenu
}