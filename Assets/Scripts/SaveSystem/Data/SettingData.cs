using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class SettingData
{
    public Dictionary<string, KeyCode> keybinds; //String that is used to store the converted dictionary.
    public float masterMixer; //Variable for master volume.
    public float effectsMixer; //Variable for effects volume.
    public float musicMixer; //Variable for music volume.
    public float ambienceMixer;//Variable for ambience volume.

    public void SaveSettings() //Used when saving file
    {
        keybinds = InputManager.instance.keybind; //Using Json converter to serialize the data within the InputManager.instance.keybind dictionary and converts it to a string.

        float value;
        if (GameManager.instance.masterMixer.GetFloat("Master", out value)) //If the mixer within MasterMixer exists, saves the value to it's respective variable so json conversion - Repeats 4 times for each Mixer.
        {
            masterMixer = value;
        }
        if (GameManager.instance.masterMixer.GetFloat("Effects", out value))
        {
            effectsMixer = value;
        }
        if (GameManager.instance.masterMixer.GetFloat("Music", out value))
        {
            musicMixer = value;
        }
        if (GameManager.instance.masterMixer.GetFloat("Ambience", out value))
        {
            ambienceMixer = value;
        }
    }

    public void LoadSettings() //Used when loading file
    {
        //Debug.Log(DictionaryDebug.Call(keybinds));
        InputManager.instance.keybind = keybinds; //Deserializes the Dictionary data stored in the json and loads it back into the static keybinds dictionary.
        GameManager.instance.masterMixer.SetFloat("Master", masterMixer); //Loads the values stored in the .json data back into the mixers.
        GameManager.instance.masterMixer.SetFloat("Effects", effectsMixer);
        GameManager.instance.masterMixer.SetFloat("Music", musicMixer);
        GameManager.instance.masterMixer.SetFloat("Ambience", ambienceMixer);
        Debug.Log(DictionaryDebug.Call(keybinds));
    }
}