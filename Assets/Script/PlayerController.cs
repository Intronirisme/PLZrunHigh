using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public Camera Cam;

    [Header("Movement")]
    public float Speed = 5f;
    public float TurnRate = 1f;
    public float LookupRate = 1f;
    public float JumpSpeed = 3f;
    public float GroundDamping = .1f;

    [Header("Air control")]
    public float AirControl = .3f;
    public float AirDamping = .01f;
    public float GravitySpeed = 10f;

    private CharacterController _controls;
    private GameObject _cam_root;
    private GameObject _body;
    private bool _iShooting = false;
    private bool _iGrounded = false;
    private Vector2 _moveDir = Vector2.zero;

    private Vector3 _walkVelocity = Vector3.zero;
    private Vector3 _impulse = Vector3.zero;
    private Vector3 _platformMovement = Vector3.zero;
    // Start is called before the first frame update
    private void Start()
    {
        _controls = gameObject.GetComponent<CharacterController>();
        _cam_root = gameObject.FindInChildren("CameraRoot");
        _body = gameObject.FindInChildren("Body");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        TestGround();

        float damping = _iGrounded ? GroundDamping : AirDamping;
        float speed = _iGrounded ? Speed : Speed * AirControl;

        if (_moveDir.magnitude > 0.1f) //move if input
        {
            Vector2 calibratedMove = _moveDir.Rotate(_cam_root.transform.rotation.eulerAngles.y * -1) * -1; //it works I don't know why
            Vector3 walkDir = new Vector3(calibratedMove.x, 0f, calibratedMove.y).normalized;
            _walkVelocity = walkDir * speed;
        }
        else //decelerate
        {
            _walkVelocity -= _walkVelocity * damping;
        }

        if (!_iGrounded) _impulse.y = Mathf.Clamp(_impulse.y - GravitySpeed*Time.fixedDeltaTime, -20f, 20f); //Apply Gravity

        _impulse.x -= _impulse.x * damping;
        _impulse.z -= _impulse.z * damping;

        Vector3 movement = (_impulse + _walkVelocity) * Time.fixedDeltaTime;
        _controls.Move(movement + _platformMovement);
    }

    public bool GetIgrounded()
    {
        return _iGrounded;
    }

    private void TestGround()
    {
        RaycastHit hit;
        _iGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1f);

        if(_iGrounded)
        {
            MovingSC moveSC = hit.collider.gameObject.GetComponent<MovingSC>(); //test for plateform
            if(moveSC != null)
            {
                _platformMovement = moveSC.GetMovement();
            }
            else
            {
                _platformMovement = Vector3.zero;
            }
        }
    }

    public void OnFire(InputAction.CallbackContext value)
    {
        if(value.ReadValueAsButton() != _iShooting)
        {
            _iShooting = value.ReadValueAsButton();
            if(_iShooting)
            {
                Vector3 aimDirection = Cam.transform.forward;
                Vector3 shootPos = _cam_root.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(shootPos, aimDirection, out hit, 200f))
                {
                    Ishootable hitShotI = hit.collider.gameObject.GetComponent<Ishootable>();
                    if (hitShotI != null) { hitShotI.Shoot(gameObject); }
                }
            }
        }


    }
    public void OnJump(InputAction.CallbackContext value)
    {
        if(_iGrounded && value.ReadValueAsButton())
        {
            _impulse.y = JumpSpeed;
        }
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        Vector3 inRot = _cam_root.transform.rotation.eulerAngles;
        inRot.z = 0f;
        inRot.y += input.x * TurnRate;
        inRot.x -= input.y * LookupRate;
        inRot.x = Mathf.Clamp((inRot.x+180)%360, 95f, 269f)-180;
        _cam_root.transform.rotation = Quaternion.Euler(inRot);

        _body.transform.rotation = Quaternion.Euler(new Vector3(0f, inRot.y, 0f));
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        _moveDir = value.ReadValue<Vector2>();
    }

    public void SetImpulse(Vector3 impulse)
    {
        _impulse = impulse;
    }
}


public static class ExtensionMethods
{
    public static Transform FindInChildren(this Transform self, string name)
    {
        int count = self.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = self.GetChild(i);
            if (child.name == name) return child;
            Transform subChild = child.FindInChildren(name);
            if (subChild != null) return subChild;
        }
        return null;
    }

    public static GameObject FindInChildren(this GameObject self, string name)
    {
        Transform transform = self.transform;
        Transform child = transform.FindInChildren(name);
        return child != null ? child.gameObject : null;
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}