using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static RootMotion.FinalIK.FBIKChain;

public class bioController : MonoBehaviour
{

    //Velocity
    //Acceleration
    //Trajectory
    //Joint Angles
    //Angular Velocity
    //Angular Acceleration
    //Impulse (Impact Force)
    //Length of the swing, from the starting phase to contact with the ball
    //Distance from the support leg to the ball
    public GameObject rhandTarget;
    public GameObject lhandTarget;
    public GameObject rLegTarget;
    public GameObject lLegTarget;
    public GameObject hipTarget;
    //joint angle variables
    //ankle
    public Transform endEffector;
    //hip
    public Transform upperJoint;
    //knee
    public Transform lowerJoint;
    Vector3 previousLegPosition = Vector3.zero;
    Vector3 currentLegPosition;
    float deltaTime;
    Vector3 velocity = Vector3.zero;
    Vector3 previousVelocity = Vector3.zero;
    Vector3 currentVelocity;
    Vector3 acceleration;
    Vector3 upperBone;
    Vector3 lowerBone;
    //Trajectory Variables
    public Transform footTransform; // Assign the foot transform from which to capture the trajectory
    public LineRenderer lineRenderer;
    public int maxPositions = 100; // Maximum number of positions to record
    public float recordRate = 0.05f; // How often to record the position (in seconds)
    private List<Vector3> positions = new List<Vector3>();
    private float timer;
    // Angular Velocity Variables Knee
    public Transform kneeJointTransform; // Assign the joint Transform to monitor (e.g., the knee)
    public float smoothing = 0.1f; // Smoothing factor for the angular velocity calculatio
    private Quaternion previousRotation;
    private Vector3 angularVelocity;
    //private Quaternion lastRotationKnee;
    //private float angularVelocitKnee;
    //// Angular Velocity Variables Ankle
    //public GameObject kneeTarget;
    //public GameObject ankleTarget;
    //private Quaternion lastRotationAnkle;
    //private float angularVelocityAnkle;
    // Start is called before the first frame update
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.widthMultiplier = 0.02f; // Set the thickness of the trajectory line
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = Color.red };
        }

        timer = recordRate;
        currentVelocity = velocity;

        //initialize rotation for angular V
        if (kneeJointTransform != null)
        {
            // Initialize previousRotation with the initial rotation of the joint
            previousRotation = kneeJointTransform.rotation;
        }
        else
        {
            Debug.LogError("Joint Transform is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()

    {   
        //Velocity of the Leg
        currentLegPosition = rLegTarget.transform.position;
        deltaTime = Time.deltaTime;
        velocity = (currentLegPosition - previousLegPosition) / deltaTime;
        previousLegPosition = currentLegPosition;
        Debug.Log("Velocity of the Leg: " + velocity);
        //acceleration of the Leg
        acceleration = (currentVelocity - previousVelocity) / deltaTime;
        previousVelocity = currentVelocity;
        //calculate joint angles
  
        float angle = angleBetweenJoints();
        Debug.Log("Angle between joints: " + angle);
        //Trajectory Line
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            RecordPosition();
            timer = recordRate;
        }
        DrawTrajectory();
        //knee amgular velocity calc
        if (kneeJointTransform != null)
        {
            CalculateAngularVelocity();
            Debug.Log("Current Angular Velocity: " + angularVelocity);
        }
        //Quaternion deltaRotationKnee = transform.rotation * Quaternion.Inverse(lastRotationKnee);
        //deltaRotationKnee.ToAngleAxis(out float angleInDegreesKnee, out Vector3 rotationAxisKnee);
        //if (angleInDegreesKnee > 180)
        //    angleInDegreesKnee -= 360;
        //// Angular velocity in degrees per second
        //angularVelocitKnee = angleInDegreesKnee / Time.deltaTime;
        //// Update lastRotation
        //lastRotationKnee = kneeTarget.transform.rotation;
        //Debug.Log("Angular Velocity: " + angularVelocitKnee + " degrees/sec");
        ////ankle amgular velocity calc
        //Quaternion deltaRotationAnkle = transform.rotation * Quaternion.Inverse(lastRotationAnkle);
        //deltaRotationAnkle.ToAngleAxis(out float angleInDegreesAnkle, out Vector3 rotationAxisAnkle);
        //if (angleInDegreesAnkle > 180)
        //    angleInDegreesAnkle -= 360;
        //angularVelocityAnkle = angleInDegreesAnkle / Time.deltaTime;
        //Debug.Log("Angular Velocity: " + angularVelocityAnkle + " degrees/sec");
    }


    float angleBetweenJoints() {

        upperBone = lowerJoint.position - upperJoint.position;
        lowerBone = endEffector.position - lowerJoint.position;
        upperBone.Normalize();
        lowerBone.Normalize();
        return Mathf.Acos(Vector3.Dot(upperBone, lowerBone)) * Mathf.Rad2Deg;
        
    }
    void RecordPosition()
    {
        if (positions.Count >= maxPositions)
        {
            positions.RemoveAt(0); // Remove the oldest position to maintain size limit
        }

        positions.Add(footTransform.position); // Record the current position of the foot
    }

    void DrawTrajectory()
    {
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
    void CalculateAngularVelocity()
    {
        // Calculate the difference in rotation
        Quaternion deltaRotation = kneeJointTransform.rotation * Quaternion.Inverse(previousRotation);

        // Convert the quaternion to Euler angles (in radians)
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
        angle *= Mathf.Deg2Rad;

        // Ensure the angle is not negative
        if (angle < 0)
        {
            angle += 2 * Mathf.PI;
        }

        // Calculate angular velocity
        Vector3 currentAngularVelocity = angle * axis / Time.deltaTime;

        // Apply smoothing to the angular velocity to reduce noise
        angularVelocity = Vector3.Lerp(angularVelocity, currentAngularVelocity, smoothing);

        // Update previousRotation
        previousRotation = kneeJointTransform.rotation;
    }
}
