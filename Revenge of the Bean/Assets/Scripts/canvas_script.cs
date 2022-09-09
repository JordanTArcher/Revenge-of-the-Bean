using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class canvas_script : MonoBehaviour
{
    public GameObject player;

    public bool playerIsAlive;
    public Image hpBarBg;
    public Image hpBarMain;
    public Image crosshair;
    public TextMeshProUGUI deathMsg;

    // Start is called before the first frame update
    void Start()
    {
        playerIsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        ThirdPersonMovement movScript = player.GetComponent<ThirdPersonMovement>();

        if (movScript.currentHp <= 0)
        {
            playerIsAlive = false;
            deathMsg.text = "You are captured!\nR to restart";
        }


        hpBarBg.enabled = playerIsAlive;
        hpBarMain.enabled = playerIsAlive;
        crosshair.enabled = playerIsAlive;

    }
}
