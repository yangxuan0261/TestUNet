using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class BtnCtrl3 : MonoBehaviour
{

    NetworkClient myClient;
    Text mText;
    PlayerController mPlayerCtrl;

    // Use this for initialization
    void Start()
    {
        mText = GameObject.Find("CtrlText").GetComponent<Text>();
    }

    public class MyMsgType
    {
        public static short Score = MsgType.Highest + 1;
        public static short TestOne = MsgType.Highest + 2;
    };

    // Create a client and connect to the server port  
    public void SetupClient()
    {
        mPlayerCtrl = PlayerController.gPlayer;

        Debug.LogFormat("--- isLocalPlayer, SetupClient, total client:{0}", NetworkClient.allClients.Count);
        //myClient = NetworkManager.singleton.client;
        myClient = NetworkClient.allClients[0];
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MyMsgType.Score, OnScore);
        myClient.RegisterHandler(MyMsgType.TestOne, OnScore);
    }

    public void OnScore(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        string str = string.Format("--- Client OnScoreMessage\n score:{0}, \n pos:{1}, \n lives:{2}"
            , msg.score, msg.scorePos.ToString(), msg.lives);
        mText.text = str.ToString();
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
        mText.text = "Connected to server";
    }

    public void SendScore(int score, Vector3 scorePos, int lives, bool toggle)
    {
        ScoreMessage msg = new ScoreMessage();
        msg.score = score;
        msg.scorePos = scorePos;
        msg.lives = lives;

        if (toggle)
            myClient.Send(MyMsgType.Score, msg);
        else
            myClient.Send(MyMsgType.TestOne, msg);
    }

    private bool mToggle = true;
    public void OnClicked()
    {
        if (mToggle)
            SendScore(001, new Vector3(100.0f, 200.0f, 300.0f), 666, mToggle);
        else
            SendScore(002, new Vector3(200.0f, 300.0f, 400.0f), 777, mToggle);

        mToggle = mToggle ? false : true;
    }
}
