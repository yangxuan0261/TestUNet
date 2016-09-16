
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Server script
public class MyCombat : NetworkBehaviour
{
    public delegate void TakeDamageDelegate(int side, int damage);
    public delegate void DieDelegate();
    public delegate void RespawnDelegate();

    float deathTimer;
    bool alive = true;
    int health;

    [SyncEvent(channel = 1)]
    public event TakeDamageDelegate EventTakeDamage;

    [SyncEvent]
    public event DieDelegate EventDie;

    [SyncEvent]
    public event RespawnDelegate EventRespawn;

    [Server]
    void TakeDamage(int amount)
    {
        if (!alive)
            return;

        if (health > amount)
        {
            health -= amount;
        }
        else
        {
            health = 0;
            alive = false;
            // send die event to all clients
            EventDie();
            deathTimer = Time.time + 5.0f;
        }
    }

    [ServerCallback]
    void Update()
    {
        if (!alive)
        {
            if (Time.time > deathTimer)
            {
                Respawn();
            }
            return;
        }

        mText.text = "--- server update, time:" + Time.time;
        //EventTakeDamage((int)Time.time, (int)Time.time);
    }

    [Server]
    void Respawn()
    {
        alive = true;
        // send respawn event to all clients
        //EventRespawn();
    }

    private Text mText;
    public void Start()
    {
        if (isLocalPlayer || isServer)
            mText = GameObject.Find("textHUD").GetComponent<Text>();

        if (isLocalPlayer)
            EventTakeDamage += ClientTakeDamage;
    }

    public void ClientTakeDamage(int side, int damage)
    {
        mText.text = string.Format("--- ClientTakeDamage, side:{0}, damage:{1}"
            , side, damage);
    }
}

