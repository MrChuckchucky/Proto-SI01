using UnityEngine;
using System.Collections;

public class physiqueobj : MonoBehaviour
{
    Vector3 lastPos;
    Vector3 pos;

    [HideInInspector]
    public bool held;
    bool launched;
    Vector3 velocity;

    [SerializeField]
    float velocityFactor;
    float velocityFactInit;

    [SerializeField]
    int collisionBrake;

    [SerializeField]
    float airFriction;

    [SerializeField]
    float bounciness;

    [SerializeField]
    float taux;

    float posY;
    float lastposY;

    void Start()
    {
        velocityFactInit = velocityFactor;
    }

    void Update()
    {
        if(held)
        {
            launched = false;
            ChangePosition();
            velocityFactor = velocityFactInit;
        }
        else
        {
            if(!launched)
            {
                launched = true;
                CalcVelocity();
            }
            transform.position += velocity *-velocityFactor * Time.deltaTime;
            lastposY = posY;
            posY = transform.position.y;
            if(Mathf.Abs(lastposY - posY) <= taux && posY <= 0.8f)
            {
                velocity = Vector3.zero;
            }
        }


        if(velocityFactor>=0)
        {
            velocityFactor -= airFriction;
        }
        
    }

    void ChangePosition()
    {
        lastPos = pos;
        pos = transform.position;
    }

    void CalcVelocity()
    {
        velocity = lastPos - pos;
    }

    void OnCollisionEnter(Collision collision)
    {
        velocityFactor *= bounciness;
    }
}
