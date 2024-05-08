using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    public int coinValue;
    public RotateCoin rotateCoin;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Global.PLAYER_TAG)
        {
            rotateCoin.speed = 100f;
            GameHandler.Instance.UpdateCoinCount(coinValue);
            Destroy(rotateCoin.gameObject);
        }
    }

    public static void DeductMoney(int money)
    {
        GameHandler.Instance.UpdateCoinCount(-money);
    }

    public static void AddMoney(int money)
    {
        GameHandler.Instance.UpdateCoinCount(money);
    }
}
