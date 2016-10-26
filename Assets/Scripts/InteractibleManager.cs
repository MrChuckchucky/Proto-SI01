using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractibleManager : MonoBehaviour
{
    public static InteractibleManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        interactibles = new List<GameObject>();
    }

    List<GameObject> interactibles;
    public bool stumbidSpwn;

    public List<GameObject> getInteractibles()
    {
        return interactibles;
    }
    public void addInteractible(GameObject value)
    {
        interactibles.Add(value);
        changeList();
    }
    public void removeInteractible(GameObject value)
    {
        for(int i = 0; i < interactibles.Count; i++)
        {
            if(interactibles[i] == value)
            {
                interactibles.RemoveAt(i);
                changeList();
                return;
            }
        }
    }
    public void removeLastInteractible()
    {
        interactibles.RemoveAt(interactibles.Count - 1);
        changeList();
    }

    void changeList()
    {
        GameObject[] stumbids = GameObject.FindGameObjectsWithTag("Stumbids");
        foreach(GameObject stum in stumbids)
        {
            stum.GetComponent<AIStumbids>().setInteractiblePoints(interactibles);
        }
    }
}
