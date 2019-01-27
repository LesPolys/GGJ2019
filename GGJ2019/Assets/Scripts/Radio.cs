using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource radioAudioSource;


    private float songStartTime;
    float songLength;

    float delayBetweenSongs = 0.4f;

    int currentClip = 0;


    // Start is called before the first frame update
    void Start()
    {
        radioAudioSource = GetComponent<AudioSource>();


        radioAudioSource.spatialBlend = 1f;
        

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


            if (currentClip >= audioClips.Length)
            {
                currentClip = 0;
                //Debug.Log("CurrentClipReset");
                
            }
            else
            {
                currentClip += 1;
                
               
            }

            playSong(currentClip);

        }

    }


}
