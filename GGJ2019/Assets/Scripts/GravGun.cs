using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GravGun : MonoBehaviour
{

    public float shootForce;
    public float pushPullForce;
    public float minDistance;
    public float maxDistance;

  

    /// <summary>The rigidbody we are currently holding</summary>
    private new Rigidbody rigidbody;

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
    private const float maxGrabDistance = 30;

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

            if (rigidbody != null)
            {
                // Reset the rigidbody to how it was before we grabbed it
                rigidbody.interpolation = initialInterpolationSetting;

                rigidbody = null;
            }

            return;
        }

        if (Input.GetMouseButtonDown(1) && rigidbody == null)
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
                    rigidbody = hit.rigidbody;
                    initialInterpolationSetting = rigidbody.interpolation;
                    rotationDifferenceEuler = hit.transform.rotation.eulerAngles - transform.rotation.eulerAngles;

                    hitOffsetLocal = hit.transform.InverseTransformVector(hit.point - hit.transform.position);

                    currentGrabDistance = Vector3.Distance(ray.origin, hit.point);

                    // Set rigidbody's interpolation for proper collision detection when being moved by the player
                    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
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
        if (rigidbody)
        {
            // We are holding an object, time to rotate & move it

            Ray ray = CenterRay();

            // Rotate the object to remain consistent with any changes in player's rotation
            rigidbody.MoveRotation(Quaternion.Euler(rotationDifferenceEuler + transform.rotation.eulerAngles));

            // Get the destination point for the point on the object we grabbed
            Vector3 holdPoint = ray.GetPoint(currentGrabDistance);
            Debug.DrawLine(ray.origin, holdPoint, Color.blue, Time.fixedDeltaTime);

            // Apply any intentional rotation input made by the player & clear tracked input
            Vector3 currentEuler = rigidbody.rotation.eulerAngles;
            rigidbody.transform.RotateAround(holdPoint, transform.right, rotationInput.y);
            rigidbody.transform.RotateAround(holdPoint, transform.up, -rotationInput.x);

            // Remove all torque, reset rotation input & store the rotation difference for next FixedUpdate call
            rigidbody.angularVelocity = Vector3.zero;
            rotationInput = Vector2.zero;
            rotationDifferenceEuler = rigidbody.transform.rotation.eulerAngles - transform.rotation.eulerAngles;

            // Calculate object's center position based on the offset we stored
            // NOTE: We need to convert the local-space point back to world coordinates
            Vector3 centerDestination = holdPoint - rigidbody.transform.TransformVector(hitOffsetLocal);

            // Find vector from current position to destination
            Vector3 toDestination = centerDestination - rigidbody.transform.position;

            // Calculate force
            Vector3 force = toDestination / Time.fixedDeltaTime;

            // Remove any existing velocity and add force to move to final position
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(force, ForceMode.VelocityChange);


            if (Input.GetMouseButton(0))
            {
            
                rigidbody.AddForce((rigidbody.transform.position - transform.position) * shootForce, ForceMode.Force );
                rigidbody.interpolation = initialInterpolationSetting;

                rigidbody = null;
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