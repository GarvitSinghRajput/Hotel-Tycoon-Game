using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyHandler : MonoBehaviour
{
    public int coinValue;
    public RotateCoin rotateCoin;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Global.PLAYER_TAG)
        {
            rotateCoin.speed = 100f;
            Vector3 pos = Camera.main.ScreenToWorldPoint(new(Screen.width - 100f, Screen.height,10f));
            transform.DOMove(pos, 1f).OnComplete(() => UpdateCoins());
        }
    }

    private void UpdateCoins()
    {
        GameHandler.Instance.UpdateCoinCount(coinValue);
        Destroy(rotateCoin.gameObject);
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
