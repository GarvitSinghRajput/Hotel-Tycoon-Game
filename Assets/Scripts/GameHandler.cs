using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;

    public static int CoinCount;

    public Transform coinPanelPostition;
    public Text Text;
    public Transform objectHolder;
    public GameObject insufficientMoneyUI;
    public GameObject coinPrefab;
    public ActionPoint receptionPoint;
    public List<RoomManager> roomList;
    public GameObject guestPrefab;
    public float timeForAddNewGuest = 1f;
    public Transform exitTranform;
    public List<Transform> resCoinPosition;


    private int resPosIndex = 0;
    private int tipPosIndex = 0;
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
        if (CoinCount <= 0)
            CoinCount = 500;
        UpdateCoinCount(0);
        CheckRoomPreviousData();        
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(Global.PP_COINS,CoinCount);
    }

    private void OnApplicationPause()
    {
        PlayerPrefs.SetInt(Global.PP_COINS, CoinCount);
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> waitingQueuePositionList = new List<Vector3>();
        Vector3 firstPosition = new Vector3(0, 0, -3f);
        float positionSize = 1.5f;
        for (int i = 0; i < 5; i++)
        {
            waitingQueuePositionList.Add(firstPosition + new Vector3(0, 0, -1f) * positionSize * i);
        }
        waitingQueue = new WaitingQueue(waitingQueuePositionList);
        waitingQueue.OnGuestArrivedAtFirstOfQueue += OnGuestArrivedAtFirstOfQueue;
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

    private void OnGuestArrivedAtFirstOfQueue(object sender, System.EventArgs e)
    {
        
    }

    private void StandingOnReception()
    {
        RoomManager roomManager = IsRoomAvailable();
        if (roomManager == null)
            return;

        Guest guest = waitingQueue.GetFirstInQueue();
        guest.MoveTo(roomManager.navPosition.position, () => { StartCoroutine(ReachedInsideRoom(guest, roomManager)); });
        roomManager.isBooked = true;
        SpawnMoney(guest.transform.position + new Vector3(0, 0, 0), resCoinPosition[resPosIndex].position, 50);
        resPosIndex++;
        if (resPosIndex >= resCoinPosition.Count)
            resPosIndex = 0;
    }
    
    IEnumerator ReachedInsideRoom(Guest guest, RoomManager roomManager)
    {
        roomManager.hideRoomLight.SetActive(true);
        Vector3 pos = roomManager.furnitureAssets[0].transform.position;
        pos.y = 1f;
        guest.MoveTo(pos, () => 
        {
            guest.animator.SetBool(Global.RunAnim, false);
            guest.animator.SetBool(Global.LayAnim, true);
        });

        yield return new WaitForSeconds(3f);

        guest.MoveTo(roomManager.navPosition.position, () => 
        {
            guest.animator.SetBool(Global.LayAnim, false);
            guest.animator.SetBool(Global.RunAnim, true);
            SpawnMoney(guest.transform.position, roomManager.tipCoinPosition[tipPosIndex].position, Random.Range(10, 40));
            roomManager.isBooked = false;
            tipPosIndex++;
            if (tipPosIndex >= roomManager.tipCoinPosition.Count)
                tipPosIndex = 0;
            guest.MoveTo(exitTranform.position, () =>
            {
                Destroy(guest.gameObject);
            });
            roomManager.hideRoomLight.SetActive(false);
        });

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

    private void SpawnMoney(Vector3 pos, Vector3 movePos, int value)
    {
        GameObject go = Instantiate(coinPrefab, pos, Quaternion.identity);
        go.transform.DOMove(movePos, 0.75f).SetEase(Ease.OutSine);
        MoneyHandler moneyHandler = go.GetComponent<MoneyHandler>();
        moneyHandler.coinValue = value;
    }

    public MoneyHandler SpawnMoney(Vector3 pos)
    {
        GameObject go = Instantiate(coinPrefab, pos, Quaternion.identity);
        MoneyHandler moneyHandler = go.GetComponent<MoneyHandler>();
        return moneyHandler;
    }

    public void UpdateCoinCount(int value)
    {
        CoinCount += value;
        Text.DOText(CoinCount.ToString(), 2f, scrambleMode: ScrambleMode.Numerals);
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
                roomList[i].UnlockRoomData();
            }
            else
            {
                roomList[i].LockRoomData();
            }
        }
    }
}
