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
    public LoadingScreen loadingScreen;
    public static string loadedSaveFile;
    public static Animator saveIcon;
    public static Player player;
    public static bool introPlayed;
    public static Settings optionsMenu;
    public FadeController fade;

    [Header("Settings")]
    public static AudioMixer masterMixer; //Creates reference for the menu musi
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode> //Dictionary to store the keybinds.
    {
        { "LookUp", KeyCode.W },
        { "LookDown", KeyCode.S },
        { "MoveLeft", KeyCode.A },
        { "MoveRight", KeyCode.D },
        { "Jump", KeyCode.Space },
        { "Dash", KeyCode.LeftShift },
        { "Attack", KeyCode.E },
        { "Heal", KeyCode.R },
        { "Interact", KeyCode.F },
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
        saveIcon = GameObject.FindWithTag("SaveIcon").GetComponent<Animator>();

        SceneManager.LoadSceneAsync((int)SceneIndex.Menu_Options, LoadSceneMode.Additive);
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
        loadingScreen.Visibility(true);

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndex.Menu_Main));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.Player, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.World, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.Menu_Pause, LoadSceneMode.Additive));

        StartCoroutine(SceneLoadProgress(scenesLoading));
        StartCoroutine(SaveDataProgress());
    }

    public void QuitGame()
    {
        loadingScreen.Visibility(true);

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            int buildIndex = SceneManager.GetSceneAt(i).buildIndex;
            Debug.Log(SceneManager.GetSceneAt(i).name);

            switch (buildIndex)
            {
                case (int)SceneIndex.Persistent: case (int)SceneIndex.Menu_Options:
                    break;
                default:
                    scenesLoading.Add(SceneManager.UnloadSceneAsync(buildIndex));
                    break;
            }
        }
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.Menu_Main, LoadSceneMode.Additive));
        StartCoroutine(SceneLoadProgress(scenesLoading));
        StartCoroutine(SaveQuitProgress());
    }

    float totalSceneProgress;

    public IEnumerator SceneLoadProgress(List<AsyncOperation> scenesLoading)
    {        
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                /*totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;*/

                yield return null;
            }
        }
    }

    public IEnumerator SaveDataProgress()
    {
        yield return new WaitForSeconds(2f);

        while (player == null)
        {
            yield return null;
        }

        if (!File.Exists(Application.persistentDataPath + "/" + loadedSaveFile + ".dat"))
        {
            SystemSave.SavePlayer(player, loadedSaveFile);
            Debug.Log("Created File: " + loadedSaveFile);
        }
        else
        {
            SystemSave.LoadPlayer(player, loadedSaveFile);
        }

        yield return new WaitForSeconds(1f);

        yield return fade.FadeOut();
        loadingScreen.Visibility(false);
    }

    public IEnumerator SaveQuitProgress()
    {
        yield return new WaitForSeconds(2f);

        while (player != null)
        {
            yield return null;
        }

        yield return fade.FadeOut();
        loadingScreen.Visibility(false);
    }

    public void OnApplicationQuit()
    {
        if (loadedSaveFile != null && player != null)
        {
            SystemSave.SavePlayer(player, loadedSaveFile);
        }
    }
}