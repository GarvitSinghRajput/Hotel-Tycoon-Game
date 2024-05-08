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
    private float waitingTime = 2f;
 
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
                if (waitingTime <= 0)
                {
                    action?.Invoke();
                    waitingTime = 2f;
                }
                else
                    waitingTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == actionType.ToString())
        {
            PlayerManager.Instance.animator.SetBool(Global.InteractAnim, false);
            PlayerManager.Instance.animator.SetBool(Global.RunAnim, true);
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
    }
}
