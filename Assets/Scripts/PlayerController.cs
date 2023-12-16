using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Animation variables
    Animator anim;
    public bool moving = false;
    public bool jumping = false;
    public bool attacking = false;
    
    //Movement Variables
    Rigidbody2D rb; //create reference for rigidbody bc jump requires physics
    public float jumpForce; //the force that will be added to the vertical component of player's velocity
    public float speed;

    //Ground Check Variables
    public LayerMask groundLayer;//layer information
    public Transform groundCheck;// player position info
    public bool isGrounded;

    SpriteRenderer sprite;

    //Audio BGM onStartup

    public GameObject backgroundMusicObject;

    public GameObject attackPoint;
    public float radius;
    public LayerMask blocks;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        AudioSource audioSource = backgroundMusicObject.GetComponent<AudioSource>();

        // Check if AudioSource exists and play the music
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .5f, groundLayer);
        
        Vector3 newPosition = transform.position;
        Vector3 newScale = transform.localScale;
        float currentScale = Mathf.Abs(transform.localScale.x);

        if(Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= speed;
            newScale.x = -currentScale;
            moving = true;
        }

        if(Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += speed;
            newScale.x = currentScale;
            moving = true;
        }

        if((Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("isJumping", true);
        }

        if(Input.GetKeyUp("a") || Input.GetKeyUp("d") || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moving = false;
        }
        
        if(Input.GetKeyDown("f"))
        {
            anim.SetBool("isAttacking", true);
        }




        anim.SetBool("isMoving", moving);
        transform.position = newPosition; 
        transform.localScale = newScale;
    }


    public void attack(){
        Collider2D[] block = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, blocks);

        foreach (Collider2D blockGameobject in block){
            Debug.Log("Hit block.");
            blockGameobject.GetComponent<blockhealth>().health -= damage;
        }
    }

    public void endAttack(){
        anim.SetBool("isAttacking", false);
    }

    public void endJump(){
        anim.SetBool("isJumping", false);
    }

  
     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("end"))
        {
            Debug.Log("hit");
            SceneManager.LoadScene(2); //access SceneManager class for LoadScene function
        }
    }
}
