using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone1Controller : MonoBehaviour
{
    private GameManager gm;

    public GameObject player, projectile;
    public Sprite[] sprites;
    public float projectileTimer, positionTimer, speed, amplitude, frequency;

    private SpriteRenderer sRenderer;
    private Vector3 tempPos, target;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;

        sRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(PositionChanger());
    }

    private void FixedUpdate()
    {
        DroneMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Rotater();
        }

    }

    void DroneMovement()
    {
        if (Vector3.Distance(this.transform.position, target) != 0f)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, target, (.01f * speed));
        }
        else
        {
            Floater();
        }
    }

    IEnumerator PositionChanger()
    {
        while (true)
        {
            while (gm.startGame == false || gm.paused)
            {
                yield return new WaitForSeconds(.15f);
            }

            Vector3 camPos = Camera.main.transform.position;
            float xPos = Random.Range(camPos.x - 7.5f, camPos.x + 7.5f);
            float yPos = Random.Range(camPos.y - 5f, camPos.y + 5f);
            target = new Vector3(xPos, yPos, 0);

            yield return new WaitForSeconds(1f);
            GameObject.Instantiate(projectile, this.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(positionTimer - 1f);
        }
    }

    void Floater()
    {
        tempPos = target;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    void Rotater()
    {
        if (this.transform.position.x - player.transform.position.x <= 0)
        {
            if (Mathf.Abs(this.transform.position.x - player.transform.position.x) <= 1)
            {
                sRenderer.sprite = sprites[2];

            }
            else if (Mathf.Abs(this.transform.position.x - player.transform.position.x) <= 4)
            {
                sRenderer.sprite = sprites[3];
            }
            else
            {
                sRenderer.sprite = sprites[4];
            }
        }
        else
        {
            if (Mathf.Abs(this.transform.position.x - player.transform.position.x) <= 1)
            {
                sRenderer.sprite = sprites[2];

            }
            else if (Mathf.Abs(this.transform.position.x - player.transform.position.x) <= 4)
            {
                sRenderer.sprite = sprites[1];
            }
            else
            {
                sRenderer.sprite = sprites[0];
            }
        }
    }
}
