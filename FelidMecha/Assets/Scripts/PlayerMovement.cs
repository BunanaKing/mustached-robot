using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Velocidad con que avanza el personaje
    public float forwardSpeed = 7f;
    // Altura del piso	
    public float minHeight = -2.7f;
    // Altura del techo	
    public float maxHeight = 5f;

    bool didJump = false;
    // Esta fuerza decrece segun el tiempo que esta en el aire el personaje
    public float jumpInitialForce = 57f;
    // Maximo tiempo que se puede presionar el salto
    public float jumpMaxTimer = 0.5f;
    // Tiempo durante el cual se presiona el salto
    float jumpAirTime = 0f;

    // Bandera que indica si el personaje murio
    bool dead = false;
    // Bandera para no colisionar ni morir					
    public bool godMode = false;
    // Estado para las animaciones: 0 idle; 1 run; 2 jump; 3 fall
    public int state = 0;

    Animator animator;

    void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    private bool IsGrounded()
    {
        return (transform.position.y <= minHeight);
    }

    // Do Graphic & Input updates here
    void Update()
    {
        // Button 0 es el click izquierdo, y el tap en mobile
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (IsGrounded())
            {
                if (!didJump)
                {
                    didJump = true;
                    jumpAirTime = 0f;
                }
            }
            else
            {
                // Wallride? 
            }
        }
        else if ((Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.Space))
              || (!Input.GetMouseButton(0) && Input.GetKeyUp(KeyCode.Space)))
        {
            didJump = false;
            jumpAirTime = 0f;
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            // Esta apretada la tecla o el touch
            if (didJump)
            {
                jumpAirTime += Time.deltaTime;
                if (jumpAirTime >= jumpMaxTimer)
                {
                    didJump = false;
                    jumpAirTime = 0f;
                }                
            }
        }
    }

    // Do physics engine updates here
    void FixedUpdate()
    {
        if (dead)
            return;      

        Vector2 vel = rigidbody2D.velocity;
        vel.x = forwardSpeed;
        rigidbody2D.velocity = vel;

        if (didJump)
        {
            float upForce = jumpInitialForce * (1 - (jumpAirTime / jumpMaxTimer));

            rigidbody2D.AddForce(Vector2.up * upForce);
            if (state != 2)
            {
                state = 2;
                animator.SetTrigger("DoFly");
            }
        }
        else if (rigidbody2D.velocity.y <= 0)
        {
            if (rigidbody2D.velocity.y < 0 && transform.position.y > minHeight)
            {
                // Esta cayendo
                if (state != 3)
                {
                    state = 3;
                    animator.SetTrigger("DoFall");
                }
            }
            else if (rigidbody2D.velocity.x > 0)
            {
                // Esta corriendo
                if (state != 1)
                {
                    state = 1;
                    animator.SetTrigger("DoRun");
                }
            }
        }

        // Corregir la posicion TOP
        if (transform.position.y > maxHeight)
        {
            Vector2 posCorrect = transform.position;
            posCorrect.y = maxHeight;
            transform.position = posCorrect;
            Vector2 velCorrect = rigidbody2D.velocity;
            velCorrect.y = 0;
            rigidbody2D.velocity = velCorrect;
        }

        // Corregir la posicion BOTTOM
        if (transform.position.y < minHeight)
        {
            Vector2 posCorrect = transform.position;
            posCorrect.y = minHeight;
            transform.position = posCorrect;
            Vector2 velCorrect = rigidbody2D.velocity;
            velCorrect.y = 0;
            rigidbody2D.velocity = velCorrect;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (godMode || dead)
            return;        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(Definitions.obstacles_layer))
        {
            if (godMode || dead)            
                return;
            
            Die();
        }
    }

    void Die()
    {
        dead = true;
        animator.SetTrigger("DoDie");

        // Cambiamos la layer para que colisione con el piso
        gameObject.layer = LayerMask.NameToLayer(Definitions.deadCharacter_layer);        
    }
}
