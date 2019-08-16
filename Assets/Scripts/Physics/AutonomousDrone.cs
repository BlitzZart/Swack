using UnityEngine;

public class AutonomousDrone : Follower {
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float strength;
    [SerializeField]
    private float targetAttraction;
    [SerializeField]
    private float weakenWhenCloseFactor;

    public float maxPower = 400;

    public bool drawPath_Debug = false;
    public bool drawVelocity_Debug = false;

    public bool showSensor;

    private Sensor m_sensor;
    private float m_sensorRadius;
    private Vector3 m_lastPosition;
    private Vector3 smoothTrailPoint;

    // used to generate diveristy
    private float rndOffsetAcceleration;

    protected override void Start () {
        base.Start();

        // standard assignment with only one leader in scene
        if (m_attractor == null)
            m_attractor = FindObjectOfType<Leader>().transform;


        m_sensor = GetComponentInChildren<Sensor>();
        m_sensorRadius = m_sensor.GetComponent<SphereCollider>().radius * m_sensor.transform.localScale.x;

        m_lastPosition = smoothTrailPoint = transform.position;

        if (!showSensor)
            m_sensor.GetComponent<Renderer>().enabled = false;
        rndOffsetAcceleration = UnityEngine.Random.Range(-2.0f, 2.0f);

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

    private void UpdateForces()
    {
        float neighborAvoidPrw = 4; // Math.Pow(..., neighborAvoidPrw)
        float neighborAvoidMult = -0.013f;
        if (heightFixed)
        {
            neighborAvoidPrw = 4;
            neighborAvoidMult = -0.013f;
        }

        targetAttraction = 0;
        Vector3 repulseDir = Vector3.zero;

        foreach (Transform item in m_sensor.CloseEntities)
        {
            Vector3 dirFollowerer = item.transform.position - transform.position;
            float mag = dirFollowerer.magnitude;
            if (mag < (2 * m_sensorRadius))
            {
                repulseDir += dirFollowerer.normalized * Mathf.Pow((mag - 2 * m_sensorRadius), neighborAvoidPrw) * neighborAvoidMult;
                strength = repulseDir.magnitude;
            }
        }

        float targetDistance = Vector3.Distance(m_attractor.transform.position, transform.position);

        repulseDir += (m_attractor.transform.position - transform.position).normalized;

        // weaken attraction to target if closer than x meter
        weakenWhenCloseFactor = Mathf.Clamp(targetDistance / 5.0f, 0.5f, 1.0f);

        // minTargetAttraction helps to approach faster
        float minTargetAttraction = 0;
        if (targetDistance > 0.5f)
        {
            minTargetAttraction = 20.0f;
        }

        targetAttraction += Mathf.Max(targetDistance * 25 * weakenWhenCloseFactor + rndOffsetAcceleration, minTargetAttraction);

        m_body.AddForce(repulseDir * Mathf.Min(targetAttraction, maxPower) * Time.deltaTime );
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
                m_attractor = l.transform;
        }
        // founde one
        if (m_attractor)
            return;

        foreach (Leader l in leaders) {
            if (l.ID == 0)
                m_attractor = l.transform;
        }
    }
}
