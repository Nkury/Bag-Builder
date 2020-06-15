using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerManager playerManager;

    public void Setup ( Context context )
    {
        playerManager = context.PlayerManager;
    }
}
