using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score; 
    public Text winText;
    public Text lives;
    private int scoreValue = 0;
    private int numLives = 3;

    private bool isJump;


    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public AudioSource musicSource;
    public AudioClip musicClip;
    Animator anim;

    private bool facingRight = true;

    private int teleNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        isJump = false;
        numLives = 3;
        score.text = scoreValue.ToString();
        lives.text = "Lives: " + numLives.ToString();
        winText.text ="";
        rd2d = GetComponent<Rigidbody2D>();
         anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (Input.GetAxis("Horizontal") != 0 )
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetAxis("Horizontal") == 0 )
        {
            anim.SetInteger("State", 0);
        }

        if(verMovement > 0 && isOnGround == false)
        {
             anim.SetInteger("State", 2);  
        }


        if(facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
          if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        print (isJump);
   }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue++;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            setText();
        }
         if(collision.collider.tag == "UFO")
        {
            numLives--;
            lives.text = "Lives: " + numLives.ToString();
            Destroy(collision.collider.gameObject);
            setText();
        }
         if (scoreValue == 4 && teleNum == 1)
        {
            teleNum = 0; 
            setText();
            transform.position = new Vector2(55.0f, 7.7f);
        
        }
        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3.3f),ForceMode2D.Impulse);
                isJump = true;
            }
            isJump = false;
        }
    }
    void setText()
    {
        if(scoreValue == 10 && numLives > 0)
        {
            winText.text = "You Win! Hooray! \n Game by Emilie Montgomery";
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        if(numLives <= 0)
        {
            winText.text = "Too Bad! You Lose! \n Game by Emilie Montgomery";
            Destroy(this);
            musicSource.Stop();
        }
        if(scoreValue == 4)
        {
            numLives = 3;
            lives.text = "Lives: " + numLives.ToString();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    
}
