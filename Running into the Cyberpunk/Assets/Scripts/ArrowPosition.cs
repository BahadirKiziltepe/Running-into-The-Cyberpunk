using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPosition : MonoBehaviour
{
    public Vector3[] pauseMenuPos;
    public Vector3[] mainMenuPos;
    public float addTimer;

    private GameManager gm;
    private GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        arrow = this.gameObject;
        arrow.transform.position = pauseMenuPos[gm.option];
    }

    // Update is called once per frame
    void Update()
    {
        MoveArrow(gm.option);
    }

    void MoveArrow(int option)
    {
        arrow.transform.position = pauseMenuPos[option];
    }
}
