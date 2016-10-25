using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    [SerializeField]
    PlayerController otherController;


    public Transform worldMover;
    Vector3 lastPosMover;
    Vector3 posMover;
    Vector3 move;
    public float tolerance;
    

    Ray charles;
    RaycastHit hit;
    public float charlesLength;

    Transform grabbedParent;

    public GameObject parent;

    GameObject grabbedObj;

    Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
    Valve.VR.EVRButtonId redButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    Valve.VR.EVRButtonId spawn = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

    public bool gripPressed;

    public GameObject world;

    bool isGrabbed;

    [SerializeField]
    float worldMoveMult;

    GameObject point;

	// Use this for initialization
	void Start ()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        isGrabbed = false;
        point = transform.GetChild(1).gameObject;
        point.GetComponent<Renderer>().material.color = Color.red;
        point.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(controller.GetPress(redButton))
        {
            PrepareShoot();
        }
        if(controller.GetPressUp(redButton))
        {
            Shoot();
        }

        if(controller.GetPressDown(spawn))
        {
            GameObject stumbid = Instantiate(Resources.Load("Stumbids"), Vector3.zero, Quaternion.identity) as GameObject;
        }


        if (controller.GetHairTriggerDown())
        {
            Grab();
        }

        if (controller.GetHairTriggerUp())
        {
            Drop();
        }


        if (isGrabbed && controller.GetPress(grip))
        {
            if (gripPressed && otherController.gripPressed)
            {
                MoveWorld();
            }

        }
        if (controller.GetPressDown(grip))
        {
            
            if (!gripPressed && otherController.gripPressed)
            {
                GrabWorld();
                isGrabbed = true;
            }
            gripPressed = true;

        }

        if(controller.GetPressUp(grip))
        {
            ResetMover();
            isGrabbed = false;
        }
    }

    void PrepareShoot()
    {
        RaycastHit hit;
        charles.origin = trackedObj.transform.position;
        charles.direction = trackedObj.transform.forward;
        if (Physics.Raycast(charles, out hit))
        {
            point.SetActive(true);
            point.transform.position = hit.point;
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        charles.origin = trackedObj.transform.position;
        charles.direction = trackedObj.transform.forward;
        if (Physics.Raycast(charles, out hit))
        {
            point.SetActive(false);
            if(hit.transform.gameObject.tag != "Ground" && hit.transform.gameObject.tag != "Bear" && hit.transform.gameObject.tag != "Interactible")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    void Grab()
    {

        charles.origin = trackedObj.transform.position;
        charles.direction = trackedObj.transform.forward;
        if (Physics.Raycast(charles, out hit, charlesLength))
        { if(hit.collider.gameObject.layer!=10)
            {
                grabbedParent = hit.collider.transform.parent;
                hit.collider.transform.parent = transform;
                grabbedObj = hit.collider.gameObject;
                grabbedObj.GetComponent<Rigidbody>().useGravity = false;
                grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObj.GetComponent<physiqueobj>().held = true;
            }
           
        }
    }

    void Drop()
    {
        if (grabbedObj!=null)
        {
            grabbedObj.transform.parent = grabbedParent;
            grabbedObj.GetComponent<Rigidbody>().useGravity = true;
            grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObj.GetComponent<physiqueobj>().held = false;
            grabbedObj = null;
           
        }
        
    }

    void GrabWorld()
    {
        posMover = new Vector3((transform.localPosition.x + otherController.transform.localPosition.x) / 2, (transform.localPosition.y + otherController.transform.localPosition.y) / 2, (transform.localPosition.z + otherController.transform.localPosition.z) / 2);
     
       
    }

    void MoveWorld()
    {
        lastPosMover = posMover;
        posMover = new Vector3((transform.localPosition.x + otherController.transform.localPosition.x) / 2, (transform.localPosition.y + otherController.transform.localPosition.y) / 2, (transform.localPosition.z + otherController.transform.localPosition.z) / 2);
        move = (posMover - lastPosMover) * worldMoveMult;
        if(Mathf.Abs(move.x)<tolerance)
        {
            move.Set(0f, move.y, move.z);
        }

        if (Mathf.Abs(move.y) < tolerance)
        {
            move.Set(move.x, 0f, move.z);
        }

        if (Mathf.Abs(move.z) < tolerance)
        {
            move.Set(move.x, move.y, 0f);
        }
       
       parent.transform.position -= (posMover - lastPosMover) * worldMoveMult;
    }

    void ResetMover()
    {
        world.transform.parent = null;
        gripPressed = false;
      
    }
}
