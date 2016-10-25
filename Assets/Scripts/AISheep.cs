using UnityEngine;
using System.Collections;

public class AISheep : MonoBehaviour
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
    float distanceMaxDestination;
    [SerializeField]
    float waitTimeMin;
    [SerializeField]
    float waitTimeMax;
    [SerializeField]
    float flyingDistance;

    float waitTime;

    Vector3 destination;

    bool isRunning;
    bool isMoving;
    bool isWaiting;
    bool isHold;

    GameObject follow;

    public void setHold(bool value, GameObject owner)
    {
        isHold = value;
        follow = owner;
    }

    void Update()
    {
        if (!isRunning && !isHold)
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
        else if(isHold)
        {
            transform.position = follow.transform.position;
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

    IEnumerator Flying()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = destination;
        isRunning = true;
        float distance = distanceMaxDestination + 10.0f;
        while (distance > distanceMaxDestination)
        {
            destination += transform.position;
            GetComponent<NavMeshAgent>().destination = destination;
            yield return new WaitForEndOfFrame();
        }
        isRunning = false;
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