using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Light muzzleFlash;
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask grappleable;

    private float maxDistance = 100;
    public Transform gunTip, cam, player;
    private SpringJoint joint;

    //for gun looking at grapple point
    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    private AudioSource source;
    public AudioClip grappleNoise;
    public AudioClip shootNoise;

    private float gunCooldown = 0f; //current cooldown, dont change this
    private float gunShotSpeed = 0.75f; //time between when player can shoot in seconds, changeable

    private float gunFlashCooldown = 0f;
    private float gunFlashTime = 0.2f;

    private void Awake()
    {
        source = player.GetComponent<AudioSource>();
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {

        ThirdPersonMovement movScript = player.GetComponent<ThirdPersonMovement>();

        //timers
        if(gunCooldown > 0)
        {
            gunCooldown -= Time.deltaTime;
        }
        if(gunFlashCooldown > 0)
        {
            muzzleFlash.GetComponent<Light>().enabled = true;
            gunFlashCooldown -= Time.deltaTime;
        }
        else
        {
            muzzleFlash.GetComponent<Light>().enabled = false;
        }



        //inputs
        if (movScript.currentHp > 0)
        {
            if (Input.GetMouseButtonDown(0) && gunCooldown <= 0)
            {
                gunCooldown = gunShotSpeed;
                gunFlashCooldown = gunFlashTime;
                source.PlayOneShot(shootNoise);
                RaycastHit hit;
                Physics.Raycast(cam.position, cam.forward, out hit, maxDistance);
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<Patrol1>().enemyHealth -= 2.5f;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartGrapple();
            }
        }
        if ((Input.GetMouseButtonUp(1) || Input.GetButtonDown("Jump") || movScript.currentHp <= 0) && joint)
        {
            EndGrapple();
        }
    }

    //called after update
    private void LateUpdate()
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (!joint)
        { //is not grappling

            //point back to normal angle
            desiredRotation = transform.parent.rotation;

            //enable player movement
            cc.enabled = true;
        }
        else
        { //is grappling

            desiredRotation = Quaternion.LookRotation(grapplePoint - transform.position);

            //disable player movement - its not physics based so it f***s up everything if its enabled
            cc.enabled = false;
            DrawRope();

            /*
            if (Input.GetButtonDown("Jump"))
            {
                EndGrapple();
                ThirdPersonMovement movScript = player.GetComponent<ThirdPersonMovement>();
                movScript.jumpOutOfGrapple = true;
            }*/
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);

    }

    void StartGrapple()
    {
        RaycastHit hit;
        ThirdPersonMovement movScript = player.GetComponent<ThirdPersonMovement>();

        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, grappleable) && movScript.currentHp > 0)
        {
            source.PlayOneShot(grappleNoise);

            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //the distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;

            movScript.canDoubleJump = true;
        }
    }

    void EndGrapple()
    {
        ThirdPersonMovement movScript = player.GetComponent<ThirdPersonMovement>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        movScript.velocity.y = rb.velocity.y;

        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        
        //if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

}
