using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVehicle : MonoBehaviour
{
    private GameManager gm;

    public GameObject projectile1, indicator, unlem;
    public Vector3[] projectile1Pos;
    public int projectile1Limit, timerType;
    public float projectile1WaitTime;
    public bool active, ongoing;

    private float enemyVehicle1Time;
    private bool onhold;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;

        enemyVehicle1Time = gm.timers[timerType];
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.startGame && active)
        {
            active = false;
            StartCoroutine(SpawnProjectile1(0));
        }


        if (gm.fightActive || gm.enemy2Active)
        {
            onhold = true;
        }
        else
        {
            onhold = false;
        }

        if (ongoing)
        {
            gm.enemy1Active = true;
        }
        else
        {
            gm.enemy1Active = false;
        }
    }

    IEnumerator SpawnProjectile1(int selectedObj)
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyVehicle1Time - 3f);
            while (onhold)
            {
                yield return new WaitForSeconds(1f);
            }
            ongoing = true;

            unlem.GetComponent<Animator>().SetInteger("state", 1);

            yield return new WaitForSeconds(3f);
            unlem.GetComponent<Animator>().SetInteger("state", 0);
            this.GetComponent<Animator>().SetInteger("state", 1);

            yield return new WaitForSeconds(1f);
            this.GetComponent<Animator>().SetInteger("state", 0);

            yield return new WaitForSeconds(5f);
            indicator.GetComponent<SpriteRenderer>().enabled = true;

            for (int i = 0; i < projectile1Limit; i++)
            {
                selectedObj = Randomizer(selectedObj);
                GameObject.Instantiate(projectile1, projectile1Pos[selectedObj], Quaternion.identity);
                if (i == projectile1Limit - 1)
                {
                    yield return new WaitForSeconds(projectile1WaitTime);
                    indicator.GetComponent<SpriteRenderer>().enabled = false;
                }
                yield return new WaitForSeconds(projectile1WaitTime);
            }

            yield return new WaitForSeconds(2.5f);
            ongoing = false;
        }
    }

    int Randomizer(int selectedObj)
    {
        int dice = Random.Range(1, 7);
        if (selectedObj == 0)
        {
            if (dice > 3)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        else if (selectedObj == 1)
        {
            if (dice > 3)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (dice > 3)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
