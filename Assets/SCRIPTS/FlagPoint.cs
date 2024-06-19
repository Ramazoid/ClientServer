using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPoint : NetworkBehaviour
{
    public Color col;

    private Renderer ren;
    
    private SpriteRenderer sren;
    
    public bool blinking;
    internal bool captured;
    public float startBlinkig;

    internal void SetColor(Color color)
    {
        col = color;
        sren.color = col; blinking = false; sren.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
        col=ren.material.color;
        sren = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltatime = Time.time - startBlinkig;
        if (deltatime<2 && !captured && Settings.CamDone)
            sren.enabled = !sren.enabled;
        else
            sren.enabled = true;
        ren.material.color = col;
    }
}
