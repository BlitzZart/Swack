using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    protected Transform m_target;
    protected Rigidbody m_body;

    public bool heightFixed = false;
    public int ID;

    protected virtual void Start()
    {
        m_body = GetComponent<Rigidbody>();
    }

    public void AssignLeader(Leader leader)
    {
        m_target = leader.transform;
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
