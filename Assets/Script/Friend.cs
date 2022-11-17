using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Friend : MonoBehaviour, Ishootable
{
    public Color deadColor;
    public TextMeshPro txt;

    private Renderer _rend;
    private GameMaster Level;

    private void Start()
    {
        GameObject[] GMs;
        GMs = GameObject.FindGameObjectsWithTag("GameMaster");
        Level = GMs[0].GetComponent<GameMaster>();
        _rend = GetComponent<Renderer>();
    }
    public void Shoot(GameObject instigator)
    {
        _rend.material.SetColor("_Color", deadColor);
        Level.FriendKill();
        txt.SetText("I'll get my revenge !!!");
    }
}
