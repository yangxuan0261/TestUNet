using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

/// <summary>
/// 可以通过
/// 1. 减少发包率（意思就是 增大 sendInterval 发包间隔）
/// 2. 增大 closeEnough 距离
/// 3. 增大 normalLerpRate、fasterLerpRate 插值速率
/// </summary>

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class SmoothMove : NetworkBehaviour
{

    [SyncVar(hook = "SyncPostionsValues")]
    private Vector3 syncPos; //同步变量

    [SerializeField]
    Transform myTransform; //SerializeField用于inspector中显示非public变量
    private float lerpRate;
    private float normalLerpRate = 16.0f;
    private float fasterLerpRate = 27.0f;

    private Vector3 lastPos;
    private float threshold = 0.5f;

    private List<Vector3> syncPosList = new List<Vector3>();
    [SerializeField]
    private bool useHistoriicalLerping = false; //是否启用平滑插值的开关，直接在 inspector 中设置
    private float closeEnough = 0.11f;

    public void Start()
    {
        lerpRate = normalLerpRate;
    }

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
            if (useHistoriicalLerping) //更加平滑
            {
                HistoryLerping();
            }
            else
            {
                OrdinaryLerping();
            }
        }
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

    [Client]
    public void SyncPostionsValues(Vector3 lastPos)
    {
        syncPos = lastPos;
        syncPosList.Add(syncPos); //将所有服务端同步过来的 pos 全都保存在队列中
    }

    void OrdinaryLerping() //普通插值，有卡顿现象
    {
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    void HistoryLerping() //平滑插值
    {
        if (syncPosList.Count > 0)
        {
            //取出队列中的第一个设为插值的目标
            myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

            //位置足够接近，从队列中移除第一个，紧接着就是第二个
            if (Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
            {
                syncPosList.RemoveAt(0);
            }

            //如果同步队列过大，加快插值速率，使其更快到达目标点
            if (syncPosList.Count > 10)
            {
                lerpRate = fasterLerpRate;
            }
            else
            {
                lerpRate = normalLerpRate;
            }

            Debug.LogFormat("--- syncPosList, count:{0}", syncPosList.Count);
        }
    }
}
