using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDoor : MonoBehaviour, Ishootable
{
    public float translation = 1f;
    public float duration = 1f;
    public bool start = true;

    private GameMaster Level;
    private bool iOpen = false;

    //animation related
    private float _translationProgress = 0f;
    private float _speed;
    private Vector3 _originalPos;

    private void Start()
    {
        GameObject[] GMs;
        GMs = GameObject.FindGameObjectsWithTag("GameMaster");
        Level = GMs[0].GetComponent<GameMaster>();
        _speed = translation / duration;
        _originalPos = gameObject.transform.position;
    }

    public void Shoot(GameObject instigator)
    {
        if(!iOpen)
        {
            Open();
            if(start)
            {
                Level.StartTimer();
            }
            else
            {
                Level.EndGame();
            }
        }
    }

    private void Open()
    {
        iOpen = true;
        StartCoroutine(OpenAnim());
    }

    IEnumerator OpenAnim()
    {
        while(_translationProgress < translation)
        {
            Vector3 fwd = gameObject.transform.forward;
            _translationProgress += Time.fixedDeltaTime * _speed;
            gameObject.transform.position = _originalPos + (_translationProgress*fwd);
            yield return new WaitForFixedUpdate();
        }
    }
}
