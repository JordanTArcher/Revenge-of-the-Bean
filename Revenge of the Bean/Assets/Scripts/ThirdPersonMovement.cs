using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public bool canDoubleJump = true;
    public bool jumpOutOfGrapple = false;
    public bool playHealNoise = false;
    public bool playGunPickupNoise = false;

    public float maxHp = 10f;
    public float currentHp = 5f;

    public CharacterController cc;

    private AudioSource source;
    public AudioClip jumpNoise;
    public AudioClip doubleJumpNoise;
    public AudioClip damageNoise;
    public AudioClip healNoise;
    public AudioClip gunPickupNoise;
    public AudioClip song1;

    private float currentInvFrames = 0;
    private float invTime = 1; //in seconds, the time the player is invincible after getting hit

    // Animator reference for controlling walking animation
    public Animation anim;
    void Start()
    {

        //makes the cursor go away
        Cursor.lockState = CursorLockMode.Locked;

        //start at max hp :)
        //currentHp = maxHp;

        // initialize animator
        anim = GetComponent<Animation>();
        source = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();

        source.PlayOneShot(song1);
    }

    // Update is called once per frame
    void Update()
    {

        if (playHealNoise)
        {
            playHealNoise = false;
            source.PlayOneShot(healNoise);
        }
        if (playGunPickupNoise)
        {
            playGunPickupNoise = false;
            source.PlayOneShot(gunPickupNoise);
        }

        if (Input.GetKeyDown(KeyCode.R) && currentHp <= 0)
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        if (currentInvFrames > 0)
        {
            currentInvFrames -= Time.deltaTime;
        }
        
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            canDoubleJump = true;
            velocity.y = -2f;

        }

        if (Input.GetButtonDown("Jump") && cc.enabled && currentHp > 0)
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                source.PlayOneShot(jumpNoise);
            }
            else if (canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                canDoubleJump = false;
                source.PlayOneShot(doubleJumpNoise);
            }
        }
        /*
        if (jumpOutOfGrapple)
        {

        }*/

        //gravity

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (currentHp <= 0) //ignore inputs if ur ded
        {
            horizontal = 0;
            vertical = 0;
        }
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //rotation shenanigans
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            //actual movement
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        //anim stuff, edited by charlie
        
        if(isGrounded && vertical != 0)
        {
            
            anim.Play("PlayerWalkAnim");
        }
         //if(horizontal != 0 || vertical != 0){
            //anim.SetBool("Grounded", isGrounded);
            //anim.SetFloat("Speed", GetComponent<RigidBody>().velocity.sqrMagnitude);
         //}
        
    }
    // Added by charlie to detect enemy melee hits
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && currentInvFrames <= 0)
        {
            currentHp -= 1;
            source.PlayOneShot(damageNoise);
            currentInvFrames = invTime;
        }
     
    }


}