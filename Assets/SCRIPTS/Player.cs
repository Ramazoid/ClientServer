using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkTransform nt;
    private void Awake()
    {
        
        print("AWAKE:" + name+" "+isLocalPlayer);
    }
    // Start is called before the first frame update
    void Start()
    {
        print("START:" + name + " " + isLocalPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
