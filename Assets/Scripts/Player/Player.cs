using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{
    public void Awake()
    {
        GameManager.player = this;
    }

    public void OnDestroy()
    {
        if (File.Exists(Application.persistentDataPath + "/" + GameManager.loadedSaveFile + ".dat"))
        {
            SystemSave.SavePlayer(this, GameManager.loadedSaveFile);
            Debug.Log("Saved Player: " + GameManager.loadedSaveFile);
        }

        GameManager.player = null;
    }
}
