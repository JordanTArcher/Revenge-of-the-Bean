using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class button_script : MonoBehaviour
{

    public bool isQuitButton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("gaming");

        if (isQuitButton)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("LevelOne");
        }
    }

}
