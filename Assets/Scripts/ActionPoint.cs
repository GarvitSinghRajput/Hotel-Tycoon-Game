using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoint : MonoBehaviour
{
    [HideInInspector]
    public float timeToFinish;
    public Coroutine coroutine;
    public Global.Tags actionType;
    public Action action;
    public bool isReceptionPoint;
    private float actionWaitingTime = 3f;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == actionType.ToString())
        {
            action?.Invoke();
            PlayerManager.Instance.animator.SetBool(Global.RunAnim, false);
            PlayerManager.Instance.animator.SetBool(Global.InteractAnim, true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == actionType.ToString())
        {
            if (isReceptionPoint)
            {
                if (actionWaitingTime <= 0)
                {
                    action?.Invoke();
                    actionWaitingTime = 3f;
                }
                else
                    actionWaitingTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == actionType.ToString())
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                timeToFinish = 3.5f;
            }
            PlayerManager.Instance.animator.SetBool(Global.InteractAnim, false);
            PlayerManager.Instance.animator.SetBool(Global.RunAnim, true);
        }
    }
}
