using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public float movespeed;
    public FixedJoystick joystick;
    public Rigidbody _rigidbody;
    public Animator animator;
    public float cameraZ_Offset = 7f;
    public Vector2 minimumXZ, maximumXZ;  
    
    private Vector3 direction;
    private Camera cam;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            return;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new(joystick.Horizontal * movespeed, _rigidbody.velocity.y, joystick.Vertical * movespeed); 
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(-_rigidbody.velocity);
            animator.SetBool(Global.RunAnim, true);
        }
        else
        {
            animator.SetBool(Global.RunAnim, false);
        }
    }

    private void LateUpdate()
    {
        cam.transform.position = new(transform.position.x, cam.transform.position.y, transform.position.z - cameraZ_Offset);
    }
}
