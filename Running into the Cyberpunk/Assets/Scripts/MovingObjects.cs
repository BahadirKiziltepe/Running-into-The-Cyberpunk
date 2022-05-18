using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour
{
    private GameManager gm;
    private Spawner spawner;

    public int objectType;
    public float multiplier, yDif;
    public bool willFall;

    private Rigidbody2D r2d;
    private PlayerController player;
    private Vector3 target;
    private float yDifIncreaser;
    private bool visible, falling, reachedTarget;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        spawner = Spawner.instance;

        visible = false;
        r2d = GetComponent<Rigidbody2D>();
        yDifIncreaser = yDif;
        target = new Vector3(0, 0, float.MaxValue);

        StartCoroutine(FallingObjects());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            player = PlayerController.instance;
        }

        multiplier = gm.multiplier[objectType];

        if (gm.startGame)
        {
            if (objectType >= 1 && objectType <= 3)
            {
                if (falling && gm.fightActive == false)
                {
                    r2d.velocity = new Vector2(-.01f * multiplier, r2d.velocity.y - yDif);
                    if (yDif < .1)
                    {
                        yDif += yDifIncreaser;
                    }
                    else
                    {
                        yDif = 1f;
                    }
                }
                else
                {
                    r2d.velocity = new Vector2(-.01f * multiplier, r2d.velocity.y);
                }
            }
            else if (objectType == 4)
            {
                r2d.velocity = new Vector2(-.01f * multiplier, r2d.velocity.y);
            }
            else if (objectType == 5)
            {
                float x = -Mathf.Abs(Mathf.Sin(35f));
                float y = -Mathf.Abs(Mathf.Sin(55f));
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + x, transform.position.y + y), (.01f * multiplier));
            }
            else if (objectType == 6 && player != null)
            {
                if (reachedTarget)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, target, (.01f * multiplier) / 2);
                    if (Vector3.Distance(this.transform.position, target) == 0f)
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    if (target.z == float.MaxValue)
                    {
                        target = player.transform.position;
                    }
                    this.transform.position = Vector3.MoveTowards(this.transform.position, target, (.01f * multiplier));
                    if (Vector3.Distance(this.transform.position, target) == 0f)
                    {
                        target = player.transform.position;
                        reachedTarget = true;
                    }
                }
            }
            else if (objectType == 7 && player != null)
            {

                if (target.z == float.MaxValue)
                {
                    target = player.transform.position;
                }
                this.transform.position = Vector3.MoveTowards(this.transform.position, target, (.01f * multiplier));
                if (Vector3.Distance(this.transform.position, target) == 0f)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {

            }
        }
    }

    IEnumerator FallingObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(.15f);
            if (willFall)
            {
                while (gm.fightActive)
                {
                    yield return new WaitForSeconds(.15f);
                }

                yield return new WaitForSeconds(1f);
                falling = true;
            }
        }

    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    private void OnBecameInvisible()
    {
        if (visible)
        {
            spawner.ObjectSpawner(objectType);
            if (objectType >= 1 && objectType <= 3)
            {
                gm.score += 1f * gm.scoreMultiplier;
            }
            Destroy(this.gameObject);
        }
    }
}
