using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.Networking.NetworkSystem;

public class BtnCtrl1 : NetworkBehaviour
{
    public class MyMsgType
    {
        public static short Score = MsgType.Highest + 1;
        public static short TestOne = MsgType.Highest + 2;
    };

    public void OnScore(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        Debug.Log("--- Server OnScoreMessage, score" + msg.score);
        msg.score += 200;

        //This applies to clients that are ready and not-ready.
        NetworkServer.SendToAll(MyMsgType.Score, msg);
    }

    public void OnTestOne(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        Debug.Log("--- Server OnScoreMessage, score" + msg.score);
        msg.score += 400;

        //Send a message to the client which owns the given connection ID.
        NetworkServer.SendToClient(netMsg.conn.connectionId, MyMsgType.TestOne, msg);
    }

    public void CmdSendScore(int score, Vector3 scorePos, int lives)
    {
        ScoreMessage msg = new ScoreMessage();
        msg.score = score;
        msg.scorePos = scorePos;
        msg.lives = lives;

    }

    void Start()
    {
        SetupServer();
    }

    public void SetupServer()
    {
        if (!isServer)
            return;

        Debug.Log("--- isServer");
        NetworkServer.RegisterHandler(MyMsgType.Score, OnScore);
        NetworkServer.RegisterHandler(MyMsgType.TestOne, OnTestOne);
    }
}
