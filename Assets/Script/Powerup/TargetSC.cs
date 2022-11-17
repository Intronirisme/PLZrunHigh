using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSC : MonoBehaviour, Ishootable
{
    public Color deadColor;

    private Renderer _rend;

    private GameMaster Level;

    private void Start()
    {
        GameObject[] GMs;
        GMs = GameObject.FindGameObjectsWithTag("GameMaster");
        Level = GMs[0].GetComponent<GameMaster>();
        Level.AddTarget();
        _rend = GetComponent<Renderer>();
    }
    public void Shoot(GameObject instigator)
    {
        _rend.material.SetColor("_Color", deadColor);
        Level.TargetShot();
    }
}
