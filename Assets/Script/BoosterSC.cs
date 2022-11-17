using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterSC : MonoBehaviour, Ishootable
{
    public Vector3 TargetDirection;
    public float Strenght = 10f;
    public float Cooldown = 2f;
    public Color offColor;

    private Renderer _rend;
    private Color _baseColor;
    private bool _active = true;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        _baseColor = _rend.material.GetColor("_Color");
    }
    public void Shoot(GameObject instigator)
    {
        PlayerController targetPC = instigator.GetComponent<PlayerController>();
        if(targetPC != null && _active)
        {
            Vector3 direction = (transform.position - instigator.transform.position).normalized;
            targetPC.SetImpulse(direction * Strenght);
        }
        StartCoroutine(Regen());
    }

    IEnumerator Regen()
    {
        _rend.material.SetColor("_Color", offColor);
        _active = false;
        yield return new WaitForSeconds(Cooldown);
        _rend.material.SetColor("_Color", _baseColor);
        _active = true;
    }
}
