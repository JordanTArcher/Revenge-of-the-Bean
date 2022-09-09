using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private float max;
    private float current;

    //private ThirdPersonMovement scripty;

    //When the Player touches the object they will get 20% of their max health back.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //scripty = other.gameObject.GetComponent<>

            current = other.gameObject.GetComponent<ThirdPersonMovement>().currentHp;   //Replace playerScript with the script that controls your player's statuses
            max = other.gameObject.GetComponent<ThirdPersonMovement>().maxHp; //replace currentHealth and maxHealth with the respective health values in the playerScript.

            current += (0.5f * max);

            //if the new health value exceeds the max Health then it reduces down to the max health amount.
            if(current > max)
            {
                current = max;
            }

            other.gameObject.GetComponent<ThirdPersonMovement>().currentHp = current;//replace here as well.
            other.gameObject.GetComponent<ThirdPersonMovement>().playHealNoise = true;
            Destroy(gameObject);
        }
    }
}
