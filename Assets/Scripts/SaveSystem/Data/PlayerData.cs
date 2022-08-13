using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

[System.Serializable]
public class PlayerData
{
    public float[] playerPos = new float[3];

    public PlayerData(Player player) //Creates a reference for the Player and is used as the baseline for all data being saved into "save.dat".
    {
        Vector3 pos = player.gameObject.transform.position;
        playerPos[0] = pos.x;
        playerPos[1] = pos.y;
        playerPos[2] = pos.z;
    }

    public void LoadData(Player player)
    {
        Vector3 pos = new Vector3(playerPos[0], playerPos[1], playerPos[2]);
        player.transform.position = pos;
    }
}
