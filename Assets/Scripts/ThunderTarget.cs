using UnityEngine;
using System.Collections;

public class ThunderTarget : MonoBehaviour
{
    public enum type { tree, rock, sheep, stumbid };
    public type objectType;

   public void ThunderStrike ()
    {
        switch (objectType)
        {
            case type.tree:
                Ablaze();
                break;

            case type.sheep:
                Die();
                break;

            case type.stumbid:
                Die();
                break;
        }
    }
	
	void Ablaze()
    {
        GetComponent<Tree>().Ablazed = true;
    }

    void Die()
    {
        Destroy(this.gameObject);
        InteractibleManager.instance.stumbidSpwn = false;
    }
}
