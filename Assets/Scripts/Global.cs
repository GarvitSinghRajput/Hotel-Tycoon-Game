using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    // Tags
    public static readonly string PLAYER_TAG = "Player";
    public static readonly string NPC_TAG = "NPC";

    //Player Pref keys
    public static readonly string PP_COINS = "Coin";
    public static readonly string ROOM = "Room";

    //Animation Paramerters
    public static readonly string RunAnim = "Run"; 
    public static readonly string LayAnim = "Laying"; 
    public static readonly string InteractAnim = "Interact"; 


    // Enums
    public enum Tags {Player, NPC}
}
