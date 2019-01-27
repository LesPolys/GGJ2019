using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravGunJuice : MonoBehaviour
{

    [SerializeField]
    public Transform lookpoint;
   
  
    //private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;



    // Update is called once per frame
    void Update()
    {

 
        Quaternion targetRotation = Quaternion.LookRotation(lookpoint.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);

    }
}
