using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBEAM : MonoBehaviour
{
    private LineRenderer Lazer;
    private Transform[] points;
    private bool hitter;
    [SerializeField] private LayerMask mask;
    
    // Start is called before the first frame update
    void Start()
    {
        Lazer = GetComponent<LineRenderer>();
        mask = LayerMask.GetMask("Ignore Raycast");
        hitter = false;
    }



    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, transform.forward, out hit, 1000);
        
        Lazer.SetPosition(0, transform.position);
        Lazer.SetPosition(1, hit.point);//hit.transform.position);
        if (!hitter && hit.collider.gameObject.CompareTag("Player"))
        {
            hitter = true;
            hit.collider.gameObject.GetComponent<ThirdPersonMovement>().currentHp -= 2.5f;
        }
        
        if(hitter && !hit.collider.gameObject.CompareTag("Player"))
        {
            hitter = false;
        }
    }


}
