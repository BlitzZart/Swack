using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralizedDrone : Follower
{
    [SerializeField]
    private Vector3 m_repulsion;
    private Vector3 m_attraction;
    private Transform m_attractor;

    public void AddRepulsor(Vector3 repulsorPosition)
    {
        Vector3 repDir = (repulsorPosition - transform.position);
        float repDist = Vector3.Distance(repulsorPosition, transform.position);
        m_repulsion = m_repulsion + (repDir / Mathf.Pow(repDist, 3));
    }

    public void SetAttractor(Transform attractor)
    {
        m_attractor = attractor;
    }

    public void ApplyResault()
    {
        Vector3 attDir = (m_attractor.position - transform.position).normalized;
        float attDist = Vector3.Distance(m_attraction, transform.position);
        m_attraction = attDir;///Mathf.Clamp(attDist, 0.0001f, 1000f);

        //m_attraction *= 10.0f;

        //print("VEL " + m_body.velocity);

        Vector3 force = m_attraction - m_repulsion;
        //force *= 0.01f;

        transform.Translate(force * Time.fixedDeltaTime);

        //m_body.AddForce(force);

        Debug.DrawRay(transform.position, force);
        // reset
        m_repulsion = Vector3.zero;
        m_attraction = Vector3.zero;
    }
}
