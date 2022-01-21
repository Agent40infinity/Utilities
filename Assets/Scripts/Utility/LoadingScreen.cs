using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public FadeController fade;

    public void Visibility(bool active)
    {
        fade.FadeIn();
        gameObject.SetActive(active);
    }
}