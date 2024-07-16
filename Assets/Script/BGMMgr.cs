using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMgr : MonoBehaviour
{
    [SerializeField] AudioClip RevBGM;
    [SerializeField] AudioClip OriBGM;
    private AudioSource BGMPlayer;
    private bool isChanging = false;
    // Start is called before the first frame update
    void Start()
    {
        BGMPlayer = GetComponent<AudioSource>();
        PreloadAudioClips();
        BGMPlayer.clip = OriBGM;
        BGMPlayer.Play();
        
    }

    private void PreloadAudioClips()
    {
        if(RevBGM.loadState != AudioDataLoadState.Loaded)
        {
            RevBGM.LoadAudioData();
        }
        if (OriBGM.loadState != AudioDataLoadState.Loaded)
        {
            OriBGM.LoadAudioData();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeBGM()
    {
        if(isChanging) return;
        StartCoroutine(ChangeBGMCoroutine(OriBGM,RevBGM));
    }
    public void ChangeRevBGM()
    {
        if (isChanging) return;
        StartCoroutine(ChangeBGMCoroutine(RevBGM, OriBGM));
    }
    private IEnumerator ChangeBGMCoroutine(AudioClip newClip, AudioClip oldClip)
    {
        isChanging = true;

        float currentTime = BGMPlayer.time;
        float reversetime = oldClip.length - currentTime;
        Debug.Log("BGMReady");
        BGMPlayer.clip = newClip;
        BGMPlayer.time = reversetime;

        yield return null;

        BGMPlayer.Play() ;
        isChanging = false;
    }

}
