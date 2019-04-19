using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralizedDrone : Follower
{
    [SerializeField]
    private Vector3 m_repulsion;
    [SerializeField]
    private Vector3 m_attraction;
    [SerializeField]
    private Transform m_attractor;

    public float attDist;

    public void AddRepulsor(Vector3 repulsorPosition)
    {
        Vector3 repDir = (repulsorPosition - transform.position);
        float repDist = Vector3.Distance(repulsorPosition, transform.position);
        m_repulsion = m_repulsion + (repDir / Mathf.Pow(repDist, 3) * 2.0f);
    }

    public void SetAttractor(Transform attractor)
    {
        m_attractor = attractor;
    }

    public void ApplyResault()
    {
        Vector3 attDir = (m_attractor.position - transform.position);
        attDist = Vector3.Distance(m_attractor.position, transform.position);
        m_attraction = attDir / (attDist);

        Vector3 forceVector = m_attraction - m_repulsion;

        if (!float.IsNaN(forceVector.x))
        {
            //m_body.AddForce(forceVector * 3);
            transform.Translate(forceVector * 3 * Time.fixedDeltaTime);
        }
        Debug.DrawRay(transform.position, forceVector);

        // reset
        m_repulsion = Vector3.zero;
        m_attraction = Vector3.zero;
    }
}
