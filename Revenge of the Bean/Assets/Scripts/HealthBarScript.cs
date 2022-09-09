using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    private float imgMaxHp;
    private float imgCurrentHp;

    public GameObject player;

    private Image hpBarImg;

    // Start is called before the first frame update
    void Start()
    {
        hpBarImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ThirdPersonMovement movScript = player.GetComponent<ThirdPersonMovement>();

        imgMaxHp = movScript.maxHp;
        imgCurrentHp = movScript.currentHp;

        hpBarImg.fillAmount = imgCurrentHp / imgMaxHp;
    }
}
