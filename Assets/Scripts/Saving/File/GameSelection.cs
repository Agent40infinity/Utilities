using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSelection : MonoBehaviour
{
    public Menu menu;

    public void Awake()
    {
        menu = GameObject.Find("Main").GetComponent<Menu>();
    }

    public void CreateLoad()
    {
        string name = gameObject.name.Replace("Save", "Save_");
        GameManager.loadedSaveFile = name;

        menu.CallStart();
    }
}