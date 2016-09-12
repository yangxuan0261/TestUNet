using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.Networking.NetworkSystem;

public class BtnCtrl2 : MonoBehaviour
{

    NetworkClient myClient;

    public class MyMsgType
    {
        public static short Score = MsgType.Highest + 1;
    };


    

    // Create a client and connect to the server port  
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MyMsgType.Score, OnScore);
        myClient.Connect("127.0.0.1", 7777);
    }

    public void OnScore(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        Debug.Log("--- Client OnScoreMessage " + msg.score);
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }

    public void CmdSendScore(int score, Vector3 scorePos, int lives)
    {
        ScoreMessage msg = new ScoreMessage();
        msg.score = score;
        msg.scorePos = scorePos;
        msg.lives = lives;

        myClient.Send(MyMsgType.Score, msg);
    }

    // Use this for initialization
    void Start()
    {
        SetupClient();
    }

    public void OnClicked()
    {
        CmdSendScore(123, new Vector3(100.0f, 200.0f, 300.0f), 666);
    }
}
