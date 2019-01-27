using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{

    public AudioSource sound;
    public float minPitchRange = 1f;
    public float maxPitchRange = 1f;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource> ();
        sound.spatialBlend = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        sound.pitch = Random.Range(minPitchRange, maxPitchRange);
        sound.Play();
    }

}
