using UnityEngine;
using System.Collections;

public class AISheep : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float distanceWalk;
    [SerializeField]
    float distanceMaxDestination;
    [SerializeField]
    float waitTimeMin;
    [SerializeField]
    float waitTimeMax;
    [SerializeField]
    float flyingDistance;

    float waitTime;
    float waitingTime;

    Vector3 destination;

    bool isRunning;
    bool isMoving;
    bool isWaiting;

    void Update()
    {
        if (!isRunning)
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            float distance = Vector3.Distance(transform.position, destination);
            if (isMoving && distance <= distanceMaxDestination)
            {
                isMoving = false;
                destination = transform.position;
            }
            if (!isMoving && !isWaiting)
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
        while (waitingTime < waitTime)
        {
            waitingTime += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
        isWaiting = false;
        Move();
    }

    IEnumerable Flying()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = destination;
        isRunning = true;
        float distance = distanceMaxDestination + 10.0f;
        while (distance > distanceMaxDestination)
        {
            destination += transform.position;
            GetComponent<NavMeshAgent>().destination = destination;
        }
        isRunning = false;
        yield return null;
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Bear")
        {
            float temp = Vector3.Distance(transform.position, collider.transform.position);
            destination = transform.position + new Vector3((transform.position.x - collider.transform.position.x) * flyingDistance / temp, transform.position.y, (transform.position.z - collider.transform.position.z) * flyingDistance / temp);
            StartCoroutine("Flying");
        }
    }
}