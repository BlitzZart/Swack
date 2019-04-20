using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AutonomousDrone : Follower {
    [SerializeField]
    private float currentSpeed;
    //[SerializeField]
    //private double deltaTime;
    [SerializeField]
    private float strength;
    [SerializeField]
    private float targetAttraction;
    [SerializeField]
    private float weakenWhenCloseFactor;



    public float maxPower = 1000;

    public bool drawPath_Debug = false;
    public bool drawVelocity_Debug = false;
    public bool drawPath = false;
    public bool drawLineToTarget = false;

    public bool showSensor;

    private Sensor m_sensor;
    private float m_sensorRadius;
    private Vector3 m_lastPosition;
    private LineRenderer m_line;
    private Vector3 smoothTrailPoint;

    // used to generate diveristy
    private float rndOffsetAcceleration;

    protected override void Start () {
        base.Start();
        m_line = GetComponent<LineRenderer>();

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

        //StartCoroutine(UpdateWithFixedHz(50));
	}
    private void Update() {
        // rotate body in velocity directions
        // !!! don't freeze rotations then  !!!
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_body.velocity), Time.deltaTime * 100);

        if (drawVelocity_Debug)
            UnityEngine.Debug.DrawRay(transform.position, m_body.velocity / 5, Color.black);

        if (drawPath_Debug) {
            UnityEngine.Debug.DrawLine(transform.position, m_lastPosition, Color.gray, 0.66f);
        }

        if (drawLineToTarget) {
            UpdateLineRenererTarget();
        }
        else if (drawPath) {
            UpdateLineRenererTrail();
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
        // TODO: fix timestep - not active!

    }

    private IEnumerator UpdateWithFixedHz(float hz) {
        double dt = 1 / hz;

        float neighborAvoidPrw = 4; // Math.Pow(..., neighborAvoidPrw)
        float neighborAvoidMult = -0.005f;
        if (heightFixed) {
            neighborAvoidPrw = 4;
            neighborAvoidMult = -0.013f;
        }

        yield return new WaitUntil(() => m_sensor.CloseEntities != null);
        Stopwatch sw = new Stopwatch();
        while (true) {
            //deltaTime = sw.Elapsed.TotalSeconds;
            sw.Reset();
            sw.Start();
            float acc = 0;
            Vector3 dir = Vector3.zero;

            foreach (Transform item in m_sensor.CloseEntities) {
                Vector3 dirFollowerer = item.transform.position - transform.position;
                float mag = dirFollowerer.magnitude;
                if (mag < (2 * m_sensorRadius)) {
                    dir += dirFollowerer.normalized * Mathf.Pow((mag - 2 * m_sensorRadius), neighborAvoidPrw) * neighborAvoidMult;
                    strength = dir.magnitude;
                }
            }
            dir += (m_attractor.transform.position - transform.position).normalized;
            acc += (m_attractor.transform.position - transform.position).magnitude * 25 + rndOffsetAcceleration;

            m_body.AddForce(dir * Mathf.Min(acc, maxPower) * (float)dt);
            currentSpeed = m_body.velocity.magnitude;
            // TODO: fix timestep - not active!

            yield return new WaitUntil(() => sw.Elapsed.TotalSeconds >= dt);
        }
    }

    private void UpdateLineRenererTrail() {
        smoothTrailPoint = Vector3.Lerp(smoothTrailPoint, m_lastPosition + (transform.position - m_lastPosition), Time.deltaTime * 5);
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1,  smoothTrailPoint);

    }
    private void UpdateLineRenererTarget() {
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1, m_attractor.transform.position);

    }


    public void EnablePathDrawing(bool draw) {
        drawPath = draw;
        m_line.enabled = drawPath;
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
