﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll) {
        print("HIT");
        if (coll.gameObject.tag == "Sword") {
            print("sword");
            Global.S.killed++;
            Destroy(this.gameObject);
        } else {
            print(coll.gameObject.name);
        }
    }
}