using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{

    Vector3 finalPosition;
    Quaternion finalRotation;

    Vector3 lastPosition;

    public float maxScoreVale;
    public float scoreCheckCountDown;
    public const float maxDistValue = 10.0f;
    public float minDistCutoff = 0.01f;
    private bool isMoving;
    private bool isCountingDown;

    private void Update()
    {
        if (transform.position != lastPosition) // has it started moving
        {
            transform.position = lastPosition;
            isMoving = true;

        }

        if (isMoving && transform.position == lastPosition) { // has it stopped moving. 
            isMoving = false;
            isCountingDown = true;
        }

        if (isCountingDown)
        {
            scoreCheckCountDown -= Time.deltaTime;
            if(scoreCheckCountDown <= 0)
            {
                RunScoreCheck();
            }
        }

    }

    public void SetFinalValues()
    {
        finalPosition = transform.position;
        finalRotation = transform.rotation;
    }


    private void RunScoreCheck()
    {
        float distanceScore;
        float finalDistance = Vector3.Distance(transform.position, finalPosition);
        if (finalDistance >= maxDistValue)
        {
            distanceScore = 0;
        }
        else
        {
            if (finalDistance < minDistCutoff)
            {
                distanceScore = maxScoreVale;
            }
            else
            {
                distanceScore = maxScoreVale / finalDistance;
            }
            
        }

       // transform.rotation.
      


   
    }



}
