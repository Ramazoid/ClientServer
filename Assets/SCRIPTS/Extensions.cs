using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class StringExtensions
{
    
    public static string Extract(this string s,char c1, char c2)
    {
        int n1 = s.IndexOf(c1);
        int n2 = s.IndexOf(c2);
        return s.Substring(n1+1, n2-n1-1);
    }
    public static string From(this string s,string fromstr)
    {
        int n1 = s.IndexOf(fromstr);
        int n2 = fromstr.Length;
        return s.Substring(n1 +  n2-1);
    }
}
public static class GameObjectExtensions
{
    public static T Child<T>(this GameObject g,string nam)
    {
        return g.transform.Find(nam).GetComponent<T>();
    }
}
public static class INtExtensions
{
    public static bool Between(this int n,int n1,int n2)
    {
        return (n > n1 && n < n2);
    }
}
public static class RecttransformExtensions
{
    public static void ImgSetColor(this RectTransform t, Color c)
    {
        t.GetComponent<Image>().color = c;
    }
}