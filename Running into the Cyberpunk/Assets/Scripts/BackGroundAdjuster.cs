using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundAdjuster : MonoBehaviour
{
    private GameManager gm;

    public int objectType;
    public float multiplier, width, height;

    private SpriteRenderer sRenderer;

    void Start()
    {
        gm = GameManager.instance;

        sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.size = new Vector2(CamRatio(), 12f);
    }

    void FixedUpdate()
    {
        multiplier = gm.multiplier[objectType];

        if (gm.startGame)
        {
            sRenderer.size += new Vector2((.01f * multiplier) / 50, 0f);
            if (sRenderer.size.x >= CamResetRatio())
            {
                sRenderer.size = new Vector2(CamRatio(), 12f);
                sRenderer.size += new Vector2((.001f * multiplier) / 50, 0f);
            }
        }
    }

    float CamRatio()
    {
        return (float)(Camera.main.aspect * 18 / (1.777372));
    }

    float CamResetRatio()
    {
        float resetRatio = (float)(CamRatio() * 3.1333 / (18));
        return resetRatio * CamRatio();
    }
}
