using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{
    public void Awake()
    {
        GameManager.instance.player = this;
    }

    public void OnDestroy()
    {
        if (File.Exists(Application.persistentDataPath + "/" + GameManager.instance.loadedSaveFile + ".dat"))
        {
            DataManager.instance.SavePlayer(this, GameManager.instance.loadedSaveFile);
            Debug.Log("Saved Player: " + GameManager.instance.loadedSaveFile);
        }

        GameManager.instance.player = null;
    }
}
