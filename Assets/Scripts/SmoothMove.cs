using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[NetworkSettings (channel = 0, sendInterval = 0.033f)]
public class SmoothMove : NetworkBehaviour
{

    [SyncVar]
    private Vector3 syncPos; //同步变量

    [SerializeField]
    Transform myTransform; //SerializeField用于inspector中显示非public变量

    private Vector3 lastPos;
    private float threshold = 0.5f;

    [SerializeField]
    float lerpRate = 15.0f;

    public void Update()
    {
        LerpPosition(); //因为方法利用了Time.deltaTime，所以只能在 Updata中调用
    }

    public void FixedUpdate() //1. server 和 client 都执行FixedUpdate
    {
        TransmitPosition(); //2. 因为是 ClientCallback，所以只有客户端调用
    }

    void LerpPosition()
    {
        if (!isLocalPlayer) //5. 只有非本机玩家才进行插值移动到最新的 syncPos 位置
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
        NetworkManager.singleton.client.GetRTT();
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos; //4. 服务端收到信息同步给所有客户端的该对象的syncPos变量
    }

    [Client]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold) //3. 只用本机玩家才提交位置信息到server上
        {
            CmdProvidePositionToServer(myTransform.position);
        }
    }
}
