using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkMgr_custom : NetworkManager
{
    /// <summary>
    /// 启动作为主机
    /// </summary>
    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    /// <summary>
    /// 加入某个ip地址并启动作为客户端
    /// </summary>
    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    public void SetIPAddress()
    {
        string IpAddress = GameObject.Find("InputFieldIPAddress").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = IpAddress;
    }

    public void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    /// <summary>
    /// 根据加载不同的scene，监听不同的ui事件
    /// </summary>
    /// <param name="level"></param>
    public void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            //SetupMenuSceneButtons();

            //Fixed issue: 重新启动失败问题
            //视频讲的太快没太听明白，大概是因为disconnect后，返回到
            //菜单界面，里面点击 再次启动StartupHost，之前的旧的 Network Manager 可能
            //还占用者端口，所以再次启动时会失败
            StartCoroutine(SetupMenuSceneButtons());
        }
        else
        {
            SetupOtherSceneButtons();
        }
    }

    /// <summary>
    /// 菜单界面的ui事件
    /// </summary>
    public IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    /// <summary>
    /// 主界面中的ui事件
    /// </summary>
    public void SetupOtherSceneButtons()
    {
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }
}
