using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Field : MonoBehaviour
{
    public GameObject limiter;
    public GameObject field;
    private Mesh fieldMesh;
    public float minx = 0, minz = 0, maxx = 0, maxz = 0;
    public static Field IN;
    public float fieldscaleFactor;
    private void Awake()
    {
        IN = this;
    }


    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLimiters()
    {
            fieldMesh = field.GetComponent<MeshFilter>().mesh;


            foreach (Vector3 v in fieldMesh.vertices)
            {
                if (v.x > maxx) maxx = v.x;
                if (v.z > maxz) maxz = v.z;
                if (v.x < minx) minx = v.x;
                if (v.z < minz) minz = v.z;
            }

            fieldscaleFactor = field.transform.lossyScale.x;
            float l = limiter.transform.lossyScale.y / 2;
            GameObject limUR = Instantiate(limiter, field.transform);
            limUR.transform.localPosition = field.transform.TransformPoint(new Vector3(maxx, l, maxz) / fieldscaleFactor);
            GameObject limUL = Instantiate(limiter, field.transform);
            limUL.transform.localPosition = field.transform.TransformPoint(new Vector3(minx, l, maxz) / fieldscaleFactor);
            GameObject limdL = Instantiate(limiter, field.transform);
            limdL.transform.localPosition = field.transform.TransformPoint(new Vector3(minx, l, minz) / fieldscaleFactor);
            GameObject limdR = Instantiate(limiter, field.transform);
            limdR.transform.localPosition = field.transform.TransformPoint(new Vector3(maxx, l, minz) / fieldscaleFactor);
        }

    internal static Vector3 RandomPosition(float y)
    {
        float x = Random.Range(Field.IN.minx, Field.IN.maxx);
        float z = Random.Range(Field.IN.minz, Field.IN.maxz);
        return new Vector3(x, y, z);
    }

    internal static bool CheckBounds(Vector3 position, Vector3 move)
    {
        Vector3 v = position + move;
        v = IN.field.transform.InverseTransformPoint(v);
        if (v.x < IN.minx || v.x > IN.maxx || v.z < IN.minz || v.z > IN.maxz) return false;
        else return true;
    }
}

