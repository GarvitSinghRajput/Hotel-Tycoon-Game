using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guest : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public void MoveTo(Vector3 pos, Action action = null)
    {
        navMeshAgent.destination = pos;
        action?.Invoke();
    }
}
