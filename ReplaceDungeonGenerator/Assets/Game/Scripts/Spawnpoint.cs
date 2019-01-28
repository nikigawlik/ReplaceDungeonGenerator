using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}
