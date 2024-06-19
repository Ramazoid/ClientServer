using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Progress : MonoBehaviour
{
    public GameObject ProgressIndicator;
    public RectTransform progress;
    public Text tipText;
    public float pr;
    private static Progress IN;

    private UIPanelMan panelMan;

    private void Awake()
    {
        IN = this;
    }

    void Start()
    {
        ProgressIndicator.SetActive(false);
        pr = 0;
        UpdateProgress();
        tipText.text = "Capturing Flag";
        panelMan=FindObjectOfType<UIPanelMan>();    
    }

    public void UpdateProgress()
    {
        progress.sizeDelta = new Vector2(Screen.width * (pr / 100), 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Do(FlagPoint fp)
    {
        
        IN.ProgressIndicator.SetActive(true);
        panelMan.Show("ProgressPanel");
        
        DoProgress(fp);
       
        if (!MiniGame.isPlaying && !Settings.LooseGame)
        {
            int r = Random.Range(0, 10000);print("minigame random=" + r);
            if (r.Between(0, 100)) MiniGame.Launch();
        }
        
    }

    private static void DoProgress(FlagPoint fp)
    {
        if(!Settings.LooseGame)
        {
            IN.pr += 0.05f;fp.blinking = true; IN.tipText.text = "Capturing Flag";
            if (IN.pr >= 100)
                Flags.Capture(fp.gameObject);
            IN.UpdateProgress();
            IN.progress.ImgSetColor(Color.green);
            Settings.LooseGame = false;
            //
        }
        else
        {
            IN.progress.ImgSetColor(Color.red);
            IN.pr -= 0.03f;fp.blinking = false; IN.tipText.text = "Penalty Time accrued";
            IN.UpdateProgress();
            if(IN.pr<=0)
            {
                Settings.LooseGame = false;
                
            }
        }
    }

    public void Reset()
    {
        panelMan.Hide("ProgressPanel");
        if (!Settings.LooseGame)
        pr = 0;
        UpdateProgress();
        
    }
}
