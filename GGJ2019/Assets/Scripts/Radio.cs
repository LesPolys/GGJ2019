using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioClip[] audioClips;
    AudioSource radioAudioSource;


    private float songStartTime;
    float songLength;

    float delayBetweenSongs = 0.4f;

    int currentClip = 0;


    // Start is called before the first frame update
    void Start()
    {
        radioAudioSource = GetComponent<AudioSource>();

        playSong(currentClip);
        

            

    }


    private void playSong(int clipToPlay)
    {

        radioAudioSource.clip = audioClips[clipToPlay];
        songStartTime = Time.time;
        songLength = radioAudioSource.clip.length;
        radioAudioSource.Play();


        
    }


    // Update is called once per frame
    void Update()
    {


        CheckForSongEnd();
    }


    void CheckForSongEnd ()
    {

        if (Time.time >= songStartTime + songLength +  delayBetweenSongs)
        {

            currentClip += 1;
            playSong(currentClip);
            //change clip
            //play song
        }

    }


}
