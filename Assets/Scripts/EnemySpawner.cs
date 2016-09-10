using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject mEnemyPrefab;
    public int mTotalNum;

    public override void OnStartServer()
    {
        for (int i = 0; i< mTotalNum; ++i)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8.0f, 8.0f), 0.0f, Random.Range(-8.0f, 8.0f));
            Quaternion spawnRotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 180.0f), 0.0f);

            GameObject enemy = (GameObject)Instantiate(mEnemyPrefab, spawnPos, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
