using UnityEngine;
using System.Collections;

public class AIBear : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float distanceWalk;
    [SerializeField]
    float waitTimeMin;
    [SerializeField]
    float waitTimeMax;
    [SerializeField]
    float eatTimeMin;
    [SerializeField]
    float eatTimeMax;
    [SerializeField]
    float distanceMaxDestination;
    [SerializeField]
    float flyingDistance;

    float waitTime;
    float waitingTime;
    float eatTime;
    float eatingTime;

    bool isEating;
    bool isMoving;
    bool isWaiting;
    bool isRunning;

    Vector3 destination;

    GameObject target;

    void Start()
    {
    }

    void Update()
    {
        if(!isRunning)
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            float distance = Vector3.Distance(transform.position, destination);
            if (isMoving && distance <= distanceMaxDestination)
            {
                isMoving = false;
                destination = transform.position;
            }
            if (!isMoving && !isEating && !isWaiting)
            {
                isWaiting = true;
                StartCoroutine("Waiting");
            }
        }
    }

    void Move()
    {
        isMoving = true;
        float randX = Random.Range(0, distanceWalk * 2) - distanceWalk;
        float randZ = Random.Range(0, distanceWalk * 2) - distanceWalk;
        destination = new Vector3(transform.position.x + randX, 0, transform.position.z + randZ);
        GetComponent<NavMeshAgent>().destination = destination;
        GetComponent<NavMeshAgent>().speed = walkSpeed;
    }

    IEnumerable Waiting()
    {
        waitingTime = 0.0f;
        waitTime = Random.Range(waitTimeMin, waitTimeMax);
        while(waitingTime < waitTime)
        {
            waitingTime += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
        isWaiting = false;
        Move();
    }

    IEnumerable Eating()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = target.transform.position;
        float distance = distanceMaxDestination + 10.0f;
        while(distance > distanceMaxDestination)
        {
            distance = Vector3.Distance(transform.position, destination);
            GetComponent<NavMeshAgent>().destination = target.transform.position;
        }
        target = null;
        isEating = true;
        eatingTime = 0.0f;
        eatTime = Random.Range(eatTimeMin, eatTimeMax);
        while(eatingTime < eatTime)
        {
            eatTime += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
        isEating = false;
    }
    
    IEnumerable Flying()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = destination;
        isRunning = true;
        float distance = distanceMaxDestination + 10.0f;
        while (distance > distanceMaxDestination)
        {
            distance = Vector3.Distance(transform.position, destination);
        }
        GetComponent<NavMeshAgent>().destination = destination;
        isRunning = false;
        yield return null;
    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Fire")
        {
            float temp = Vector3.Distance(transform.position, collider.transform.position);
            destination = transform.position + new Vector3((transform.position.x - collider.transform.position.x) * flyingDistance / temp, transform.position.y, (transform.position.z - collider.transform.position.z) * flyingDistance / temp);
            StartCoroutine("Flying");
        }
        else
        {
            if (collider.tag == "Stumbids" || collider.tag == "Sheep")
            {
                if (target == null || (target.tag == "Stumbids" && collider.tag == "Sheep"))
                {
                    target = collider.gameObject;
                    StartCoroutine("Eating");
                }
            }
        }
    }
}
