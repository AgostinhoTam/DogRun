using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMgr : MonoBehaviour
{
    bool touch = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began )
            {
                touch = true;
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            touch = true;
        }
        if (touch)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
