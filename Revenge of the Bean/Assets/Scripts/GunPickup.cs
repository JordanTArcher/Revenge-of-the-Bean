using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public GameObject gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameObject gun = GameObject.Find("grapple gun");
            other.gameObject.GetComponent<ThirdPersonMovement>().playGunPickupNoise = true;

            if (gun != null)
            {
                if(gun.activeSelf == false)
                {
                    gun.SetActive(true);
                }
            }

            Destroy(gameObject);
        }
    }
}
