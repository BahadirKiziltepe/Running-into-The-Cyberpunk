using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret1Controller : MonoBehaviour
{
    private GameManager gm;

    public GameObject projectile;
    public Sprite[] sprites;
    public float projectileTimer, projectileCounter, range;

    private SpriteRenderer sRenderer;
    private PlayerController player;
    private bool canShoot;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;

        sRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(PorjectileSpawner());
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = PlayerController.instance;
        }

        if (player != null)
        {
            Rotater();
            if (Vector3.Distance(this.transform.position, player.transform.position) < range && canShoot == false)
            {
                canShoot = true;
            }
        }
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

    IEnumerator PorjectileSpawner()
    {
        while (gm.startGame == false || gm.paused || canShoot == false || player == null)
        {
            yield return new WaitForSeconds(.15f);
        }

        int i = 0;
        while (i < projectileCounter)
        {
            i++;
            GameObject.Instantiate(projectile, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(projectileTimer);
        }
    }
}
