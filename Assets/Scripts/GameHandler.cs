using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;

    public static int CoinCount;

    public Transform coinPanelPostition;
    public TextMeshProUGUI coinText;
    public Transform objectHolder;
    public GameObject insufficientMoneyUI;
    public GameObject coinPrefab;
    public Transform MoneyArea;
    public ActionPoint receptionPoint;
    public List<RoomManager> roomList;
    public GameObject guestPrefab;
    public float timeForAddNewGuest = 2f;

    private float timeCounter = 0;
    private WaitingQueue waitingQueue;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            return;

        receptionPoint.action = StandingOnReception;
        CoinCount = PlayerPrefs.GetInt(Global.PP_COINS);
        if (CoinCount == 0)
            CoinCount = 500;
        UpdateCoinCount(0);
        CheckRoomPreviousData();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(Global.PP_COINS,CoinCount);
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> waitingQueuePositionList = new List<Vector3>();
        Vector3 firstPosition = new Vector3(0, 0, -3f);
        float positionSize = 1f;
        for (int i = 0; i < 5; i++)
        {
            waitingQueuePositionList.Add(firstPosition + new Vector3(0, 0, -1) * positionSize * i);
        }
        waitingQueue = new WaitingQueue(waitingQueuePositionList);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeCounter > 0)
        {
            timeCounter -= Time.deltaTime;
            return;
        }
        if (waitingQueue.CanAddGuest())
        {
            GameObject go = Instantiate(guestPrefab,new Vector3(0,0,-15), Quaternion.identity);
            go.transform.SetParent(objectHolder);
            Guest guest = go.GetComponent<Guest>();
            waitingQueue.AddGuest(guest);
            timeCounter = timeForAddNewGuest;
        }
    }

    private void StandingOnReception()
    {
        RoomManager roomManager = IsRoomAvailable();
        if (roomManager == null)
            return;

        Guest guest = waitingQueue.GetFirstInQueue();
        guest.MoveTo(roomManager.navPosition.position);
        roomManager.isBooked = true;
        SpawnMoneyReservation(guest.transform.position);
    }

    private RoomManager IsRoomAvailable()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            if (!roomList[i].LockedGO.activeSelf && roomList[i].isBooked == false)
                return roomList[i];
        }
        return null;
    }

    private void SpawnMoneyReservation(Vector3 pos)
    {
        SpawnMoney(pos, 50);
    }

    private void SpawnMoney(Vector3 pos, int value)
    {
        GameObject go = Instantiate(coinPrefab, MoneyArea.position, Quaternion.identity);
        //go.transform.Translate(MoneyArea.position,Space.Self);
        MoneyHandler moneyHandler = go.GetComponent<MoneyHandler>();
        moneyHandler.coinValue = value;
    }

    public void UpdateCoinCount(int value)
    {
        CoinCount += value;
        coinText.SetText(CoinCount.ToString());
    }

    public void ActivateBrokeUI()
    {
        insufficientMoneyUI.SetActive(true);
        Invoke("DisableBrokeUI", 2.5f);
    }

    private void DisableBrokeUI()
    {
        insufficientMoneyUI.SetActive(false);
    }

    private void CheckRoomPreviousData()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            int value = PlayerPrefs.GetInt(Global.ROOM + i);
            if (value == 1)
            {
                roomList[i].UpdateRoom();
            }
        }
    }
}
