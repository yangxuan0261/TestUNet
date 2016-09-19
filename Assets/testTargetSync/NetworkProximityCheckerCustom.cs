// The default NetworkProximityChecker requires a collider on the same object,
// but in some cases we want to put the collider onto a child object (e.g. for
// animations).
//
// We modify the NetworkProximityChecker source from BitBucket to support
// colliders on child objects by searching the NetworkIdentity in parents.
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkProximityCheckerCustom : NetworkProximityChecker
{
    GameObject mGo;
    public float mRadius = 5.0f;

    public void Start()
    {
        mGo = gameObject;
    }

    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        return base.OnRebuildObservers(observers, initialize);
    }

    // function from 



    //public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initial)
    //{
    //    if (forceHidden)
    //    {
    //        // ensure player can still see themself
    //        var uv = GetComponent<NetworkIdentity>();
    //        if (uv.connectionToClient != null)
    //        {
    //            observers.Add(uv.connectionToClient);
    //        }
    //        return true;
    //    }

    //    // find players within range
    //    switch (checkMethod)
    //    {
    //        case CheckMethod.Physics3D:
    //            {
    //                var hits = Physics.OverlapSphere(transform.position, visRange);
    //                foreach (var hit in hits)
    //                {
    //                    // (if an object has a connectionToClient, it is a player)
    //                    var uv = hit.GetComponent<NetworkIdentity>();           //  < -----DEFAULT
    //                    //var uv = hit.GetComponentInParent<NetworkIdentity>(); //    <----- MODIFIED
    //                    if (uv != null && uv.connectionToClient != null)
    //                    {
    //                        observers.Add(uv.connectionToClient);
    //                    }
    //                }
    //                return true;
    //            }

    //        case CheckMethod.Physics2D:
    //            {
    //                var hits = Physics2D.OverlapCircleAll(transform.position, visRange);
    //                foreach (var hit in hits)
    //                {
    //                    // (if an object has a connectionToClient, it is a player)
    //                    var uv = hit.GetComponent<NetworkIdentity>();          //   < -----DEFAULT
    //                    //var uv = hit.GetComponentInParent<NetworkIdentity>(); //    <----- MODIFIED
    //                    if (uv != null && uv.connectionToClient != null)
    //                    {
    //                        observers.Add(uv.connectionToClient);
    //                    }
    //                }
    //                return true;
    //            }
    //    }
    //    return false;
    //}

    /// <summary>
    /// 当有新玩家加入游戏的时候，每个联网物体的OnCheckObserver函数都会被调用。
    /// 如果他返回了True，那么新加入的玩家就会被加入到物体的可见区域中。
    /// 这个函数执行一个简单的距离判断逻辑。
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public override bool OnCheckObserver(NetworkConnection connection)
    {
        uint id = GetComponent<NetworkIdentity>().netId.Value;
        Debug.LogFormat("--- {0}:{1}, OnCheckObserver, connectionId:{2} enter my visible area"
            , gameObject.name
            , id
            , connection.connectionId);
        return base.OnCheckObserver(connection);
    }

    /// <summary>
    /// 绘制可见区域
    /// </summary>
    public void OnDrawGizmos()
    {
        if (Application.isPlaying)
            Gizmos.color = Color.yellow;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, mRadius);
    }
}
