using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class SyncData : NetworkBehaviour
{
    [SyncVar]
    public string syncText;

    public Text log;
    private int counter;
    
    public static SyncData IN;
    private static float nowTime;
    private static bool needtoSendFlags;

    public Queue<Vector3>qflags= new Queue<Vector3>() { };
    private UIPanelMan panelMan;

    private void Awake()
    {
        IN = this;
    }

    void Start()
    {

        FindObjectOfType<NetworkManagerHUD>().enabled = false;
        panelMan = FindObjectOfType<UIPanelMan>();
        //NetworkClient.RegisterHandler<DataMessage>(OnData);
        NetworkServer.RegisterHandler<DataMessage>(OnClientData);
        NetworkServer.RegisterHandler<FlagMessage>(OnFlagData);
    }

    private void OnFlagData(NetworkConnectionToClient client, FlagMessage mess)
    {
        Debug.Log($"Server Got FlagData[{mess.pos}] [{mess.col}]");
        Flags.CapturaAt(mess.pos, mess.col);
        SensFlagToAll(mess.pos, mess.col,client.connectionId);

    }

    private void SensFlagToAll(Vector3 pos, Color col, int connectionId)
    {
        Debug.Log("Sendto ALL server");
        foreach (var player in Flags.IN.players)
        {
            NetworkIdentity client = player.GetComponent<NetworkIdentity>();
            if (!client.isLocalPlayer)
                client.connectionToClient.Send(new FlagMessage(pos,col));

        }
    }
    public static void SrvrMessageToAll()
    {
        Debug.Log("Sendto ALL server");
        foreach (var player in Flags.IN.players)
        {
            NetworkIdentity client = player.GetComponent<NetworkIdentity>();
            if (!client.isLocalPlayer)
                client.connectionToClient.Send(new DataMessage("loose"));

        }

    }
    public static void ClientMessageToServer()
    {
        print("Sendto ALL client");
        NetworkClient.Send(new DataMessage("loose"));
    }
    private void OnClientData(NetworkConnectionToClient client, DataMessage mess)
    {
  
        Debug.Log($"CLIENT Got Data [{mess.mess}]");
        //Log.Show(mess.mess);
        if (mess.mess == "loose")
        {
            panelMan.Show("LoosePanel");
            SrvrMessageToAll();
        }
    }

    public void Sync()
    {
          counter = 0;
        print("---------------------============================================================================START SYNC");

        Caller();
    }

    private void Update()
    {
         if (needtoSendFlags)// && (Time.time - nowTime > 60))
         {
             needtoSendFlags = false;
             Sync();
         }
     
    }

    [ClientRpc]
    public void RpcSync(string pos)
    {
        print("*******************RPCSYNC:MOVER isServer=" + isServer);
        print("*******************RPCSYNC:MOVER isClient=" + isClient);
        print("*******************RPCSYNC:MOVER isOwned=" + isOwned);

        if (isClient&&!isServer) Parse(pos);
        
        
        //log.text += ":Client got:" + pos+"\n\n";
        


    }

    private void Parse(string pos)
    {
        print($"**Parser GOT :" + pos+"FlagsCount="+ Flags.IN.flags.Count);
        int index=0;
        print("**index=" + index);

        
        Vector3 vpos= JsonUtility.FromJson<Vector3>(pos);
        print("pos=" + vpos);
        qflags.Enqueue(vpos);
        //Flags.IN.flags[Flags.loadindex++].transform.position = vpos;

    }

    public void Caller()
    {
        while (counter < Flags.IN.flags.Count)
        {
            
            

            string pos = JsonUtility.ToJson(Flags.IN.flags[counter].transform.position);
            print($"-------------------------------sending-------------------Counter=[{pos}]");
            
            //if(this.connectionToServer.isReady)
            RpcSync(pos);
            counter++;
        }
        needtoSendFlags = false;
    }

    public static void OtherPlayerHere()
    {
        print("***************************************************************OTHER PLAYER HERE!!!!!!!!!!!!!!!!!!!!!!!");
        IN.Sync();
        nowTime = Time.time;
        needtoSendFlags = true;
    }

    public static void BroadcastLoose()
    {
        print("Sendto ALL server");
        NetworkServer.SendToAll(new DataMessage("Someone loose"));
        print("Sendto ALL client");
        NetworkClient.Send(new DataMessage("Someone loose"));

    }

    internal static void SendCaptured(Vector3 pos, Color col)
    {

        NetworkClient.Send(new FlagMessage(pos, col));
    }
}

public struct FlagMessage : NetworkMessage
{
    public Vector3 pos;
    public Color col;

    public FlagMessage(Vector3 pos, Color col)
    {
        this.pos = pos;
        this.col = col;
    }
}

public struct DataMessage : NetworkMessage
{
    public string mess;

    public DataMessage(string mess) : this()
    {
        this.mess = mess;
    }
}
