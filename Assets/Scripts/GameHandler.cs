using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject guestPrefab;
    private WaitingQueue waitingQueue;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> waitingQueuePositionList = new List<Vector3>();
        Vector3 firstPosition = new Vector3(0, 0, -3);
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
        if (waitingQueue.CanAddGuest())
        {
            GameObject go = Instantiate(guestPrefab,new Vector3(0,0,-15), Quaternion.identity);
            Guest guest = go.GetComponent<Guest>();
            waitingQueue.AddGuest(guest);
        }
    }
}
