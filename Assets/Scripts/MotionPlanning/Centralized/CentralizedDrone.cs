// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using UnityEngine;

public class CentralizedDrone : Follower
{
    [SerializeField]
    private Vector3 m_repulsion;
    [SerializeField]
    private Vector3 m_attraction;

    protected override void Start()
    {
        base.Start();

        CentralProcessor cp = FindObjectOfType<CentralProcessor>();
        cp.AddDrone(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    public float attDist;

    public void AddRepulsor(Vector3 repulsorPosition)
    {
        Vector3 repDir = (repulsorPosition - transform.position);
        float repDist = Vector3.Distance(repulsorPosition, transform.position);
        m_repulsion = m_repulsion + (repDir / Mathf.Pow(repDist, 3) * 2.0f);
    }

    public void ApplyResault()
    {
        if (Attractor == null)
        {
            return;
        }

        Vector3 attDir = (Attractor.position - transform.position);
        attDist = Vector3.Distance(Attractor.position, transform.position);
        m_attraction = attDir / (attDist);

        float atTargetRepulsionUpscale = 1;
        float atTargetAttractionDownscale = 1;
        if (attDist < 1.0f)
        {
            atTargetRepulsionUpscale = Mathf.Clamp(1 / attDist, 1.0f, 1.5f);

            if (attDist < 0.5f)
            {
                atTargetAttractionDownscale = Mathf.Clamp(attDist / 2, 0.01f, 1.0f);
            }
        }

        Vector3 forceVector = (m_attraction - m_repulsion * m_rndAlpha * atTargetRepulsionUpscale) * atTargetAttractionDownscale;

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
