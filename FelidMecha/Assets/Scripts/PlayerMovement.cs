using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float forwardSpeed = 7f;				//Velocidad con que avanza el personaje
	public float minHeight = -2.7f;				//Altura del piso
	public float maxHeight = 5f;				//Altura del techo

	bool didJump = false;
	public float jumpInitialForce = 57f;		//Esta fuerza decrece segun el tiempo que esta en el aire el personaje
	public float jumpMaxTimer = 0.5f;			//Maximo tiempo que se puede presionar el salto
	float jumpAirTime = 0f;						//Tiempo durante el cual se presiona el salto

	bool dead = false;							//Bandera que indica si el personaje murio
	public bool godMode = false;				//Bandera para no colisionar ni morir
	public int state = 0; 						//Estado para las animaciones
												//0 idle; 1 run; 2 jump; 3 fall

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = transform.GetComponentInChildren<Animator>();
		
	}

	private bool IsGrounded(){
		return (transform.position.y <= this.minHeight);
	}
	
	// Do Graphic & Input updates here
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) //Button 0 se supone que es el click izquierdo, y el tap en mobile
		{
			if(this.IsGrounded()){
				if(!didJump){
					didJump = true;
					jumpAirTime = 0f;
				}
			}
			else
			{
				//Wallride? 
			}
		}
		else if((Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.Space))
		        || (!Input.GetMouseButton(0) && Input.GetKeyUp(KeyCode.Space))){
			didJump = false;
			jumpAirTime = 0f;
		}

		if(Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)){
			//Esta apretada la tecla o el touch
			if(didJump){
				jumpAirTime += Time.deltaTime;
				if(jumpAirTime >= jumpMaxTimer){
					didJump = false;
					jumpAirTime = 0f;
				}
				//Debug.Log ("Jump Timer: " + jumpAirTime);
			}
		}
	}

	//Do physics engine updates here
	void FixedUpdate() {
		
		if(dead)
			return;
		
		Vector2 vel = rigidbody2D.velocity;
		vel.x = forwardSpeed;
		rigidbody2D.velocity = vel;

		if(didJump){
			float upForce = jumpInitialForce * (1-(jumpAirTime/jumpMaxTimer));
			//Debug.Log("UpForce: " + upForce);

			rigidbody2D.AddForce(Vector2.up * upForce);
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

		//Corregir la posicion TOP
		if(transform.position.y > maxHeight){
			Vector2 posCorrect = transform.position;
			posCorrect.y = maxHeight;
			transform.position = posCorrect;
			Vector2 velCorrect = rigidbody2D.velocity;
			velCorrect.y = 0;
			rigidbody2D.velocity = velCorrect;
		}
		
		//Corregir la posicion BOTTOM
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

		//Debug.Log("Colision con: " + collision.gameObject.name);

		if(godMode)
			return;
		if(dead)
			return;


	}

	void OnTriggerEnter2D(Collider2D collider){
		//Debug.Log("Tigger enter with: " + collider.gameObject.name);
		
		if(collider.gameObject.layer == LayerMask.NameToLayer(Definitions.layer_obstaculos))
		{
			Debug.Log("Se detecta colision con un obstaculo: " + collider.gameObject.name);
			if(godMode)
				return;
			if(dead)
				return;

			this.Die();
		}

	}

	void Die(){

		this.dead = true;
		animator.SetTrigger("DoDie");
		//Cambiamos la layer para que colisione con el piso
		this.gameObject.layer = LayerMask.NameToLayer(Definitions.layer_deadCharacter);
		BoxCollider2D boxCollider = this.GetComponent<BoxCollider2D>();
		CircleCollider2D cirCollider = this.GetComponent<CircleCollider2D>();
		if(boxCollider != null && cirCollider != null){
			//boxCollider.isTrigger = true;
			//cirCollider.isTrigger = false;
		}

	}
}
