// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using UnityEngine;

public class AutonomousDrone : Follower {
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxPower = 200;
    private float attractionStrength;
    private float weakenWhenCloseFactor;

    private float m_targetDistance;
    public float TargetDistance
        {
        get { return m_targetDistance; }
    }

    public bool drawPath_Debug = false;
    public bool drawVelocity_Debug = false;
    public bool showSensor;

    private Sensor m_sensor;
    private float m_sensorRadius;
    private Vector3 m_lastPosition;
    private Vector3 smoothTrailPoint;

    // used to generate diveristy
    private float rndAttractionFactor;

    #region unity callbacks
    protected override void Start () {
        base.Start();

        // standard assignment with only one leader in scene
        //if (m_attractor == null)
        //    m_attractor = FindObjectOfType<Leader>().transform;

        m_sensor = GetComponentInChildren<Sensor>();
        m_sensorRadius = m_sensor.GetComponent<SphereCollider>().radius * m_sensor.transform.localScale.x;

        m_lastPosition = smoothTrailPoint = transform.position;

        if (!showSensor)
            m_sensor.GetComponent<Renderer>().enabled = false;
        rndAttractionFactor = UnityEngine.Random.Range(-1.0f, 1.0f);

        FixHeight(heightFixed);
	}
    protected override void Update() {
        base.Update();

        if (drawVelocity_Debug)
            UnityEngine.Debug.DrawRay(transform.position, m_body.velocity / 5, Color.black);

        if (drawPath_Debug) {
            UnityEngine.Debug.DrawLine(transform.position, m_lastPosition, Color.gray, 0.66f);
        }

        UpdateForces();

        m_lastPosition = transform.position;
    }
    #endregion

    private void UpdateForces()
    {
        if (Attractor == null)
        {
            return;
        }
        float neighborAvoidPwr = 3;
        float neighborAvoidScale = -0.03f;

        attractionStrength = 0;
        Vector3 overallSum = Vector3.zero;

        // sum repulsive part
        foreach (Transform item in m_sensor.CloseEntities)
        {
            Vector3 dirFollowerer = item.transform.position - transform.position;
            float mag = dirFollowerer.magnitude;
            if (mag < (2 * m_sensorRadius))
            {
                overallSum += dirFollowerer.normalized * 
                    Mathf.Pow((Mathf.Clamp(m_sensorRadius - mag, 1, m_sensorRadius)), neighborAvoidPwr) *
                    neighborAvoidScale;
            }
        }

        m_targetDistance = Vector3.Distance(Attractor.transform.position, transform.position);

        // add attraction
        overallSum += (Attractor.transform.position - transform.position).normalized;

        // minTargetAttraction helps to approach faster when close (<1m)
        float minTargetAttraction = 0;
        if (m_targetDistance > 1.0f)
        {
            minTargetAttraction = 20.0f;
        }

        attractionStrength += Mathf.Max(m_targetDistance * 25.0f + rndAttractionFactor, minTargetAttraction);

        // move by force
        m_body.AddForce(overallSum * Mathf.Min(attractionStrength, maxPower) * Time.deltaTime );
        // limit speed, 0 = no speed limit
        if (maxSpeed != 0 && m_body.velocity.magnitude > maxSpeed)
        {
            m_body.velocity = m_body.velocity.normalized * maxSpeed;
        }

        // [expermimental] using transformation instead of applying force to a ridgib body
        //transform.Translate(Vector3.ClampMagnitude(repulseSum * Mathf.Min(attracgionSum, maxPower), maxSpeed) * Time.deltaTime);

        currentSpeed = m_body.velocity.magnitude;
    }

    public void EnableSensorRangeDrawing(bool draw) {
        showSensor = draw;
        m_sensor.GetComponent<Renderer>().enabled = showSensor;
    }

    /// <summary>
    /// sets drone to traget with same ID if available
    /// </summary>
    /// <param name="f"></param>
    public void HardcodedCustomLeaderAssignment() {
        Leader[] leaders = FindObjectsOfType<Leader>();

        foreach (Leader l in leaders) {
            if (l.ID == ID)
                Attractor = l.transform;
        }
        // founde one
        if (Attractor)
            return;

        foreach (Leader l in leaders) {
            if (l.ID == 0)
                Attractor = l.transform;
        }
    }
}
