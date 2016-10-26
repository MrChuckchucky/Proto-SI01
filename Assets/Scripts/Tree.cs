using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{
    [SerializeField]
    GameObject flamedBranch;

    GameObject instFlamedBranch;

    public bool Ablazed;

	
    public void GiveBranch(GameObject Receiver)
    {
        
        if (Ablazed)
        {
            instFlamedBranch = Instantiate(flamedBranch, Receiver.transform.position+ new Vector3 (0f,0f,0.1f), Receiver.transform.rotation,Receiver.transform) as GameObject;
            Receiver.GetComponent<AIStumbids>().holdingItem = instFlamedBranch;
        }
    }

}

