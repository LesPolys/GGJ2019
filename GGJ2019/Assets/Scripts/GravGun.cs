using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GravGun : MonoBehaviour
{

    public float shootForce;
    public float pushPullForce;
    public float minDistance;
    public float maxDistance;

    private float chargeValue;
    public Slider chargeSlider;

    /// <summary>The rigidbody we are currently holding</summary>
    public Rigidbody HoldingObject { get; private set; }

    #region Held Object Info
    /// <summary>The offset vector from the object's position to hit point, in local space</summary>
    private Vector3 hitOffsetLocal;

    /// <summary>The distance we are holding the object at</summary>
    private float currentGrabDistance;

    /// <summary>The interpolation state when first grabbed</summary>
    private RigidbodyInterpolation initialInterpolationSetting;

    /// <summary>The difference between player & object rotation, updated when picked up or when rotated by the player</summary>
    private Vector3 rotationDifferenceEuler;
    #endregion

    /// <summary>Tracks player input to rotate current object. Used and reset every fixedupdate call</summary>
    private Vector2 rotationInput;

    /// <summary>The maximum distance at which a new object can be picked up</summary>
    public float maxGrabDistance = 30;

    /// <returns>Ray from center of the main camera's viewport forward</returns>
    private Ray CenterRay()
    {
        return Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
    }

    void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            // We are not holding the mouse button. Release the object and return before checking for a new one

            if (HoldingObject != null)
            {
                // Reset the rigidbody to how it was before we grabbed it
                HoldingObject.interpolation = initialInterpolationSetting;

                HoldingObject = null;
            }

            return;
        }

        if (Input.GetMouseButtonDown(1) && HoldingObject == null)
        {
            // We are not holding an object, look for one to pick up

            Ray ray = CenterRay();
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * maxGrabDistance, Color.blue, 0.01f);

            if (Physics.Raycast(ray, out hit, maxGrabDistance))
            {
                // Don't pick up kinematic rigidbodies (they can't move)
                if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
                {
                    // Track rigidbody's initial information
                    HoldingObject = hit.rigidbody;
                    initialInterpolationSetting = HoldingObject.interpolation;
                    rotationDifferenceEuler = hit.transform.rotation.eulerAngles - transform.rotation.eulerAngles;

                    hitOffsetLocal = hit.transform.InverseTransformVector(hit.point - hit.transform.position);

                    currentGrabDistance = Vector3.Distance(ray.origin, hit.point);

                    // Set rigidbody's interpolation for proper collision detection when being moved by the player
                    HoldingObject.interpolation = RigidbodyInterpolation.Interpolate;
                }
            }
        }
        else
        {
            // We are already holding an object, listen for rotation input

            if (Input.GetKey(KeyCode.R))
            {
                rotationInput += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            }

        


        }


    }

    private void FixedUpdate()
    {
        if (HoldingObject)
        {
            // We are holding an object, time to rotate & move it

            Ray ray = CenterRay();

            // Rotate the object to remain consistent with any changes in player's rotation
            HoldingObject.MoveRotation(Quaternion.Euler(rotationDifferenceEuler + transform.rotation.eulerAngles));

            // Get the destination point for the point on the object we grabbed
            Vector3 holdPoint = ray.GetPoint(currentGrabDistance);
            Debug.DrawLine(ray.origin, holdPoint, Color.blue, Time.fixedDeltaTime);

            // Apply any intentional rotation input made by the player & clear tracked input
            Vector3 currentEuler = HoldingObject.rotation.eulerAngles;
            HoldingObject.transform.RotateAround(holdPoint, transform.right, rotationInput.y);
            HoldingObject.transform.RotateAround(holdPoint, transform.up, -rotationInput.x);

            // Remove all torque, reset rotation input & store the rotation difference for next FixedUpdate call
            HoldingObject.angularVelocity = Vector3.zero;
            rotationInput = Vector2.zero;
            rotationDifferenceEuler = HoldingObject.transform.rotation.eulerAngles - transform.rotation.eulerAngles;

            // Calculate object's center position based on the offset we stored
            // NOTE: We need to convert the local-space point back to world coordinates
            Vector3 centerDestination = holdPoint - HoldingObject.transform.TransformVector(hitOffsetLocal);

            // Find vector from current position to destination
            Vector3 toDestination = centerDestination - HoldingObject.transform.position;

            // Calculate force
            Vector3 force = toDestination / Time.fixedDeltaTime;

            // Remove any existing velocity and add force to move to final position
            HoldingObject.velocity = Vector3.zero;
            HoldingObject.AddForce(force, ForceMode.VelocityChange);


            if (Input.GetMouseButton(0))
            {
          
                if(chargeValue < 1)
                {
                    chargeValue += 0.01f;
                }else{
                    chargeValue = 1;
                }
                chargeSlider.value = chargeValue;
           
                
            }

            if (Input.GetMouseButtonUp(0))
            {
                HoldingObject.AddForce((HoldingObject.transform.position - transform.position) * shootForce * chargeValue, ForceMode.Force);
                HoldingObject.interpolation = initialInterpolationSetting;

                HoldingObject = null;
                chargeValue = 0.0f;
                chargeSlider.value = chargeValue;
            }


            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                float newGrabDistance = currentGrabDistance + pushPullForce;
                if (newGrabDistance > maxDistance)
                {
                    newGrabDistance = currentGrabDistance;
                }
                currentGrabDistance = newGrabDistance;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {

                float newGrabDistance = currentGrabDistance - pushPullForce;
                if (newGrabDistance < minDistance)
                {
                    newGrabDistance = currentGrabDistance;
                }
                currentGrabDistance = newGrabDistance;

              
            }


        }


    }
}