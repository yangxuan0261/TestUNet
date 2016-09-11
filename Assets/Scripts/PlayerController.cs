using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public GameObject mBulletPrefab;
    public Transform mBulletSpawn;

	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
            return;

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKey(KeyCode.Space))
        {
            CmdFire();
        }
	}

    /// <summary>
    /// 加上 [Command] 告诉服务端生产一个子弹，然后同步给各个客户端这个子弹对象，这个对象一定要
    /// Network Manager中的 Registered Spawnabel Prefabs中注册过，方法的前缀一定要是Cmd
    /// </summary>
    [Command]
    public void CmdFire()
    {
        GameObject bullet = (GameObject)Instantiate(mBulletPrefab, mBulletSpawn.position, mBulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

        //告诉服务生产一个子弹，然后同步给各个客户端
        NetworkServer.Spawn(bullet);

        DestroyObject(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        //base.OnStartLocalPlayer();
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
