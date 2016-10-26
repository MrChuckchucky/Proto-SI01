using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIStumbids : MonoBehaviour
{
    [SerializeField]
    List<GameObject> interactiblePoints;

    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float distanceMaxDestination;

    bool isMoving;

    GameObject destination;

    public GameObject holdingItem;

    int index;

    public void setInteractiblePoints(List<GameObject> value)
    {
        interactiblePoints = value;
        Move();
    }

    void Start()
    {
        index = 0;
        interactiblePoints = new List<GameObject>();
        interactiblePoints = InteractibleManager.instance.getInteractibles();
        Move();
    }

    void Update()
    {
        if(isMoving)
        {
            Vector3 goTo = destination.transform.position;
            GetComponent<NavMeshAgent>().destination = goTo;
            GetComponent<NavMeshAgent>().speed = walkSpeed;
            float distance = Vector3.Distance(transform.position, goTo);
            if (distance <= distanceMaxDestination)
            {
                isMoving = false;
                GetComponent<NavMeshAgent>().destination = transform.position;
                StartCoroutine(Interaction());
            }
        }
    }

    void Move()
    {
        if(interactiblePoints.Count>0 && index < interactiblePoints.Count)
        {
            destination = interactiblePoints[index];
            isMoving = true;
        }
        
    }

    IEnumerator Interaction()
    {

        float interactionTime = interactiblePoints[interactiblePoints.Count - 1].GetComponent<InteractibleItem>().getInteractionTime();
        yield return new WaitForSeconds(interactionTime);
        if(interactiblePoints[interactiblePoints.Count - 1].GetComponent<InteractibleItem>().getCanBeHeld())
        {
            if(holdingItem.tag == "Sheep")
            {
                holdingItem.GetComponent<AISheep>().setHold(false, null);
            }
            holdingItem = interactiblePoints[interactiblePoints.Count - 1];
            if(holdingItem.tag == "Sheep")
            {
                holdingItem.GetComponent<AISheep>().setHold(true, this.gameObject);
            }
        }

        interactiblePoints[index].GetComponent<InteractibleItem>().Interaction(this.gameObject);

        index++;
        Move();
    }
}
