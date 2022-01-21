using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public Animator fade; //Creates a reference for the Animator: Fade.
    public FadeState fadeState;

    public IEnumerator FadeOut() //Called upon to Fade Out.
    {
        fade.SetTrigger("FadeOut");
        yield return FadeWait();
    }

    public void FadeIn() //Called upon to Fade In.
    {
        fade.SetTrigger("FadeIn");
    }

    public IEnumerator FadeWait()
    {
        while (!FinishedFade())
        {
            yield return null;
        }
    }

    public bool FinishedFade()
    {
        switch (fadeState)
        {
            case FadeState.Ended:
                fadeState = FadeState.Idle;
                return true;
            default:
                return false;
        }
    }
}

public enum FadeState
{
    Idle,
    Ended
}
