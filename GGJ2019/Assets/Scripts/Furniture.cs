using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{

    public Vector3 finalPosition;
    public Quaternion finalRotation;

    Vector3 lastPosition;

    public string furnitureName;

    public float maxScoreVal;
    public float scoreCheckCountDown = 2;
    public const float maxDistValue = 10.0f;
    public float minDistCutoff = 0.01f;
    private bool isMoving;
    private bool isCountingDown;

    float rotScoreMultiplier = 2.5f;

    public float finalScore { get; private set; }

    private void Update()
    {
        if (transform.position != lastPosition && !isCountingDown) // has it started moving
        {
            lastPosition = transform.position;
            isMoving = true;
            isCountingDown = false;
            scoreCheckCountDown = 2;

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
                scoreCheckCountDown = 2;
                isCountingDown = false;
                RunScoreCheck();
            }
        }


    }

    public void SetFinalValues()
    {
        finalPosition = transform.position;
        finalRotation = transform.rotation;
    }


    public void RunScoreCheck()
    {
        float distanceScore;
        float finalDistance = Vector3.Distance(transform.position, finalPosition);
        if (finalDistance >= maxDistValue)
        {
            distanceScore = -maxScoreVal * 10;
        }
        else
        {
            if (finalDistance < minDistCutoff)
            {
                distanceScore = maxScoreVal;
            }
            else
            {
                distanceScore = (Mathf.Pow(-finalDistance, 2)) + maxScoreVal;
            }
            
        }


        float xAngle = transform.rotation.eulerAngles.x - finalRotation.eulerAngles.x;
        float xScore = (Mathf.Cos(xAngle) / rotScoreMultiplier) + 1;

        float yAngle = transform.rotation.eulerAngles.y - finalRotation.eulerAngles.y;
        float yScore = (Mathf.Cos(yAngle) / rotScoreMultiplier) + 1;

        float zAngle = transform.rotation.eulerAngles.z - finalRotation.eulerAngles.z;
        float zScore = (Mathf.Cos(zAngle) / rotScoreMultiplier) + 1;


        float finalRotScore = xScore + yScore + zScore / 3;


        finalScore = distanceScore + finalRotScore;
        print(furnitureName);
        print(furnitureName + finalScore);
    }



}
