using UnityEngine;
using System.Collections;

public class InteractibleItem : MonoBehaviour
{
    [SerializeField]
    float interactionTime;
    [SerializeField]
    bool canBeHeld;

    public float getInteractionTime()
    {
        return interactionTime;
    }
    public bool getCanBeHeld()
    {
        return canBeHeld;
    }

    void Start()
    {
        GameObject[] stumbids = GameObject.FindGameObjectsWithTag("Stumbids");
        foreach(GameObject s in stumbids)
        {
            s.GetComponent<AIStumbids>().addInteractiblePoint(this.gameObject);
        }
    }

    void OnDestroy()
    {
        GameObject[] stumbids = GameObject.FindGameObjectsWithTag("Stumbids");
        foreach (GameObject s in stumbids)
        {
            s.GetComponent<AIStumbids>().removeInteractiblepoint(this.gameObject);
        }
    }
}
