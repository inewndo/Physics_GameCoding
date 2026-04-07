using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Color = UnityEngine.Color;

public class ObjectGrabber : MonoBehaviour
{
    public float grabRange = 4;

    public float holdSmoothing = 15;

    //the point in front of the camera where the obj is held 
    public Transform holdPoint;

    //how much force is applied when throwing 
    public float throwforce = 4f;

    private Rigidbody _rb;
    private bool _isHolding = false;
    private Rigidbody heldObject;
    private InteractableObject _currentHighlight;

    public LineRenderer lineRenderer;
    public int linePoints = 20;
    public float timeIntervalbetweenPoints = 0.1f;
    public float throwSpeed = 10f;

    void FixedUpdate()
    {
        //runs on an interval schedule
        //we move held object here os it stays smootha nd physics is accurate

        if (_isHolding && heldObject != null) MoveHeldObject();
    }

    // Update is called once per frame
    void Update()
    {
        //run the detection raycast everyframe to update the highlight
        //this is diff from grab raycast
        UpdateHighlight();

        //draw trajectory when left mousebutton held down
        if(lineRenderer != null)
        {
            if (Input.GetMouseButton(0))
            {
                DrawTrajectory();
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * grabRange, UnityEngine.Color.yellow, 0.5f);

        if (Physics.Raycast(ray, out hit, grabRange))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                //get rb so we can move with physics
                heldObject = hit.collider.GetComponent<Rigidbody>();

                if (heldObject != null)
                {
                    heldObject.useGravity = false;

                    heldObject.freezeRotation = true;

                    heldObject.linearVelocity = Vector3.zero;
                    heldObject.angularVelocity = Vector3.zero;

                    //unhiglight when grabbed, obj is now in hand we dont need highlight anymore
                    interactable.Unhighlight();
                    _currentHighlight = null;

                    _isHolding = true;
                }
            }
        }
    }

    void MoveHeldObject()
    {
        Vector3 targetPos = holdPoint.position;
        Vector3 currentPos = heldObject.position;

        Vector3 newPos = Vector3.Lerp(currentPos, targetPos, holdSmoothing * Time.fixedDeltaTime);

        heldObject.MovePosition(newPos);
    }

    void DropObject()
    {
        if (heldObject == null) return;
        heldObject.useGravity = true;

        heldObject.freezeRotation = false;

        heldObject = null;
        _isHolding = false;
        Debug.Log("Dropped obj");
    }

    void ThrowObject()
    {
        if (heldObject == null) return;
        heldObject.useGravity = true;

        heldObject.freezeRotation = false;

        //apply force in direction cam is facing
        //forcemode.impsule applies force instantly like a punch 
        //as opposite to forcemode.force which applies gradually over time
        heldObject.AddForce(transform.forward * throwforce, ForceMode.Impulse);
        heldObject = null;
        _isHolding = false;
    }

    public void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_isHolding)
        {
            DropObject();


        }
        else
        {
            TryGrab();

        }


    }

    public void OnThrowPerformed(InputAction.CallbackContext context)
    {
        if ((!context.performed))
        {
            return;
        }
        if (_isHolding) ThrowObject();
    }

    void Charge()
    {
        if (_isHolding)
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
        }
    }

    void DrawTrajectory()
    {
        Vector3 origin = holdPoint.position;
        //Vector3 startVelocity = throwSpeed * holdPoint.forward;
        Vector3 startVelocity = transform.forward * throwforce;
        lineRenderer.positionCount = linePoints;
        float time = 0;
        //calculates x,y coordinates of each point from the time
        for(int i = 0; i< linePoints; i++)
        {
            //s=u*t+1/2*g*t*t
            var x = (startVelocity.x * time) + (Physics.gravity.x / 2 * time * time);
            var y = (startVelocity.y * time) + (Physics.gravity.y / 2 * time * time);
            var z = (startVelocity.z * time) + (Physics.gravity.z / 2 * time * time);
            Vector3 point = new Vector3(x, y, z);
            //get current world pos of the point relative to start pos
            lineRenderer.SetPosition(i, origin + point);
            time += timeIntervalbetweenPoints;
        }

    }

    void UpdateHighlight()
    {
        //dont change highlight while holding an object 
        if (_isHolding) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.red);

        if (Physics.Raycast(ray, out hit, grabRange))
        {
            InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
            if (interactableObject != null)
            {
                //if looking at diff obj unhighlight old one 
                if (_currentHighlight != null && _currentHighlight != interactableObject)
                {
                    _currentHighlight.Unhighlight();
                    Debug.Log("UNHIGHLIGHTED");
                }

                //highligh the new obj
                interactableObject.Highlight();
                _currentHighlight = interactableObject;
                return;
            }

            //raycast hits nothing -> clear highlight
            if (_currentHighlight != null)
            {
                _currentHighlight.Unhighlight();
                _currentHighlight = null;
            }
        }
    }
}