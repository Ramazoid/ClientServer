using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : NetworkBehaviour
{
    public Text log;
    private static Log IN;

    private void Awake()
    {
        IN = this;
    }

    void Start()
    {
        log = GetComponent<Text>();
    }

    
    void Update()
    {
    }

    internal static void Show(string v)
    {
        IN.log.text = v;
    }
}
