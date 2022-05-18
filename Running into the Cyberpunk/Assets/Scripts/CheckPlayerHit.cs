using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerHit : MonoBehaviour
{
    public PlayerController player;

    private BoxCollider2D b2d;
    private float avoidTimer;

    private void Start()
    {
        b2d = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (player != null && player.dashed)
        {
            if (player.avoidTimer <= Time.time)
            {
                avoidTimer = Time.time + player.avoidTimer;
                b2d.enabled = false;
            }
        }

        if(avoidTimer <= Time.time)
        {
            b2d.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 20)
        {
            Destroy(player);
        }
    }
}
