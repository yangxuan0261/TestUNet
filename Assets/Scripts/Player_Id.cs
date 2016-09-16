using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Id : NetworkBehaviour
{
    [SyncVar]
    private string playerUniqueIdentity;
    private NetworkInstanceId playerNetId;
    private Transform myTransform;

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();
    }

    public void Start()
    {
        if (myTransform.name == "" || myTransform.name == "Player(Clone)")
        {
            SetIdentity(); //这里设置唯一name的是本机客户端中的其他玩家，本机玩家已经在OnStartLocalPlayer设置好了
        }
    }

    public void Awake()
    {
        myTransform = transform;
    }



    // Update is called once per frame
    void Update()
    {
        //if (myTransform.name == "" || myTransform.name == "Player(Clone)")
        //{
        //    SetIdentity();
        //}
    }

    /// <summary>
    /// 获取唯一id，生产一个唯一的name，并告诉服务端，同步到其他客户端
    /// </summary>
    void GetNetIdentity()
    {
        playerNetId = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentiy());
    }

    /// <summary>
    /// 如果是其他玩家，则从网络中同步唯一name
    /// 如果是本机玩家，则直接自己生成唯一name
    /// </summary>
    void SetIdentity()
    {
        if (!isLocalPlayer)
        {
            myTransform.name = playerUniqueIdentity;
        }
        else
        {
            myTransform.name = MakeUniqueIdentiy();
        }
    }

    string MakeUniqueIdentiy()
    {
        string uniqueName = "Player_" + playerNetId.ToString();
        return uniqueName;
    }

    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }
}
