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
    public const float minDistValue = 10.0f;
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
        float finalDistance = Vector3.Distance(transform.position, finalPosition);


   
    }



}
