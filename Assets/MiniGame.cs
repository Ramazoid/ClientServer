using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGame : MonoBehaviour
{
    private static MiniGame IN;
    private UIPanelMan panelMan;
    private RectTransform rt;
    public RectTransform winzone;
    public RectTransform runner;
    public bool running;
    private Vector2 dir;
    public float speed;
    internal static bool isPlaying;
    private float fromX;
    private float toX;

    private void Awake()
    {
        IN = this;
    }

    void Start()
    {
        panelMan = FindObjectOfType<UIPanelMan>();
        rt = GetComponent<RectTransform>();
        rt.position = new Vector2(Screen.width / 2, Screen.height / 2);
        rt.sizeDelta = new Vector2(Screen.width / 2, 100);
        RandomizeWinzone();
        running = false;
        dir = Vector2.right;
        gameObject.SetActive(false);
    }

    private void RandomizeWinzone()
    {
        winzone.anchoredPosition = new Vector2(Random.Range(-rt.rect.width / 2, rt.rect.width / 2), 0);
        winzone.sizeDelta = new Vector2(Random.Range(50, 100), 100);
        runner.sizeDelta = new Vector2(10, 110);
        runner.anchoredPosition = new Vector2(-rt.rect.width / 2, 0);
         fromX = winzone.anchoredPosition.x - winzone.rect.width / 2;
        toX = winzone.anchoredPosition.x + winzone.rect.width / 2;
        print($"fromX=[{fromX}] ftoX=[{toX}]");
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (Input.GetMouseButtonDown(0)) Click();
            print(runner.anchoredPosition.x);
            runner.anchoredPosition += dir * speed;
            if (runner.anchoredPosition.x > rt.rect.width / 2d) dir = -dir;
            if (runner.anchoredPosition.x < -rt.rect.width / 2)
            {
                //running = false;
                dir = -dir;
            }
        }

    }

    public void Click()
    {
        print("Click");
        
        if (runner.anchoredPosition.x >= fromX && runner.anchoredPosition.x <= toX)
        {
            Settings.winGame = true;
            
        }
        else
        {
            print("*************LOOOSE****");
            Settings.LooseGame = true;SyncData.ClientMessageToServer();
        }
        Close();
    }

    public static void Launch()
    {
        
        IN.gameObject.SetActive(true); IN.panelMan.Hide("LoosePanel");
        IN.RandomizeWinzone();
        IN.running = true;
        isPlaying = true;
        Settings.LooseGame = false;
        Settings.winGame = false;
    }

    private void CloseGame()
    {
        gameObject.SetActive(false);
        isPlaying = false;
    
}
    internal static void Close()
    {
        IN.gameObject.SetActive(false);
        isPlaying = false;
    }
}
