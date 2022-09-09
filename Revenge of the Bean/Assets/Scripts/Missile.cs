using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Vector3 target;
    private bool launch = false;
    private Rigidbody rb;
    private bool translate = false;

   
    //public Vector3 target;
    // private Vector3 distance; 
   // private Vector3 me = transform.position;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
   void Update()
    {
        if (translate)
        {
            //rb.MovePosition(transform.position + (target - transform.position) * 3f * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target, 5.0f * Time.deltaTime);
        }
        if (launch)
        {
            // transform.position = Vector3.MoveTowards(transform.position, target, 3.0f * Time.deltaTime);
            // rb.position = transform.position;
           // ;
           rb.velocity = ((target-transform.position)/Time.deltaTime/60.0f);// * 3.0f * Time.deltaTime);
            if(Vector3.Distance(transform.position, target) < 3f)
            {
                target += (target - transform.position);
            }
        }
    }

    public void Launch(Vector3 x)
    {
        print("Missile Launch");
        // while(Vector3.Distance(transform.position, target) > 0.1f)
        // {
        target = x;
        transform.parent = null;
        //launch = true;
        translate = true;
        Update();
       // }
        //Starts at position and launches to target with flair
    }

    public void Fire()
    {
        
            translate = false;
            launch = true;
            rb.isKinematic = false;
        
    }
    void OnCollisionEnter(Collision other)
    {
        // if (!other.gameObject.CompareTag("Turret"))
        // {
        if (other.gameObject.CompareTag("Turret"))
        {
            return;
        }

       
            print("EXPLODE");
           //play explosion
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5.0f);
            foreach(var collidedObject in hitColliders)
            {
                if (collidedObject.gameObject.CompareTag("Player"))
                {
                    collidedObject.gameObject.GetComponent<ThirdPersonMovement>().currentHp -= (Vector3.Distance(transform.position, collidedObject.gameObject.transform.position)/2);
                }
            } 
            Destroy(gameObject);
       // }
    }

   
}
