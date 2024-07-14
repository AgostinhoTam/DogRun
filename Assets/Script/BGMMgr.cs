using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMgr : MonoBehaviour
{
    [SerializeField] AudioClip RevBGM;
    [SerializeField] AudioClip OriBGM;
    private AudioSource BGMPlayer;
    // Start is called before the first frame update
    void Start()
    {
        BGMPlayer = GetComponent<AudioSource>();
        BGMPlayer.clip = OriBGM;
        BGMPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeBGM()
    {
        float currentTime = BGMPlayer.time;
        float reverseTime = RevBGM.length - BGMPlayer.time;
        BGMPlayer.clip = OriBGM;
        BGMPlayer.time = reverseTime;
        BGMPlayer.Play();
    }
    public void ChangeRevBGM() 
    {
        float currentTime = BGMPlayer.time;
        float reverseTime = OriBGM.length - BGMPlayer.time;
        BGMPlayer.clip = RevBGM;
        BGMPlayer.time = reverseTime;
        BGMPlayer.Play();
    }
}
