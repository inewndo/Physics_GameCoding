using TMPro;
using UnityEngine;

public class PhysicsObjects : MonoBehaviour
{
    [Header("Mass & Motion")]
    //how heave the object is in kg affects how much force is needed to move it
    [Range(0.1f, 100f)]
    public float mass = 1f;

    //linear drag, how quickly does the object slow down in the air 0- no draft, 10- very sluggish
    [Range(0f, 10f)]
    public float drag = 0.5f;

    //angular drag, how quickly spinning slows down
    [Range(0f, 10f)]
    public float angularDrag = 0.5f;

    [Header("Surface Properties")]
    //bounciness of the surface, 0- no bounce. 1 perfect bounce; requires physics mat on collider
    [Range(0f, 1f)]
    public float bouncinnes = 0f;
    [Range(0f , 1f)]
    public float friction = 0.6f;

    [Header("Puzzle Properties")]
    //effective weight used by pressure plate
    //defaults the mass but can be overwritten
    public float puzzleWeight = -1f;

    Rigidbody rb;
    PhysicsMaterial physMat;
    public bool isHeld = false;
    public TextMeshProUGUI weight;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //call function here
        ApplyRigidBodySettings();
        ApplySurfaceSettings();
    }

    //sets mass and drag directly
    void ApplyRigidBodySettings()
    {
        rb.mass = mass;
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
    }

    //physic mat in unity control bounce and friction
    //we create phys mat in runtime and assign it
    void ApplySurfaceSettings()
    {
        physMat = new PhysicsMaterial(gameObject.name);
        physMat.bounciness = bouncinnes;
        physMat.dynamicFriction = friction;
        physMat.staticFriction = friction;

        //combineMode.maximum mean the highr friction of the two
        //colliding obj wins, good default for solid obj
        physMat.frictionCombine = PhysicsMaterialCombine.Average;
        physMat.bounceCombine = PhysicsMaterialCombine.Maximum;

        //assign mat to the collider
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.material = physMat;
        }
    }

    //preview in editor
    //when you change values in the inspector during play mode, makes changes apply
    private void OnValidate()
    {
        //on validate runs in the editor whenever an inspector value changes
        if (rb != null) ApplyRigidBodySettings();
    }

    public float GetWeight()
    {
        if (puzzleWeight >= 0f) return puzzleWeight;
        return mass;
    }
}
