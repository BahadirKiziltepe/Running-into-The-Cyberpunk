using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAvailability : MonoBehaviour
{
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            active = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            active = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            active = true;
        }
    }
}
