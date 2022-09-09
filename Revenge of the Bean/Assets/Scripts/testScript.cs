using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2.0f * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = (target.transform.position- transform.position)/ Time.deltaTime/60f;//MovePosition(transform.position + (target.transform.position * Time.deltaTime * 0.5f));
    }
}
