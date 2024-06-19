using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Driver : MonoBehaviour
{
    public Transform player;
    bool repos;
    public float distance;
    public float elevation;
    private float startXrotate;
    private float startYelevate;
    private float deltaXrotate;
    private float deltaYelevate;
    private float storedElevate;
    public UnityEvent CameraOnPosition;

    internal void SetPlayer(Transform _player)
    {
        print("Camera SetPlayer");
        player = _player;
        repos = true;
    }




    void Start()
    {
        storedElevate = elevation;

    }

    void Update()
    {
        if (!player) return;
        if (repos)
        {
            Vector3 newPos = player.position + player.forward * distance + Vector3.up * elevation;
            transform.position = Vector3.Lerp(transform.position, newPos, 0.02f);
            if (Vector3.Distance(transform.position, newPos) <= 1f)
            {
                repos = false; Settings.CamDone = true;
                Tutorial.Show();
            }

        }
        else
            transform.position = player.position + player.forward * distance + Vector3.up * elevation;
        transform.LookAt(player.position);
        if (Input.mouseScrollDelta.y != 0)
        {
            Tutorial.Hide(1);
            distance -= Input.mouseScrollDelta.y;
            elevation -= Input.mouseScrollDelta.y;
        }

        if (Input.GetMouseButtonDown(1))
        {
            startXrotate = Input.mousePosition.x;
            startYelevate = Input.mousePosition.y;
        }

        if (Input.GetMouseButton(1) && Settings.CamDone)
        {
            Tutorial.Hide(0);
            deltaXrotate = Input.mousePosition.x - startXrotate;
            deltaYelevate = Input.mousePosition.y - startYelevate;
            if (elevation + deltaYelevate / 1000 > 0 && elevation + deltaYelevate / 1000 < 10)
                elevation -= deltaYelevate / 1000;
            player.GetComponent<Mover>().rotateY += deltaXrotate / 200;
        }
        else
        {
            elevation = Mathf.Lerp(elevation, storedElevate, 0.01f);

        }

    }
}