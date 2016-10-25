using UnityEngine;
using System.Collections;

public class AIBear : MonoBehaviour
{
    [SerializeField]
    Color color;
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
    float eatTime;

    bool isEating;
    bool isMoving;
    bool isWaiting;
    bool isRunning;

    Vector3 destination;

    GameObject target;

    void Start()
    {
        GetComponent<Renderer>().material.color = color;
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

    IEnumerator Waiting()
    {
        waitTime = Random.Range(waitTimeMin, waitTimeMax);
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        Move();
    }

    IEnumerator Eating()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = target.transform.position;
        float distance = distanceMaxDestination + 10.0f;
        while(distance > distanceMaxDestination)
        {
            distance = Vector3.Distance(transform.position, destination);
            GetComponent<NavMeshAgent>().destination = target.transform.position;
            yield return new WaitForEndOfFrame();
        }
        Destroy(target.gameObject);
        target = null;
        isEating = true;
        eatTime = Random.Range(eatTimeMin, eatTimeMax);
        yield return new WaitForSeconds(eatTime);
        isEating = false;
    }

    IEnumerator Flying()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = destination;
        isRunning = true;
        float distance = distanceMaxDestination + 10.0f;
        while (distance > distanceMaxDestination)
        {
            distance = Vector3.Distance(transform.position, destination);
            yield return new WaitForEndOfFrame();
        }
        destination = transform.position;
        GetComponent<NavMeshAgent>().destination = destination;
        isRunning = false;
    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Fire")
        {
            float temp = Vector3.Distance(transform.position, collider.transform.position);
            destination = transform.position + new Vector3((transform.position.x - collider.transform.position.x) * flyingDistance / temp, transform.position.y, (transform.position.z - collider.transform.position.z) * flyingDistance / temp);
            StartCoroutine("Flying");
        }
        else if (!isEating && !isRunning)
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
