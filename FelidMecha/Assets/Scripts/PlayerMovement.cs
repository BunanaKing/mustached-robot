using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float flySpeed = 15f;
	public float forwardSpeed = 7f;
	public float minHeight = -2.7f;
	public float maxHeight = 5f;

	bool dead = false;
	bool didJump = false;
	public bool godMode = false;
	public int state = 0; //0 idle; 1 run; 2 jump; 3 fall

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
		else if((Input.GetMouseButtonUp(0) && !Input.GetKeyDown(KeyCode.Space))
		        || (!Input.GetMouseButtonDown(0) && Input.GetKeyUp(KeyCode.Space))){
			didJump = false;
		}



	}

	//Do physics engine updates here
	void FixedUpdate() {
		
		if(dead)
			return;

		//rigidbody2D.AddForce(Vector2.right * forwardSpeed);

		//rigidbody2D.constantForce = ()
		Vector2 vel = rigidbody2D.velocity;
		vel.x = forwardSpeed;
		rigidbody2D.velocity = vel;

		if(didJump){
			rigidbody2D.AddForce(Vector2.up * flySpeed);
			if(state != 2){
				state = 2;
				animator.SetTrigger("DoFly");
			}
		}
		else if(rigidbody2D.velocity.y <= 0){
			if(rigidbody2D.velocity.y < 0 && transform.position.y > minHeight){
				//Esta cayendo
				if(state != 3){
					state = 3;
					animator.SetTrigger("DoFall");
				}
			}
			else if(rigidbody2D.velocity.x > 0){
				//Esta corriendo
				if(state != 1){
					state = 1;
					animator.SetTrigger("DoRun");
				}
			}
		}	

		//Correct position TOP
		if(transform.position.y > maxHeight){
			Vector2 posCorrect = transform.position;
			posCorrect.y = maxHeight;
			transform.position = posCorrect;
			Vector2 velCorrect = rigidbody2D.velocity;
			velCorrect.y = 0;
			rigidbody2D.velocity = velCorrect;
		}
		
		//Correct position BOTTOM
		if(transform.position.y < minHeight){
			Vector2 posCorrect = transform.position;
			posCorrect.y = minHeight;
			transform.position = posCorrect;
			Vector2 velCorrect = rigidbody2D.velocity;
			velCorrect.y = 0;
			rigidbody2D.velocity = velCorrect;
		}

	}

	void OnCollisionEnter2D(Collision2D collision){

		if(godMode)
			return;

	}
}
