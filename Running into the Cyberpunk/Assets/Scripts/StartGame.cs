using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject player;
    public int layerNumber;

    private int killCounter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layerNumber)
        {
            player.SetActive(true);
            killCounter++;
            if(killCounter == 2)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
