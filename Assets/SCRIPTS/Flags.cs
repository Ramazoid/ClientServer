using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flags : NetworkBehaviour
{
    public GameObject flagPrefab;
    public int flagNum;
    private Progress progress;
    public float flagZoneR;
    public Transform player;
    public List<GameObject> flags = new List<GameObject>();
    public static Flags IN;
    internal static int loadindex;
    public List<Transform> players = new List<Transform>();
    private Mover capturer;
    private FlagPoint captured;

    private void Awake()
    {
        IN = this;
    }


    void Start()
    {
        progress = FindObjectOfType<Progress>();
        flagZoneR = flagPrefab.transform.Find("Zone").transform.lossyScale.x;
        Field.IN.SetLimiters();

        if (SyncData.IN.qflags.Count > 0)
        {
            
            int n = 0;
            while (SyncData.IN.qflags.Count > 0)
            {
                GameObject flag = Instantiate(flagPrefab);
                FlagPoint fp = flag.GetComponent<FlagPoint>();
                fp.startBlinkig = Time.time;
                flag.name = "FLAG_R_" + (n++);
                flag.transform.position = SyncData.IN.qflags.Dequeue();
                 flags.Add(flag);
            }
        }
        else
            for (int i = 0; i < flagNum; i++)
            {
                GameObject flag = Instantiate(flagPrefab);
                FlagPoint fp = flag.GetComponent<FlagPoint>();
                fp.startBlinkig = Time.time;
                flag.name = "FLAG_" + i;


                Vector3 flagPos;
                do
                {
                    float x = Random.Range(Field.IN.minx, Field.IN.maxx);
                    float z = Random.Range(Field.IN.minz, Field.IN.maxz);
                    flagPos = new Vector3(x, 0, z) * Field.IN.field.transform.lossyScale.x;
                } while (!CheckNearest(flagPos, flagZoneR * 3));
                flag.transform.position = flagPos;
                flag.transform.localScale = Vector3.one;
                flags.Add(flag);
            }
        print("+++++++++++++++++++++++++++++++++++++++++++ FLAG DONE");
    }

    private bool CheckNearest(Vector3 flagPos, float distance)
    {

        foreach (GameObject flag in flags)
        {
            if (Vector3.Distance(flag.transform.position, flagPos) < distance)
                return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        capturer = null;
        
       if(captured != null)captured.blinking = false;
        captured = null;
        
        CheckPlayers();
        if (captured)
        {
            
            if(capturer.isLocalPlayer)
                progress.Do(captured);
        }
        else
            progress.Reset();



    }

    private void CheckPlayers()
    {
        foreach(Transform player in players)
        {
            if(player)
           CheckPlayer(player);
        }
    }

    private void CheckPlayer(Transform player)
    {
        foreach (GameObject flag in flags)
        {
            FlagPoint fp = flag.GetComponent<FlagPoint>();
            if (Vector3.Distance(flag.transform.position, player.position) < flagZoneR * 2)
            {
                //print($"player[{player.name}] on da flag[{fp.name}]");
                fp.startBlinkig = Time.time;
                capturer = player.GetComponent<Mover>();
                captured = fp;
            }
        }
    }

    internal static void Capture(GameObject flag)
    {
        GameObject f = IN.flags.Find((f) => { return f == flag; });
        MiniGame.Close();

        FlagPoint fp = f.transform.GetComponentInChildren<FlagPoint>();
        if (!fp.captured)
        {
            IN.flags.Remove(f);
            fp.SetColor(Settings.MyColor); SyncData.SendCaptured(f.transform.position, Settings.MyColor);
            fp.captured= true;
        }
        
    }

    [ClientRpc]
    public void RpcAjustFlag(int i, Vector3 pos)
    {
        flags[i].transform.position = pos;
    }

    internal static void ADddPlayer(Transform player)
    {
        if (!IN.players.Contains(player))
        {
            print("not exist adding");
            IN.players.Add(player);
        }
        else print("/////////////////////////////////////////////////////////////////////  PLAYER EXIST!!!!!!!!!!!!!!!!!!!!!");
    }

    internal static void CapturaAt(Vector3 pos, Color col)
    {
        GameObject fo = IN.flags.Find((f) => { return f.transform.position == pos; });
        FlagPoint f = fo.transform.GetComponentInChildren<FlagPoint>();
        f.SetColor(col);
        f.captured = true;
    }
}
