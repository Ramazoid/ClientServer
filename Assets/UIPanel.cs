using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    private RectTransform rt;
    public Vector2 normalPos;
    
    private bool repos;
    private Vector2 toPos;
    public Vector2 farPos;

    internal void Show()
    {
        repos = true;
        toPos = normalPos;
    }

    // Start is called before the first frame update
    void Start()
    {
     rt = GetComponent<RectTransform>();   
        rt.anchoredPosition += farPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(repos)
        {
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, toPos, 0.1f);
            if (Vector2.Distance(rt.anchoredPosition, toPos) <= 1) repos = false;

        }
    }

    internal void Hide()
    {
        repos = true;
        toPos = farPos;
    }
}
