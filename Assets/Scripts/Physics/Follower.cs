using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    protected float m_rndAlpha;

    protected Transform m_attractor;
    protected Rigidbody m_body;

    public bool heightFixed = false;
    public int ID;

    protected virtual void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_rndAlpha = 1.0f;// Random.Range(1.0f, 1.1f); // actually unused
    }

    public void SetAttractor(Leader leader)
    {
        m_attractor = leader.transform;
    }

    public void FixHeight(bool fix)
    {
        if (fix)
        {
            heightFixed = true;
            m_body.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            heightFixed = false;
            m_body.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

}
