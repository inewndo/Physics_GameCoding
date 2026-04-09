using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HingeObject : MonoBehaviour
{
    public float minAngle = 0f; //min angle hinge can rotate to 0=close
    public float maxAngle = 90f; //max angle hinge can rotate to 90=open
    public bool useSpring = true; //hinge springs back if released
    public float springTargetAngle = 0f;//the angle spring tries to retrn to
    public float springForce = 50f; //how strong force is
    public float springDamper = 5f;

    //fored when hinge returns or passes min or max angle
    public UnityEvent OnReachMax;
    public UnityEvent OnReachMin;

    public float eventTreshold = 5f; //how close to the limit angle b4 the vent fired

    HingeJoint hinge;
    bool maxEventFired = false;
    bool minEventFired = false;


    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        ConfigureHinge();
    }

    // Update is called once per frame
    void Update()
    {
        //check if we hit the limits and should fore puzzle events
        float currentAngle = hinge.angle;
        if (!maxEventFired && currentAngle >= maxAngle - eventTreshold)
        {
            maxEventFired = true;
            minEventFired = false;
            OnReachMax?.Invoke();
            Debug.Log(gameObject.name + "hinge reached max angle");
        }
        if(!minEventFired && currentAngle <= minAngle + eventTreshold)
        {
            minEventFired = true;
            maxEventFired = false;
            OnReachMin?.Invoke();
            Debug.Log(gameObject.name + "hinge reached min angle");
        }
    }

    //configure hinge, sets join limits and spring through code
    void ConfigureHinge()
    {
        //limits, joint limits is a struct we have to set all fields then assign it back
        JointLimits limits = hinge.limits;
        limits.min = minAngle;
        limits.max = maxAngle;
        limits.bounciness = 0f;
        limits.bounceMinVelocity = 0.2f;
        hinge.limits = limits;
        hinge.useLimits = true;

        if (useSpring)
        {
            JointSpring spring = hinge.spring;
            spring.targetPosition = springTargetAngle;
            spring.spring = springForce;
            spring.damper = springDamper;
            hinge.spring = spring;
            hinge.useSpring = true;
        }
        else
        {
            hinge.useSpring = false;
        }
    }

    public void DriveToMax()
    {
        SetMotorTarget(maxAngle);
    }
    public void DriveToMin()
    {
        SetMotorTarget(minAngle);
    }
    void SetMotorTarget(float targetAngle)
    {
        JointMotor motor = hinge.motor;
        //motor velocitydirection determines which way it moves
        motor.targetVelocity = targetAngle > hinge.angle ? 50f : -50f; //shorthand if statement
        motor.force = 100f;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;
    }
}
