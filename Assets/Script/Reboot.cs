using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reboot : MonoBehaviour, Ishootable
{
    public void Shoot(GameObject instigator)
    {
        SceneManager.LoadScene(0);
    }
}
