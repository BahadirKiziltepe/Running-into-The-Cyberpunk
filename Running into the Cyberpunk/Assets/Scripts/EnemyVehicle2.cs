using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVehicle2 : MonoBehaviour
{
    private GameManager gm;

    public GameObject projectile2;
    public Vector3[] projectile2Pos;
    public Vector3 Projectile2Rotation;
    public int projectile2Limit, timerType;
    public float projectile2WaitTime;
    public bool active, ongoing;

    private float enemyVehicle2Time;
    private bool onhold;
    private int taken = -1;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;

        enemyVehicle2Time = gm.timers[timerType];
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.startGame && active)
        {
            active = false;
            int selectedObj = Random.Range(0, 3);
            StartCoroutine(SpawnProjectile2(0, 0));
        }

        if (gm.fightActive || gm.enemy1Active)
        {
            onhold = true;
        }
        else
        {
            onhold = false;
        }

        if (ongoing)
        {
            gm.enemy2Active = true;
        }
        else
        {
            gm.enemy2Active = false;
        }
    }

    IEnumerator SpawnProjectile2(int selectedObj, int pos)
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyVehicle2Time);
            while (onhold)
            {
                yield return new WaitForSeconds(2f);
            }
            ongoing = true;

            for (int i = 0; i < 3; i++)
            {
                this.GetComponent<Animator>().SetInteger("state", selectedObj);
                yield return new WaitForSecondsRealtime(1f);
                this.GetComponent<Animator>().SetInteger("state", 3);
                yield return new WaitForSecondsRealtime(2f);

                for (int j = 0; j < projectile2Limit; j++)
                {
                    pos = Randomizer(pos);
                    Debug.Log(selectedObj);
                    Vector3 newObjPos = projectile2Pos[selectedObj];
                    for (int k = 0; k < pos; k++)
                    {
                        newObjPos = new Vector3(newObjPos.x - 1f, newObjPos.y, newObjPos.z);
                    }
                    GameObject newProjectile2 = GameObject.Instantiate(projectile2, newObjPos, Quaternion.identity);
                    newProjectile2.transform.Rotate(Projectile2Rotation, Space.Self);

                    yield return new WaitForSeconds(projectile2WaitTime);
                }
                selectedObj = Randomizer2(selectedObj, i);
            }

            yield return new WaitForSeconds(2.5f);
            ongoing = false;
        }
    }

    int Randomizer(int pos)
    {
        int dice = Random.Range(1, 13);
        if (pos == 0)
        {
            if (dice >= 1 && dice <= 3)
            {
                return 1;
            }
            else if (dice >= 4 && dice <= 6)
            {
                return 2;
            }
            else if (dice >= 7 && dice <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else if (pos == 1)
        {
            if (dice >= 1 && dice <= 3)
            {
                return 0;
            }
            else if (dice >= 4 && dice <= 6)
            {
                return 2;
            }
            else if (dice >= 7 && dice <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else if (pos == 2)
        {
            if (dice >= 1 && dice <= 3)
            {
                return 0;
            }
            else if (dice >= 4 && dice <= 6)
            {
                return 1;
            }
            else if (dice >= 7 && dice <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else if (pos == 3)
        {
            if (dice >= 1 && dice <= 3)
            {
                return 0;
            }
            else if (dice >= 4 && dice <= 6)
            {
                return 1;
            }
            else if (dice >= 7 && dice <= 9)
            {
                return 2;
            }
            else
            {
                return 4;
            }
        }
        else
        {
            if (dice >= 1 && dice <= 3)
            {
                return 0;
            }
            else if (dice >= 4 && dice <= 6)
            {
                return 1;
            }
            else if (dice >= 7 && dice <= 9)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }

    int Randomizer2(int selectedObj, int i)
    {

        if (i == 0)
        {
            int dice = Random.Range(1, 7);
            if (selectedObj == 0)
            {
                if (dice > 3)
                {
                    taken = 1;
                    return 1;
                }
                else
                {
                    taken = 2;
                    return 2;
                }
            }
            else if (selectedObj == 1)
            {
                if (dice > 3)
                {
                    taken = 0;
                    return 0;
                }
                else
                {
                    taken = 2;
                    return 2;
                }
            }
            else
            {
                if (dice > 3)
                {
                    taken = 0;
                    return 0;
                }
                else
                {
                    taken = 1;
                    return 1;
                }
            }
        }
        else if (i == 1)
        {
            if (selectedObj == 0)
            {
                if (taken == 1)
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
                if (taken == 0)
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
                if (taken == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        else
        {
            taken = -1;
        }

        return -1;
    }
}
