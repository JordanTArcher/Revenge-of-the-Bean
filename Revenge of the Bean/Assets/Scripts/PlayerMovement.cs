using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public Transform spawnPoint; 
    public float lookSpeed = 10f;
    public Vector2 rotation = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        transform.Translate(movement * Time.deltaTime * moveSpeed);

        rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        transform.eulerAngles = rotation;


    }
}
