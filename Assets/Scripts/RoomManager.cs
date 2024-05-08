using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RoomManager : MonoBehaviour
{
    public bool isBooked = false;
    public RoomData.RoomType roomType;
    public ActionPoint actionPoint;
    public List<GameObject> furnitureAssets;
    public GameObject LockedGO;
    public RoomManager nextRoomToUnlock;
    public Transform navPosition;
    public TextMeshPro priceText;
    public List<Transform> tipCoinPosition;
    public GameObject hideRoomLight;

    private bool isUnlocking = false;
    private int unlockCost;

    private void Start()
    {
        actionPoint.timeToFinish = 3.5f;
        unlockCost = (int)roomType * 100 + 100;
        priceText.SetText(unlockCost.ToString());
    }

    private void OnEnable()
    {
        actionPoint.action = UnlockRoom;
    }

    private void Update()
    {
        if (isUnlocking)
        {
            float money = Mathf.Lerp(0, unlockCost, actionPoint.timeToFinish);
            money = Mathf.RoundToInt(money);
            priceText.text = money.ToString();
        }
        else
            priceText.text = unlockCost.ToString();
    }

    private void UnlockRoom()
    {
        actionPoint.coroutine = StartCoroutine(UnlockRoomCor());
    }

    IEnumerator UnlockRoomCor()
    {
        if (GameHandler.CoinCount < unlockCost)
            CanNotBuy();
        while(actionPoint.timeToFinish > 0)
        {
            isUnlocking = true;
            actionPoint.timeToFinish -= Time.deltaTime;
            yield return true;
        }
        GameHandler.Instance.UpdateCoinCount(-unlockCost);
        LockedGO.SetActive(false);
        actionPoint.gameObject.SetActive(false);
        PlayerManager.Instance.animator.SetBool(Global.InteractAnim, false);
        PlayerManager.Instance.animator.SetBool(Global.RunAnim, true);
        if (nextRoomToUnlock != null)
            nextRoomToUnlock.actionPoint.gameObject.SetActive(true);
        isUnlocking = false;
        PlayerPrefs.SetInt(Global.ROOM + (int)roomType, 1);
    }

    private void CanNotBuy()
    {
        GameHandler.Instance.ActivateBrokeUI();
        StopAllCoroutines();
    }

    public void UnlockRoomData()
    {
        LockedGO.SetActive(false);
        actionPoint.gameObject.SetActive(false);
        if (nextRoomToUnlock != null)
            nextRoomToUnlock.actionPoint.gameObject.SetActive(true);
    }

    public void LockRoomData()
    {
        LockedGO.SetActive(true);
    }
}

public class RoomData
{
    public enum RoomType {LeftDown, RightDown, LeftUp, RightUp, LeftTop, RightTop}    
}