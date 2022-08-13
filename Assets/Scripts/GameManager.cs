using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Linq;
using DevLocker.Utils;

public class GameManager : MonoBehaviour
{
    [Header("Instance")]
    public static GameManager instance;

    [Header("General")]
    public string loadedSaveFile;
    public Animator saveIcon;
    public Player player;
    public bool introPlayed;
    public Settings optionsMenu;

    [Header("Manager")]
    public LoadingScreen loadingScreen;
    public FadeController fade;
    public List<SceneReference> exclusionScenes;
    public List<SceneReference> startingScenes;
    public List<SceneReference> GameScenes;

    [Header("Settings")]
    public AudioMixer masterMixer; //Creates reference for the menu musi

    public void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        instance = this;
        saveIcon = GameObject.FindWithTag("SaveIcon").GetComponent<Animator>();
        masterMixer = Resources.Load("Music/Mixers/Master") as AudioMixer; //Loads the MasterMixer for renference.

        SceneHandler.LoadScenes(startingScenes);
    }

    public void LoadGame()
    {
        loadingScreen.Visibility(true);

        List<AsyncOperation> scenesLoading = SceneHandler.LoadScenes(GameScenes);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("Menu_Main"));

        StartCoroutine(LoadProgression(scenesLoading, true));
    }

    public void QuitGame()
    {
        loadingScreen.Visibility(true);

        List<AsyncOperation> scenesLoading = SceneHandler.SwapScenes(startingScenes, exclusionScenes);

        StartCoroutine(LoadProgression(scenesLoading, false));
    }

    public IEnumerator LoadProgression(List<AsyncOperation> scenesLoading, bool isLoad)
    {
        StartCoroutine(SceneLoadProgress(scenesLoading));

        switch (isLoad)
        {
            case true:
                yield return StartCoroutine(SaveDataProgress());
                break;
            case false:
                yield return StartCoroutine(SaveQuitProgress());
                break;
        }

        yield return fade.FadeOut();
        loadingScreen.Visibility(false);
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
            DataManager.instance.SavePlayer(player, loadedSaveFile);
            Debug.Log("Created File: " + loadedSaveFile);
        }
        else
        {
            DataManager.instance.LoadPlayer(player, loadedSaveFile);
        }

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator SaveQuitProgress()
    {
        yield return new WaitForSeconds(2f);

        while (player != null)
        {
            yield return null;
        }
    }

    public void OnApplicationQuit()
    {
        if (loadedSaveFile != null && player != null)
        {
            DataManager.instance.SavePlayer(player, loadedSaveFile);
        }
    }
}