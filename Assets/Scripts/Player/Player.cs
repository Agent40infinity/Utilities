using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Awake()
    {
        GameManager.player = this;
    }
}
