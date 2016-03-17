﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {
    
	public GameObject player;
    public Vector3 init_offset;
    public float y_bound;
    Vector3 pos;

    public Camera outside;
    public Camera inside;

    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
        transform.position = player.transform.position + init_offset;
	}
	
    void FixedUpdate () {

        pos = transform.position;

        if (player.transform)
            pos.x = Vector3.Lerp(transform.position, player.transform.position, speed).x;

        pos.x = (pos.x < 1.5f ? 1.5f : pos.x);
        
		if (player.transform.position.y > y_bound + pos.y) {
			pos.y = player.transform.position.y - y_bound + 0;
		} else if (player.transform.position.y < -y_bound + pos.y) {
			pos.y = player.transform.position.y + y_bound + 0;
		} else {
			//pos.y = player.transform.position.y;	
		}
			

        pos.z = -10;
        
        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            outside.gameObject.SetActive(true);
            inside.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            outside.gameObject.SetActive(false);
            inside.gameObject.SetActive(true);
        }
    }
}
