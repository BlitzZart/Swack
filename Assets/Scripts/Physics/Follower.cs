using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField]
    private float currentSpeed;

    public float maxPower = 1000;
    public bool heightFixed = false;
    public bool drawPath_Debug = false;
    public bool drawVelocity_Debug = false;
    public bool drawPath = false;

    public bool showSensor;
    private Leader m_leader;
    private Sensor m_sensor;
    private float m_sensorRadius;
    private Rigidbody m_body;
    private Vector3 m_lastPosition;
    private LineRenderer m_line;
    private Vector3 smoothTrailPoint;

    private float rndOffsetAcceleration;

    private void Start () {
        m_line = GetComponent<LineRenderer>();
        m_leader = FindObjectOfType<Leader>();
        m_sensor = GetComponentInChildren<Sensor>();
        m_sensorRadius = m_sensor.GetComponent<SphereCollider>().radius * m_sensor.transform.localScale.x;
        m_body = GetComponent<Rigidbody>();


        m_lastPosition = smoothTrailPoint = transform.position;

        if (!showSensor)
            m_sensor.GetComponent<Renderer>().enabled = false;
        rndOffsetAcceleration = Random.Range(0.0f, 10.0f);

        FixHeight(heightFixed);

        StartCoroutine(UpdateWithFixedHz(10));
	}
    private void Update() {
        // rotate body in velocity directions
        // !!! don't freeze rotations then  !!!
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_body.velocity), Time.deltaTime * 100);

        if (drawVelocity_Debug)
            Debug.DrawRay(transform.position, m_body.velocity / 5, Color.black);
        if (drawPath_Debug) {
            Debug.DrawLine(transform.position, m_lastPosition, Color.gray, 0.66f);
        }
        if (drawPath) {
            UpdateLineRenerer();
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
        while (true) {
            float acc = 0;
            Vector3 dir = Vector3.zero;

            foreach (Transform item in m_sensor.CloseEntities) {
                Vector3 dirFollowerers = item.transform.position - transform.position;
                if (dirFollowerers.magnitude < (2 * m_sensorRadius)) {
                    dir += dirFollowerers.normalized * Mathf.Pow((dirFollowerers.magnitude - 2 * m_sensorRadius), neighborAvoidPrw) * neighborAvoidMult;
                }
            }
            dir += (m_leader.transform.position - transform.position).normalized;
            acc += (m_leader.transform.position - transform.position).magnitude * 25;

            m_body.AddForce(dir * Mathf.Min(acc, maxPower) * dt + Vector3.one * rndOffsetAcceleration);
            currentSpeed = m_body.velocity.magnitude;

            yield return new WaitForSeconds(dt);
        }
    }

    private void UpdateLineRenerer() {
        smoothTrailPoint = Vector3.Lerp(smoothTrailPoint, m_lastPosition + (transform.position - m_lastPosition), Time.deltaTime * 5);
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1,  smoothTrailPoint);

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
}
