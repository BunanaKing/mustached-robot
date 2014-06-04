using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float flySpeed = 5f;
	public float forwardSpeed = 1f;

	bool dead = false;
	bool didJump = false;
	public bool godMode = false;

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = transform.GetComponentInChildren<Animator>();
	}
	
	// Do Graphic & Input updates here
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) //Button 0 se supone que es el click izquierdo, y el tap en mobile
		{
			didJump = true;
		}
		else {
			//didJump = false;
		}
	}

	//Do physics engine updates here
	void FixedUpdate() {

		if(dead)
			return;

		Vector2 vel = new Vector2();
		vel.x = forwardSpeed;
		rigidbody2D.velocity = vel;

		if(didJump){
			didJump = false;
			//vel.y = 0;
			rigidbody2D.velocity = vel;
			rigidbody2D.AddForce(Vector2.up * flySpeed);

		}



		if(animator != null){
			if(rigidbody2D.velocity.y > 0)
				animator.SetTrigger("DoFly");
			else if(rigidbody2D.velocity.y < 0)
				animator.SetTrigger("DoFall");		
		}

	}

	void OnCollisionEnter2D(Collision2D collision){

		if(godMode)
			return;

	}
}
