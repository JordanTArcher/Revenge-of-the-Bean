using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : MonoBehaviour 
{ 
    public GameObject[] launcher;
    public GameObject[] relod;
    public Vector3 target;
    private Vector3 LastKnownLocation;
    private int ammo;
    private float timer;
    private bool reloadable = false;

    // Start is called before the first frame update
    void Start()
    {
        ammo = 0;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit);
        Debug.DrawRay(transform.position, transform.forward * 20, Color.green);
        if (hit.collider.gameObject.CompareTag("Player"))
        {
           // print(timer);
            target = hit.point;
            Launch();
            timer += Time.deltaTime;
        }
        if (reloadable)
        {
            Reload();
            timer += Time.deltaTime;
        }
    }


    public void Launch()
    {
        if(timer > 2.0f && ammo < 4){
            print("launch: " + ammo);
            relod[ammo] = Instantiate(launcher[ammo], launcher[ammo].transform.position, launcher[ammo].transform.rotation, launcher[ammo].transform.parent);
            relod[ammo].gameObject.SetActive(false);
            
            launcher[ammo].GetComponent<Missile>().Launch(target); //update to make missile launch.
            ammo++;
            timer -= 0.5f; //to make shots per second
        }
        
        if(ammo >= 4)
        {
            reloadable = true;
        }
    }

    public void Reload()
    {
        if(timer > 4f && (ammo - 4) < 4)
        {
            print("Reloading: " + (ammo - 4));
            launcher[(ammo - 4)] = Instantiate(relod[(ammo - 4)], relod[(ammo -4)].transform.position, relod[(ammo-4)].transform.rotation, relod[(ammo-4)].transform.parent);
            Destroy(relod[(ammo - 4)]);
            launcher[(ammo - 4)].SetActive(true);
            timer -= 1.0f;
            ammo++;
        }

        if((ammo-4) >= 4)
        {
            print("Reloading complete");
            ammo = 0;
            timer = 0;
            reloadable = false;
        }
        print("RELOADING");
    }
}
