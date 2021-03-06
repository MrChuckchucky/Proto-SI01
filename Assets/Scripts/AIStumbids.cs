﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIStumbids : MonoBehaviour
{
    List<GameObject> interactiblePoints;

    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float distanceMaxDestination;

    bool isMoving;

    GameObject destination;
    GameObject holdingItem;

    public void addInteractiblePoint(GameObject value)
    {
        interactiblePoints.Add(value);
        Move();
    }
    void removeLastInteractiblePoint()
    {
        interactiblePoints.RemoveAt(interactiblePoints.Count - 1);
    }
    public void removeInteractiblepoint(GameObject value)
    {
        for(int i = 0; i < interactiblePoints.Count; i++)
        {
            if(interactiblePoints[i] == value)
            {
                interactiblePoints.RemoveAt(i);
                return;
            }
        }
    }

    void Start()
    {
        GetComponent<NavMeshAgent>().speed = walkSpeed;
        interactiblePoints = new List<GameObject>();
        GameObject[] interacts = GameObject.FindGameObjectsWithTag("Interactible");
        foreach(GameObject obj in interacts)
        {
            addInteractiblePoint(obj);
        }
        Move();
    }

    void Update()
    {
        if(isMoving)
        {
            Vector3 goTo = destination.transform.position;
            GetComponent<NavMeshAgent>().destination = goTo;
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
        destination = interactiblePoints[interactiblePoints.Count - 1];
        isMoving = true;
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
        removeLastInteractiblePoint();
        Move();
    }
}
