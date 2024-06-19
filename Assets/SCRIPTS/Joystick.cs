using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public RectTransform hand;
    private bool drag;
    private Vector3 startmouse;
    private Vector3 delta;
    private static float forward;
    public static Vector3 dir;
    public static bool ON;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            delta = Input.mousePosition - startmouse;


            if (delta.magnitude < 100)
                hand.anchoredPosition = delta;

            else
                hand.anchoredPosition = delta.normalized * 100;

            if (delta.magnitude > 10)
            {
                Joystick.ON = true;
                Joystick.dir = new Vector3(-delta.x, -delta.y,0);
            }
            else Joystick.ON = false;
        }
        else
        {
            hand.anchoredPosition = Vector3.Lerp(hand.anchoredPosition, Vector3.zero, 0.1f);
            Joystick.ON = false;
        }


        if (!Settings.CamDone)
        {
            hand.localScale = Vector3.one / 2;
            hand.ImgSetColor(new Color(1, 1,1,0.5f));
        }
        else
        {
            hand.localScale = Vector3.one;
            hand.ImgSetColor(Color.white);
        }



        }
    public void StartDrag()
    {
        if (!Settings.CamDone) return;
        drag = true;
        startmouse = Input.mousePosition;
    }
    public void StopDrag()
    {
        drag = false;
    }
}
