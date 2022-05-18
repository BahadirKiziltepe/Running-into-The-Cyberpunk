using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAdjuster : MonoBehaviour
{
    private GameManager gm;

    private Text score;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        score = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = ((gm.score * 100.00f) / 100.00f) + "";
    }
}
