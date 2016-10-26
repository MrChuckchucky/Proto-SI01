using UnityEngine;
using System.Collections;

public class InteractibleItem : MonoBehaviour
{
    [SerializeField]
    float interactionTime;
    [SerializeField]
    bool canBeHeld;

    float waitingTime = 0.0f;

    public enum type { tree, rock, sheep };
    public type objectType;

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
        if (this.gameObject.tag == "Bear")
        {
            waitingTime = 1.0f;
        }
        StartCoroutine(wait());
    }

    void OnDestroy()
    {
        InteractibleManager.instance.removeInteractible(this.gameObject);
    }

    public void Interaction(GameObject interacter)
    {

        switch (objectType)
        {
            case type.tree:
                GetComponent<Tree>().GiveBranch(interacter);
                break;
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitingTime);
        InteractibleManager.instance.addInteractible(this.gameObject);
    }
}
