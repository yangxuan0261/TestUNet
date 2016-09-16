using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy_motionSync : NetworkBehaviour
{

    [SyncVar]
    private Vector3 syncPos;
    [SyncVar]
    private float syncYRot;

    private Vector3 lastPos;
    private Quaternion lastRot;
    private Transform myTransform;
    private float lerpRate = 10.0f;
    private float posThreshold = 0.5f;
    private float rotThreshold = 5.0f;

    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        TransmitMotion();
        LerpMotion();
    }



    /// <summary>
    /// 在关卡加载的时候调用
    /// </summary>
    /// <param name="level"></param>
    public void OnLevelWasLoaded(int level)
    {

    }



    /// <summary>
    /// enemy是由服务端生成的，这段代码只有服务端才需要执行
    /// </summary>
    void TransmitMotion()
    {
        if (!isServer)
            return;

        if (Vector3.Distance(myTransform.position, lastPos) > posThreshold
            || Quaternion.Angle(myTransform.rotation, lastRot) > rotThreshold)
        {
            lastPos = myTransform.position;
            lastRot = myTransform.rotation;

            syncPos = myTransform.position;
            syncYRot = myTransform.localEulerAngles.y;
        }
    }

    /// <summary>
    /// 插值过程是展示上的东西，服务端不需要执行
    /// </summary>
    void LerpMotion()
    {
        if (isServer)
            return;

        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);

        Vector3 newRot = new Vector3(0.0f, syncYRot, 0.0f);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(newRot), Time.deltaTime * lerpRate);
    }
}
