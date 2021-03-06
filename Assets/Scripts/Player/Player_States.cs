﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

enum AnimState { idle, running, jumping, attack, death, falling};

public class State_Player_Normal_Movement : State
{
	Player player;
    private bool jump;
    private bool running;
    private float jump_timer;
    private float attack_timer;
    private bool attacked;


    public State_Player_Normal_Movement(Player player) {
        this.player = player;
    }

    public override void OnStart() {
        jump_timer = player.jumpTime;
        attack_timer = player.attack_speed;
        attacked = false;
        player.sword.SetActive(false);
    }

    public override void OnUpdate(float time_delta_fraction) {
        if (!player.dead) {

            //if (Input.GetButtonDown ("Jump") && player.grounded) {
            if (Input.GetButtonDown("Jump") && player.jumps_left > 0) {
                //Debug.Log("jump");
                jump = true;
                player.jumps_left--;
                player.grounded = false;
            }

            float h = Input.GetAxis("Horizontal");
            float jumpmove;

            // Seeing if the player is running or walking
            /*if (Input.GetButton("B Button") && player.grounded) {
                running = true;
                //Debug.Log ("running");
            } else {
                running = false;
                //Debug.Log ("walking");
            }*/

            if (player.grounded && running) {
                jumpmove = 2f;
            } else {
                jumpmove = 1f;
            }

            // Jumping
            if (jump) {
                // Add a vertical force to the player.
                if (jump_timer > 0) {
                    player.rb2d.AddForce(new Vector2(0, player.jumpForce), ForceMode2D.Impulse);
                    jump_timer -= Time.deltaTime;
                } else {
                    // Make sure the player can't jump again until the jump conditions from Update are satisfied.
                    jump = false;
                    jump_timer = player.jumpTime;
                }
            }



            // Left and right walk movement
            if (!attacked) {
                if (player.walled && !player.grounded) {
                    //can't run
                } else if (running == true) {
                    player.rb2d.velocity = (new Vector2(h * player.runForce * jumpmove, player.rb2d.velocity.y));
                } else {
                    player.rb2d.velocity = (new Vector2(h * player.moveForce * jumpmove, player.rb2d.velocity.y));
                }
            }
            Vector3 scale = player.rb2d.transform.localScale;
            if (h > 0)
                scale.x = -Mathf.Abs(scale.x);
            if (h < 0)
                scale.x = Mathf.Abs(scale.x);
            player.rb2d.transform.localScale = scale;

            if (Input.GetButtonDown("X Button")) {
                //player.sword.SetActive(true);
                attacked = true;
            }

            // Attacking
            if (attacked) {
                attack_timer -= Time.deltaTime;
                if (attack_timer <= .333) {
                    player.sword.SetActive(true);
                    if (player.grounded) {
                        player.rb2d.velocity = (new Vector2(0, 0));
                    }
                    //Debug.Log ("poop");
                }
                if (attack_timer <= 0) {
                    attack_timer = player.attack_speed;
                    player.sword.SetActive(false);
                    attacked = false;
                }
            }

            //Doors
            if (player.door != null && Input.GetButtonDown("B Button")) {
                CameraFollow.S.in_out();
                player.door.GetComponent<Door>().in_out();
                player.spawn = player.transform.position;
            }
        }



		// Animations -----------------------------------------------

		if (player.dead) {
			player.anim.SetInteger ("State", (int)AnimState.death);
			//player.rb2d.velocity = (new Vector2 (0, 0));
		} else if (attacked) {
			player.anim.SetInteger ("State", (int)AnimState.attack);
		} else if (player.rb2d.velocity.magnitude > .05f && player.grounded) {
			player.anim.SetInteger ("State", (int)AnimState.running);
		} else if (player.grounded == false) {
			player.anim.SetInteger ("State", (int)AnimState.jumping);
		} else {
			player.anim.SetInteger ("State", (int)AnimState.idle);
		}

    }

    public override void OnFinish() {
    }
}

public class State_Player_Paused : State {
    public State_Player_Paused() {
		
    }
}

public class State_Player_Falling : State {

    
    public State_Player_Falling() {
    }

    public override void OnUpdate(float time_delta_fraction) {
        Player.S.anim.SetInteger("State", (int)AnimState.falling);
    }

    public override void OnFinish() {
        MonoBehaviour.print("hmm");
    }
}
