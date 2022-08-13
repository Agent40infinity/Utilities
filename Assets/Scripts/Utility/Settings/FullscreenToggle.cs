using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    public Dictionary<int, FullScreenMode> options = new Dictionary<int, FullScreenMode>
    {
        { 0, FullScreenMode.ExclusiveFullScreen },
        { 1, FullScreenMode.FullScreenWindow },
        { 2, FullScreenMode.Windowed },
    };
    int index;

    public void CallToggleFullscreen(bool increased)
    {
        switch (increased)
        {
            case true:
                ToggleFullscreen(IndexLooper.Increment(index, options.Count));
                break;
            case false:
                ToggleFullscreen(IndexLooper.Decrement(index, options.Count));
                break;
        }
    }

    public void ToggleFullscreen(int index) //Trigger for applying fullscreen
    {
        Debug.Log(index);

        Screen.fullScreenMode = options[index];
    }
}
