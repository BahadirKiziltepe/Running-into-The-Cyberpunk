using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningProjectile1 : MonoBehaviour
{
    public GameObject indicator;
    public float speed;

    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()//-8.88686 -> 8 >>> 16.88686
    {
        indicator.transform.position = new Vector3(CalculateXPos(), 4, 0);
        targetPos = new Vector3(CalculateXPos(), 4, 0);
    }

    // Update is called once per frame
    void Update()
    {
        indicator.transform.position = Vector3.MoveTowards(indicator.transform.position, targetPos, speed * Time.deltaTime);
    }

    float CalculateXPos()
    {
        return (float)((Camera.main.aspect * 16.88686 / (1.777372)) - 8.88686);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            targetPos = new Vector3(CalculateXPos(), collision.transform.position.y, indicator.transform.position.z);
        }
    }
}
