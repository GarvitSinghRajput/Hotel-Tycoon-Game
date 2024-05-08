using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guest : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public Rigidbody rb;
    private Vector3 _position;

    public void MoveTo(Vector3 pos, Action action = null)
    {
        //animator.SetBool(Global.RunAnim, true);
        //_position = transform.position;
        navMeshAgent.SetDestination(pos);
        action?.Invoke();
    }

    //private void Update()
    //{
    //    if (_position != transform.position)
    //    {
    //        animator.SetBool(Global.RunAnim, true);
    //        _position = transform.position;
    //    }
    //    else
    //    {
    //        animator.SetBool(Global.RunAnim, false);
    //    }
    //}
}
