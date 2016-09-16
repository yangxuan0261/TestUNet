using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScoreMessage : MessageBase
{
    public int score;
    public Vector3 scorePos;
    public int lives;

    public override void Deserialize(NetworkReader reader)
    {
        //base.Deserialize(reader);
        score = reader.ReadInt32();
        scorePos = reader.ReadVector3();
        lives = reader.ReadInt32();
        Debug.LogFormat("--- Deserialize, score:{0}, pos:{1}, lives:{2}"
            , score, scorePos, lives);
    }

    public override void Serialize(NetworkWriter writer) 
    {
        //base.Serialize(writer);
        Debug.Log("--- Serialize");
        writer.Write(score);
        writer.Write(scorePos);
        writer.Write(lives);
    }
}
