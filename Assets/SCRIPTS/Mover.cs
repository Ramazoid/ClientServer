using kcp2k;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Mover : NetworkBehaviour
{
    public Color MyColor;
    public float rotateY;

    private void Awake()
    {
        Camera.main.transform.position = new Vector3(-15.54f, 38.6300011f, -80.3700027f);
        print("AWAKE:MOVER AWAKE=" + isLocalPlayer);
        print("AWAKE:MOVER isServer=" + isServerOnly);
        print("AWAKE:MOVER isClient=" + isClientOnly);
        Flags.loadindex = 0;
         MyColor = Settings.GetFreeColor();
        Settings.MyColor = MyColor;
        GetComponent<Renderer>().material.color = MyColor;
    }

    void Start()
    {
        

        print("MOVER START=" + isLocalPlayer);
        print("START:MOVER* isServer=" + isServer);
        print("START:MOVER* isClient=" + isClient);
        print("START:MOVER* isOwned=" + isOwned);
        
        if (!isOwned)
            SyncData.OtherPlayerHere();
        print("START:MOVER* isClient=" + isClient);
        if (isLocalPlayer)
        {
            
            name = "PLAYER-MY";
            transform.LookAt(Vector3.zero);
            transform.position = Field.RandomPosition(2.2f);
            Camera.main.GetComponent<Driver>().SetPlayer(transform);
            GameObject.FindObjectOfType<Flags>().player = transform;
            rotateY = 0;
            
        }
        Flags.ADddPlayer(transform);
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.rotation = Quaternion.Euler(0, rotateY, 0);
        if (!isLocalPlayer) return;
            if (Joystick.ON)
        {
            Vector3 move = transform.right * Joystick.dir.x + transform.forward * Joystick.dir.y;
            if(Field.CheckBounds(transform.position, move * 0.001f))
            transform.position += move * 0.001f;
        }
        //Debug.DrawRay(transform.position, transform.forward, Color.red, 100);
    }

    private void ConnectedToServer()
    {
         
        print("*-*************************CONNECTED TO SERVER");
    }

    private void OnPlayerConnected(NetworkIdentity player)
    {
        print("*-*************************PLAYER CONNECTED TOSERVER");
    }


}
