using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int mMaxHealth = 100;

    /// <summary>
    /// 同步各个客户端的变量mCurrHealth
    /// 当变量mCurrHealth变化的时候，会回调OnChangeHealth方法，并把值传进去
    /// </summary>
    [SyncVar (hook = "OnChangeHealth")]
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
                RpcRespwan();
            }

        }

    }

    public void OnChangeHealth(int _health)
    {
        mHealthBar.sizeDelta = new Vector2(_health * 2.0f, mHealthBar.sizeDelta.y);
    }

    [ClientRpc]
    public void RpcRespwan()
    {
        if (isLocalPlayer)
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
	void Update () {
	
	}
}
