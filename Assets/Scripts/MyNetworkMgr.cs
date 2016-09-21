using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkMgr : NetworkManager
{
    private int s_StartPositionIndex = 0;

    /// <summary>
    /// 重写实例化对象，使用自定义的prefab，这个prefab必须在spawnPrefabs列表中
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="playerControllerId"></param>
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject go = null;
        if (spawnPrefabs.Count > 0)
            go = spawnPrefabs[spawnPrefabs.Count - 1];
        else
            go = playerPrefab;

        var player = (GameObject)GameObject.Instantiate(go, GetStartPos().position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    private Transform GetStartPos()
    {
        if (playerSpawnMethod == PlayerSpawnMethod.Random && startPositions.Count > 0)
        {
            // try to spawn at a random start location
            int index = Random.Range(0, startPositions.Count);
            return startPositions[index];
        }
        else if (playerSpawnMethod == PlayerSpawnMethod.RoundRobin && startPositions.Count > 0)
        {
            if (s_StartPositionIndex >= startPositions.Count)
            {
                s_StartPositionIndex = 0;
            }

            Transform startPos = startPositions[s_StartPositionIndex];
            s_StartPositionIndex += 1;
            return startPos;
        }
        else
            return startPositions[0];
    }

}
