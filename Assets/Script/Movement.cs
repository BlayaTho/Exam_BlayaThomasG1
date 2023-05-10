using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    float horizontal_value;
    Vector2 ref_velocity = Vector2.zero;



    [SerializeField] float moveSpeed_horizontal = 400.0f;
    [SerializeField] float jumpForce = 23f;
    [SerializeField] bool can_jump = false;
    [SerializeField] float maxFallSpeed;
    bool duringJump;
    bool holdingJumpButton;
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //recup le component automatiquement
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        #region Capture Donnees
        horizontal_value = Input.GetAxis("Horizontal");

        if (horizontal_value > 0) sr.flipX = false;
        else if (horizontal_value < 0) sr.flipX = true; //flip le sprite selon la direction
        #endregion

        #region Jump if
        if (Input.GetButton("Jump"))
        {
            holdingJumpButton = true;
        }
        if (Input.GetButtonUp("Jump"))                  //verifier si on appuie ou lache le bouton
        {
            holdingJumpButton = false;
        }


        if (Input.GetButtonDown("Jump") && can_jump)   //jump update a le transmettre au fixedUpdate
        {
            duringJump = true;
        }
        #endregion
    }
    void FixedUpdate()
    {
        #region Movement
        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, rb.velocity.y); //multiplier la velocité par la puissance de la direction voulu et la vitesse défini
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f); // Smooth des deplacements, acceleration et deceleration
        #endregion

        #region JumpFixed
        if (duringJump && can_jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Ajout velocité du player en y par la jumpforce
        }

        // Higher gravity when falling of a jump
        if (!holdingJumpButton && duringJump)
        {
            rb.AddForce(new Vector2(0, -50), ForceMode2D.Force); //pousser le joueur vers le bas quand il lache le bouton du jump (jump à hauteur adapatative)
        }
        #endregion

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) //si on touche le sol
        {
            duringJump = false;
            can_jump = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) can_jump = false;
    }
}
