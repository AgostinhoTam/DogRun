using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScores : MonoBehaviour
{
    private Text scoreText;
    private int scores;
    private bool touch;
    // Start is called before the first frame update
    void Start()
    {
        scores = PlayerPrefs.GetInt("Scores");
        scoreText = GetComponent<Text>();
        scoreText.text = scores.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touch = true;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            touch = true;
        }
        if (touch)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
