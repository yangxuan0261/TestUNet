using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyMgr_custom : NetworkLobbyManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("--- OnClientConnect");
        base.OnClientConnect(conn);
    }

}
