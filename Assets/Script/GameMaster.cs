using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float timerOffset = 0f;
    private bool timerRunning = false;

    private int _numTarget = 0;
    private int _shotTarget = 0;
    private bool _friendKilled = false;

    public void StartTimer()
    {
        timerOffset = Time.realtimeSinceStartup;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
    public float GetTimer()
    {
        return Mathf.Round(elapsedTime * 100f) / 100f;
    }

    public void AddTarget()
    {
        _numTarget += 1;
    }
    public void TargetShot()
    {
        _shotTarget += 1;
    }
    public void EndGame()
    {
        StopTimer();
        if(!_friendKilled)
        {
            gameObject.GetComponent<TextMeshPro>().SetText("Training completed." + '\n' + '\n' + "Target hit : " + _shotTarget + " / " + _numTarget + '\n' + "Time elapsed : " + GetTimer() + " s");
        }
        else
        {
            gameObject.GetComponent<TextMeshPro>().SetText("You suck !" + '\n' + "Your score is : 0" + '\n' + '\n' + "I told you I'd take revenge.");
        }
    }

    public void FriendKill()
    {
        _friendKilled = true;
    }

    private void FixedUpdate()
    {
        //Update the timer
        if(timerRunning)
        {
            elapsedTime = Time.unscaledTime - timerOffset;
        }
    }
}
