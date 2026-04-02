using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{

    [Header("Weight Settings")]
    //how  much total weight is needed to activate pres plate
    public float weightTreshold = 5f;

    //if true the plate stays activated even after weight is removed
    public bool lockOnActivate = false;

    //events
    //event fired when final weight exceeds treshold
    //diff from static event, unity event needs to be wired in inspector and is more akin to buttons
    //static is just code, does not need a ref tp sender, not too designer friendly
    public UnityEvent onActivated;

    //fired when weight drops below the treshold(ignore is lockOnActivate is true
    public UnityEvent onDeactivated;

    //visual feedback
    //optional, the plate mesh that mvees when pressed
    public Transform plate;

    //how far the plate depresses when pressed (world units)
    public float pressDepth = 0.05f;

    float currentWeight = 0f;
    bool isActivated = false;
    bool isLocked = false;
    Vector3 plateResetPos;
    Vector3 platePressedPos;

    HashSet<PhysicsObjects> objectsOnPlate = new HashSet<PhysicsObjects>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(plate != null)
        {
            //stores where plate is
            plateResetPos = plate.localPosition;
            //moving down
            platePressedPos = plateResetPos + Vector3.down * pressDepth;
        }
    }

    //fires when any collider enters the trigger zone
    //we check phys obj to get the weight
    private void OnTriggerEnter(Collider other)
    {
        PhysicsObjects physObj = other.GetComponent<PhysicsObjects>();
        if (physObj == null) return;

        //so it does not  trigger when you are still holding the obj in trigger zone
        if (physObj.isHeld) return;

        ////first simple version
        //currentWeight += physObj.puzzleWeight;
        //Debug.Log($"{other.gameObject.name} entered plate. total weight: {currentWeight}");
        //CheckActivation();

        //this is instead of adding it to a list to avoid double activating
        if (objectsOnPlate.Add(physObj))
        {
            currentWeight += physObj.puzzleWeight;
            CheckActivation();
        }
    }

    //called whneever weight changes, activates if treshold is met
    void CheckActivation()
    {
        if(!isActivated && currentWeight>= weightTreshold)
        {
            isActivated = true;
            if (lockOnActivate) isLocked = true;

            //calss it for wtv is listening to it
            onActivated.Invoke();
            Debug.Log("Pressure plate is activated");

            if(plate != null)
            {
                //after its aactivated - move the plate
                plate.localPosition = platePressedPos;
            }
        }
    }

    //call when weight is removed, deactivates if below treshold
    void CheckDeactivation()
    {
        if(isActivated && !isLocked && currentWeight < weightTreshold)
        {
            isActivated = false;
            onDeactivated.Invoke();
            Debug.Log("pressure plate is deactivated");

            if(plate != null)
            {
                plate.localPosition = plateResetPos;
            }
        }
    }
}
