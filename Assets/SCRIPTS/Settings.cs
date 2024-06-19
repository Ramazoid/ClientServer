using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class Settings : MonoBehaviour
{
    public static Color[] colors = new Color[] { Color.red, Color.yellow, Color.blue };
    public static int PlayerNumber=0;
    internal static bool CamDone;
    internal static bool LooseGame;
    internal static bool winGame;

    public static Color MyColor;

    internal static Color GetFreeColor()
    {
        print($"------------------Get color of player number[{PlayerNumber}]");
        if (PlayerNumber == colors.Length) PlayerNumber = 0;

        return colors[PlayerNumber++];
        
    }
}

