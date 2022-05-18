using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    private GameManager gm;

    public GameObject[] objectsToSpawn;
    public GameObject[] spawners;
    public int timerType;

    private Vector3 pos;
    private bool canSpawn, spawnTurret;
    private int objType, type, i;
    private float zTracker, zResetter, yPos = int.MaxValue;

    void Start()
    {
        gm = GameManager.instance;

        zTracker = 5;

        StartCoroutine(TurretSpawner());
    }

    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            if (objType == 1 || objType == 2 || objType == 3)
            {
                PlatformTypeOne();
            }
        }
    }

    void PlatformTypeOne()
    {
        type = Random.Range(1, 4);

        if (CheckAvailability())
        {
            if (yPos == int.MaxValue)
            {
                int dice = Random.Range(1, 7);
                if (dice <= 3)
                {
                    yPos = -7.5f;
                }
                else
                {
                    yPos = -4.5f;
                }
            }
            else
            {
                YPosCalculator();
            }
        }
    }

    void InstantiateObject(bool bottom, bool top)
    {

        pos = new Vector3(spawners[i].transform.position.x, yPos, zTracker++);
        zResetter++;
        if (zTracker == 9 || zResetter == 3)
        {
            zTracker = 5;
            zResetter = 0;
        }
        GameObject newObject = GameObject.Instantiate(objectsToSpawn[type], pos, Quaternion.identity);

        if (bottom)
        {
            newObject.GetComponent<MovingObjects>().objectType++;
        }
        if (top)
        {
            newObject.GetComponent<MovingObjects>().objectType += 2;
        }

        if(spawnTurret)
        {
            spawnTurret = false;
            newObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void YPosCalculator()
    {
        if (yPos == -4.5f)
        {
            if (CheckAvailability())
            {
                yPos = -7.5f;
                InstantiateObject(true, false);
                int dice = Random.Range(1, 7);
                if (type == 1)
                {
                    if (dice <= 3)
                    {
                        type = 2;
                    }
                    else
                    {
                        type = 3;
                    }
                }
                else if (type == 2)
                {
                    if (dice <= 3)
                    {
                        type = 1;
                    }
                    else
                    {
                        type = 3;
                    }
                }
                else
                {
                    if (dice <= 3)
                    {
                        type = 1;
                    }
                    else
                    {
                        type = 2;
                    }
                }
                yPos = -1.5f;
                InstantiateObject(false, true);
            }
        }
        else
        {
            yPos = -4.5f;
            InstantiateObject(false, false);
        }
    }

    bool CheckAvailability()
    {
        for (i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].GetComponent<SpawnerAvailability>().active)
            {
                return true;
            }
        }
        return false;
    }

    public void ObjectSpawner(int platformType)
    {
        this.objType = platformType;
        canSpawn = true;
    }

    IEnumerator TurretSpawner()
    {
        while (true)
        {
            if (gm.startGame == false)
            {
                yield return new WaitForSeconds(.15f);
            }

            yield return new WaitForSeconds(gm.timers[timerType]);
            spawnTurret = true;
        }
    }

}
