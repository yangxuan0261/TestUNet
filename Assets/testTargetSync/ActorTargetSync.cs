using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ActorTargetSync : NetworkBehaviour
{
    GameObject go = null;
    float counter = 0.0f;
    public int mAge = 1;
    public bool Testing = true;

    public override void OnStartClient()
    {
        gameObject.name = "Player_" + GetComponent<NetworkIdentity>().netId;
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    /// <summary>
    /// 只在服务端中Update，每3秒SetDirtyBit(1)，序列化OnSerialize 同步一次变量 mAge
    /// </summary>
    [ServerCallback]
    public void Update()
    {
        //Debug.Log("--- server Update");
        if (!Testing)
            return;

        counter += Time.deltaTime;
        if (counter > 3.0f)
        {
            SetDirtyBit(1);
            mAge += 1;
            counter = 0.0f;
        }
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        //Debug.LogFormat("--- OnSerialize");
        //writer.Write(mAge);


        return true;
    }

    /// <summary>
    /// 客户端收到服务端的同步，反序列化出来 mAge
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="initialState"></param>
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        //mAge = reader.ReadInt32();
        //Debug.LogFormat("--- OnDeserialize, name:{0}, age:{1}", gameObject.name, mAge);
    }
}
