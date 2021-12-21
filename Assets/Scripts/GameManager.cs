using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Manager")]
    public static GameManager instance;
    public GameObject loadingScreen;

    [Header("Settings")]
    public static AudioMixer masterMixer; //Creates reference for the menu musi
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode> //Dictionary to store the keybinds.
    {
        { "MoveUp", KeyCode.W },
        { "MoveDown", KeyCode.S },
        { "MoveLeft", KeyCode.A },
        { "MoveRight", KeyCode.D },
        { "ShootUp", KeyCode.UpArrow },
        { "ShootDown", KeyCode.DownArrow },
        { "ShootLeft", KeyCode.LeftArrow },
        { "ShootRight", KeyCode.RightArrow },
        { "Melee", KeyCode.E },
        { "Heal", KeyCode.R },
        { "Parry", KeyCode.LeftShift },
        { "TrueSight", KeyCode.X },
        { "Interact", KeyCode.F },
        { "SwitchWeapon", KeyCode.Tab },
        { "Inventory", KeyCode.I },
        { "Pause", KeyCode.Escape }
    };

    public void Awake()
    {
        StartGame();
        LoadSettings();
    }

    public void StartGame()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndex.Menu_Main, LoadSceneMode.Additive);
    }

    public void LoadSettings()
    {
        masterMixer = Resources.Load("Music/Mixers/Master") as AudioMixer; //Loads the MasterMixer for renference.

        if (File.Exists(Application.persistentDataPath + "/settings.json")) //Checks if the file already exists and loads the file if it does.
        {
            SystemConfig.LoadSettings();
        }
        else //Else, creates the data for the new file.
        {
            SystemConfig.SaveSettings(); //Saves the new data as a new file "Settings".
        }
    }

    public void LoadGame()
    {
        loadingScreen.SetActive(true);

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndex.Menu_Main));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.World));

        StartCoroutine(SceneLoadProgress(scenesLoading));
    }

    public IEnumerator SceneLoadProgress(List<AsyncOperation> scenesLoading)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }
}