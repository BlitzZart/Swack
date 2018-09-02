using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField]
    private float currentSpeed;

    public DateTime chck;

    public int ID;

    public float maxPower = 1000;
    public bool heightFixed = false;
    public bool drawPath_Debug = false;
    public bool drawVelocity_Debug = false;
    public bool drawPath = false;
    public bool drawLineToTarget = false;

    public bool showSensor;
    private Transform m_target;
    private Sensor m_sensor;
    private float m_sensorRadius;
    private Rigidbody m_body;
    private Vector3 m_lastPosition;
    private LineRenderer m_line;
    private Vector3 smoothTrailPoint;

    private float rndOffsetAcceleration;

    private void Start () {
        m_line = GetComponent<LineRenderer>();

        // standard assignment with only one leader in scene
        //if (m_target == null)
        //    m_target = FindObjectOfType<Leader>().transform;


        m_sensor = GetComponentInChildren<Sensor>();
        m_sensorRadius = m_sensor.GetComponent<SphereCollider>().radius * m_sensor.transform.localScale.x;
        m_body = GetComponent<Rigidbody>();


        m_lastPosition = smoothTrailPoint = transform.position;

        if (!showSensor)
            m_sensor.GetComponent<Renderer>().enabled = false;
        rndOffsetAcceleration = UnityEngine.Random.Range(0.0f, 10.0f);

        FixHeight(heightFixed);

        StartCoroutine(UpdateWithFixedHz(10));
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
        m_lastPosition = transform.position;
    }

    private IEnumerator UpdateWithFixedHz(float hz) {
        float dt = 1 / hz;

        float neighborAvoidPrw = 4; // Math.Pow(..., neighborAvoidPrw)
        float neighborAvoidMult = -0.005f;
        if (heightFixed) {
            neighborAvoidPrw = 4;
            neighborAvoidMult = -0.013f;
        }

        yield return new WaitUntil(() => m_sensor.CloseEntities != null);
        //Stopwatch sw = new Stopwatch();
        while (true) {
            //sw.Reset();
            //sw.Start();
            float acc = 0;
            Vector3 dir = Vector3.zero;

            foreach (Transform item in m_sensor.CloseEntities) {
                Vector3 dirFollowerer = item.transform.position - transform.position;
                float mag = dirFollowerer.magnitude;
                if (mag < (2 * m_sensorRadius)) {
                    dir += dirFollowerer.normalized * Mathf.Pow((mag - 2 * m_sensorRadius), neighborAvoidPrw) * neighborAvoidMult;
                }
            }
            dir += (m_target.transform.position - transform.position).normalized;
            acc += (m_target.transform.position - transform.position).magnitude * 25;

            m_body.AddForce(dir * Mathf.Min(acc, maxPower) * /*(float)*/dt + Vector3.one * rndOffsetAcceleration);
            currentSpeed = m_body.velocity.magnitude;
            // TODO: fix timestep - not active!
            yield return new WaitForSeconds(/*(float)(*/dt/* - sw.Elapsed.TotalSeconds)*/);
        }
    }

    private void UpdateLineRenererTrail() {
        smoothTrailPoint = Vector3.Lerp(smoothTrailPoint, m_lastPosition + (transform.position - m_lastPosition), Time.deltaTime * 5);
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1,  smoothTrailPoint);

    }
    private void UpdateLineRenererTarget() {
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1, m_target.transform.position);

    }

    public void FixHeight(bool fix) {
        if (fix) {
            heightFixed = true;
            m_body.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        } else {
            heightFixed = false;
            m_body.constraints = RigidbodyConstraints.FreezeRotation;
        }
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
                m_target = l.transform;
        }
        // founde one
        if (m_target)
            return;

        foreach (Leader l in leaders) {
            if (l.ID == 0)
                m_target = l.transform;
        }
    }

}
