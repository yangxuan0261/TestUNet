using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public const int mMaxHealth = 100;

    /// <summary>
    /// 同步各个客户端的变量mCurrHealth
    /// 当变量mCurrHealth变化的时候，会回调OnChangeHealth方法，并把值传进去该方法
    /// </summary>
    [SyncVar(hook = "OnChangeHealth")]
    public int mCurrHealth = mMaxHealth;
    public RectTransform mHealthBar;
    public bool mIsDestroyOnDead;
    public NetworkStartPosition[] mSpawnPosVec;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            mSpawnPosVec = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int _amount)
    {
        //Q:为什么要加 isServer判断呢？
        //A:因为里面后面执行的代码里有个同步的变量mCurrHealth，以服务端为主，不然所有客户端都同步有个n*n的同步复制度
        //so, 凡是有涉及到需要 "同步变量SyncVar" 和调用 "ClientRpc方法" 的代码，都需要进行 isServer 判断，谁叫服务端才是老大呢
        if (!isServer)
            return;

        mCurrHealth -= _amount;
        if (mCurrHealth <= 0)
        {
            if (mIsDestroyOnDead)
            {
                DestroyObject(gameObject);
            }
            else
            {
                mCurrHealth = mMaxHealth;
                RpcRespwan(); //服务端调用所有的客户端，
            }

        }

    }

    public void OnChangeHealth(int _health)
    {
        mHealthBar.sizeDelta = new Vector2(_health * 2.0f, mHealthBar.sizeDelta.y);
    }

    /// <summary>
    /// 服务端调用所有客户端的函数，方法的前缀一定要是Rpc
    /// </summary>
    [ClientRpc]
    public void RpcRespwan()
    {
        if (isLocalPlayer) //可以理解为pc才可以重生, npc则不重生
        {
            Vector3 spawnPos = Vector3.zero;
            if (mSpawnPosVec != null && mSpawnPosVec.Length > 0)
            {
                spawnPos = mSpawnPosVec[Random.Range(0, mSpawnPosVec.Length)].transform.position;
            }
            transform.position = spawnPos;
        }
    }



    // Update is called once per frame
    void Update()
    {

    }


}
