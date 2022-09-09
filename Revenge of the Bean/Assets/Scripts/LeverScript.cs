using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverScript : MonoBehaviour
{
    public GameObject platform;
    public bool leverPulled = false;
    public float timer;
    private Animation anim;
    [SerializeField] private Image customImage;
    [SerializeField] private Text customText;

    void OnTriggerStay(Collider other)
    {
        customImage.enabled = true;
        customText.enabled = true;
        if (other.gameObject.CompareTag("Player"))
        {

            if(Input.GetKeyDown(KeyCode.E) && !leverPulled)
            {
                leverPulled = true;
                Debug.Log("Lever Pulled");
                anim.Play("LeverAnim");
                customImage.enabled = false;
                customText.enabled = false;
                //anim.SetTrigger("LeverPull");
                //StartCoroutine(MovePlat());
            }
        }

        //IEnumerator MovePlat()
        //{
        //    yield return new WaitForSeconds(timer);
        //   leverPulled = false;
        //    Debug.Log("PlatMoved");
        //}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            customImage.enabled = false;
            customText.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        customImage.enabled = false;
        customText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
