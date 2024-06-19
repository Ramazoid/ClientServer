using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RpcTest : NetworkBehaviour
{
    public Text log;
    private UIPanelMan panelMan;

    void Start()
    {
        panelMan = FindObjectOfType<UIPanelMan>();
        //NetworkClient.RegisterHandler<DataMessage>(ClientGotData);
        //NetworkServer.RegisterHandler<DataMessage>(OnData2);
    }

    
    private void ClientGotData(DataMessage mess)
    {
        Debug.Log($"CLIENT Got Data [{mess.mess}] isServer=[{isServer}] isOwned=[{isOwned}]");
        Log.Show(mess.mess);
        if (mess.mess == "loose")
        {
            panelMan.Show("LoosePanel");
            MiniGame.Close();
        }
    
    }

    public void testClient()
    {
        RpcClientButton(Random.insideUnitSphere);
    }
    public void CommandTest()
    {
        CmdTest();
    }
    [Command]
    private void CmdTest()
    {
        log.text = "ButtonCommand" + Random.Range(0, 1000);
        Debug.Log("CmdTest");
    }

    [ClientRpc]
    public void RpcClientButton(Vector3 n)
    {
        log.text = "RpcClientButton" + n;
        Debug.Log("RpcClientButton" + n);
    }

    
}
