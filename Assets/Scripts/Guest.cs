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

    public delegate void onSuccess();
    public void MoveTo(Vector3 pos, onSuccess action = null)
    {
        animator.SetBool(Global.RunAnim, true);
        navMeshAgent.SetDestination(pos);
        transform.LookAt(pos);
        StartCoroutine(MoveCorourtine(pos ,action));
    }

    IEnumerator MoveCorourtine(Vector3 pos, onSuccess action)
    {
        while(Vector3.Distance(transform.position, pos) > 0.5f)
        {
            yield return true;
        }
        action?.Invoke();
    }
}
