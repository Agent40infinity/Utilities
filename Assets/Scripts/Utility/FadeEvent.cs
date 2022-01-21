using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEvent : MonoBehaviour
{
    public FadeController controller;

    public void Awake()
    {
        controller = gameObject.GetComponentInParent<FadeController>();
    }

    public void UpdateFadeState()
    {
        controller.fadeState = FadeState.Ended;
    }
}