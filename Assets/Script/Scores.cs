using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Scores : MonoBehaviour
{
    public int score;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        score = 0;
        ShowScore();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int value)
    {
        score += value;
        ShowScore();
    }
    private void ShowScore()
    {
        text.text = string.Format("{0:D4}",score);
    }
}
