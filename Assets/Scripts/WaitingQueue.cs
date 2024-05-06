using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingQueue : MonoBehaviour
{
    private List<Guest> guestList;
    private List<Vector3> positionList;
    private Vector3 entrancePosition;
    public WaitingQueue(List<Vector3> positionList)
    {
        this.positionList = positionList;
        entrancePosition = positionList[positionList.Count - 1] + new Vector3(0, 0, -1);

        //for (int i = 0; i < positionList.Count; i++)
        //{
        //    Debug.LogError(positionList[i]);
        //}
        //Debug.LogError(entrancePosition);

        guestList = new List<Guest>();
    }

    public bool CanAddGuest()
    {
        return guestList.Count < positionList.Count;
    }

    public void AddGuest(Guest guest)
    {
        guestList.Add(guest);
        guest.MoveTo(entrancePosition, () => {
            guest.MoveTo(positionList[guestList.IndexOf(guest)]);
        });
    }

    public Guest GetFirstInQueue()
    {
        if (guestList.Count <= 0)
        {
            return null;
        }
        else
        {
            Guest guest = guestList[0];
            guestList.RemoveAt(0);
            RelocateAllGuest();
            return guest;
        }
    }

    private void RelocateAllGuest()
    {
        for (int i = 0; i < guestList.Count; i++)
        {
            guestList[i].MoveTo(positionList[i]);
        }
    }
}
