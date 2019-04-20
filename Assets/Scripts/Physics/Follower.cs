using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    protected float m_rndAlpha;

    protected Transform m_attractor;
    protected Rigidbody m_body;
    protected LineRenderer m_line;

    public bool drawPath = false;
    public bool drawLineToTarget = false;
    public bool heightFixed = false;
    public int ID;

    protected virtual void Start()
    {
        m_line = GetComponent<LineRenderer>();
        m_body = GetComponent<Rigidbody>();
        m_rndAlpha = Random.Range(0.9f, 1.1f); // actually unused
    }

    protected virtual void Update()
    {
        if (drawLineToTarget)
        {
            UpdateLineRenererTarget();
        }
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
    public void EnablePathDrawing(bool draw)
    {
        drawPath = draw;
        m_line.enabled = drawPath;
    }

    private void UpdateLineRenererTarget()
    {
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1, m_attractor.transform.position);
    }
}
