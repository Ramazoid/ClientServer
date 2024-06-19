using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPanelMan : MonoBehaviour
{
    public List<UIPanel> panels = new List<UIPanel>();
    private static UIPanelMan IN;

    void Start()
    {
        panels = FindObjectsOfType<UIPanel>().ToList<UIPanel>();
    }

    private void Awake()
    {
        IN = this;
    }


    void Update()
    {

    }

    public void Show(string panelName)
    {
        UIPanel panel = GetPanelByName(panelName); 
        panel.Show();
    }

    private UIPanel GetPanelByName(string panelName)
    {
        UIPanel panel = panels.Find(p => p.name == panelName);
        if (panel == null)
            throw new Exception($"Panel[{panelName}] not found!");
        return panel;
    }

    internal void Hide(string panelName)
    {
        UIPanel panel = GetPanelByName(panelName);
        panel.Hide();
    }
}
