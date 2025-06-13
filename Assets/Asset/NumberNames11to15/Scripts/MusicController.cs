using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MusicController : MonoBehaviour
{

    [Header("AUDIO---------------------------------------------------------")]
    [SerializeField] private AudioSource AS_Music;

    [SerializeField] private Image IMG_Music;

    [SerializeField] private Sprite SPR_Music_On;
    [SerializeField] private Sprite SPR_Music_Off;


    private bool isMusicPlaying;


    void Start()
    {
        isMusicPlaying = true;
    }


    public void ToggleMusic()
    {
        if (isMusicPlaying)
        {
            isMusicPlaying = false;
            AS_Music.Stop();
            IMG_Music.sprite = SPR_Music_Off;
        }
        else
        {
            isMusicPlaying = true;
            AS_Music.Play();
            IMG_Music.sprite = SPR_Music_On;
        }
    }







}
