using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarzanSC : MonoBehaviour, Ishootable
{
    public Vector2 RopeLimits = new Vector2(0.0f, 0.0f);
    private bool _iGrabed = false;
    private GameObject _player;
    private Camera _playerCam;
    private PlayerController _targetPC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(_iGrabed && !_targetPC.GetIgrounded())
        {
            //variables
            Vector3 direction = (transform.position - _player.transform.position).normalized;
            Vector3 camForward = _playerCam.transform.forward;
            Vector3 camRight = _playerCam.transform.right;
            
            Vector3 pendulumAxis = Vector3.Cross(direction, camRight).normalized;
        }
    }

    public void Shoot(GameObject instigator)
    {
        
        if(instigator.TryGetComponent(out PlayerController targetPC))
        {
            if(_iGrabed)
            {
                _iGrabed = false;
                _player = null;
            }
            else
            {
                _iGrabed = true;
                _player = instigator;
                _playerCam = targetPC.Cam;

            }
        }
    }
}
