using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            transform.position = player.transform.position;
        }
    }
}
