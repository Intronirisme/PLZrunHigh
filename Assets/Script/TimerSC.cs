using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerSC : MonoBehaviour
{
    private GameMaster Level;

    private void Start()
    {
        
        GameObject[] GMs;
        GMs = GameObject.FindGameObjectsWithTag("GameMaster");
        Level = GMs[0].GetComponent<GameMaster>();
    }

    private void FixedUpdate()
    {
        float time = Level.GetTimer();
        gameObject.GetComponent<TextMeshPro>().SetText(time + "s");
    }
}