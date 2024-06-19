using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private CanvasGroup cg;

    public GameObject[] icons = new GameObject[2];
    private static Tutorial IN;

    private void Awake()
    {
        IN = this;
    }


    void Start()
    {
        cg = GetComponent<CanvasGroup>(); cg.alpha = 0;
    }

    internal static void Show()
    {
        IN.StartCoroutine(IN.FadeIn());
    }
    IEnumerator FadeIn()
    {
        while (cg.alpha < 1)
        {
            cg.alpha += 0.01f;
            yield return true;
        }
    }

    internal static void Hide(int n)
    {
        IN.icons[n].gameObject.SetActive(false);
    }
}
