using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetection : MonoBehaviour
{
    public GameObject DTC;
    private bool inside;
    private GameObject play;
    private LineRenderer targetting;
    
    Vector3 relativePos = Vector3.zero;
    Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
       // DTC = GameObject.Find("DetectionCube");
        play = GameObject.Find("Player");
        targetting = DTC.GetComponent<LineRenderer>();
        targetting.startWidth = 0.1f;
        targetting.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(DTC.transform.position, DTC.transform.forward, out hit);
        targetting.SetPosition(0, DTC.transform.position);
        targetting.SetPosition(1, hit.point);
        //Requires player be in FoV
        if (inside)
        {
           
            //bool x prevents clashing raycasting calls.
            bool x = Engage();
            
            //IF hits player:  
            //Stop searching, lock on to player and persistent facing the player.
            //ELSE IF hits anything else
            //Continue searching
            if (x)
            {
               
                DTC.GetComponent<Animator>().enabled = false;
               
                if (relativePos != play.transform.position)
                {
                    float verMove = Input.GetAxis("Vertical");
                    float horMove = Input.GetAxis("Horizontal");

                    Vector3 movement = new Vector3(horMove, 0f, verMove);
                    relativePos = play.transform.position;
                    rotation = Quaternion.LookRotation(relativePos - DTC.transform.position +movement.normalized);
                    DTC.transform.rotation = rotation;
                   
                }
                if (DTC.transform.rotation != rotation)
                {
                    DTC.transform.rotation = Quaternion.RotateTowards(DTC.transform.rotation, rotation, 20.0f * Time.deltaTime); //higher float is more accurate. Lower float is cleaner cone.
                }

            } else if (!x) {

                DTC.GetComponent<Animator>().enabled = true;
            }
        }
     
     

    }

    //Determines whether the player is visible
    private bool Engage()
    {
        //Movement calculations makes raycast more accurate
        float verMove = Input.GetAxis("Vertical");
        float horMove = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horMove, 0f, verMove);


            //Shoots Ray out from the turret head.
             RaycastHit hit;
             Vector3 toTarget = play.transform.position - DTC.transform.position;
             Vector3 test = toTarget + movement;
             Physics.Raycast(DTC.transform.position, toTarget.normalized, out hit);
        //Debug.DrawRay(DTC.transform.position, test.normalized * 50f, Color.red);
        
        //If it hits player returns true
        return hit.collider.gameObject.CompareTag("Player");
       
       

    }

    //When Player enters FoV stop searching and snap to player.
    void OnTriggerEnter(Collider other)
    {
           
        if (other.CompareTag("Player"))
        {
            print("TEST");
            inside = true;
            
            DTC.GetComponent<Animator>().enabled = false;
           
           


            relativePos = play.transform.position;
            rotation = Quaternion.LookRotation(relativePos - DTC.transform.position);
            DTC.transform.rotation = rotation;

            DTC.transform.rotation = Quaternion.RotateTowards(DTC.transform.rotation, rotation, 10.0f * Time.deltaTime);



        }
    }

    //When Player leaves FoV continue Searching
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
            DTC.GetComponent<Animator>().enabled = true;
           // DTC.GetComponent<Animator>().SetBool("Engaged", false);
        }
    }
}
