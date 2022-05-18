using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillWithPlayer : MonoBehaviour
{
    public PlayerController player;

    private BoxCollider2D b2d;
    private float avoidTimer;

    private void Start()
    {
        b2d = GetComponent<BoxCollider2D>();
        b2d.enabled = false;
    }

    private void Update()
    {
        if (player != null && player.dashed)
        {
            if (player.avoidTimer <= Time.time)
            {
                avoidTimer = Time.time + player.avoidTimer;
                b2d.enabled = true;
            }
        }

        if (avoidTimer <= Time.time)
        {
            b2d.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 19)
        {
            Destroy(collider.gameObject);
            if(player.dashCount < 2)
            {
                player.dashCount++;
            }
        }
    }
}
