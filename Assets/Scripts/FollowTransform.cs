using UnityEngine;

public class FollowTransform : MonoBehaviour {
    public Transform target;
    public Vector3 offset;
    public float camFollowSpeed = 3;
    public float camMoveSpeed = 3;
    public Vector3 manipulation;

    private bool m_isFollowing = true;
    public bool IsFollowing {
        get {
            return m_isFollowing;
        }
    }

    private float initFOV;

    private void Start() {
        initFOV = Camera.main.fieldOfView;
        FollowAttractor(false);
    }

    void Update() {
        if (!m_isFollowing)
            return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset + manipulation, camFollowSpeed * Time.deltaTime);
        transform.LookAt(target.position);

        if (Input.GetKey(KeyCode.W)) {
            if (Camera.main.fieldOfView > 3) {
                Camera.main.fieldOfView -= Time.deltaTime * 25;
            }
        }
        else if (Input.GetKey(KeyCode.S)) {
            if (Camera.main.fieldOfView < 100) {
                Camera.main.fieldOfView += Time.deltaTime * 25;
            }
        }

        if (Input.GetKey(KeyCode.A)) {
            manipulation.x -= camMoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D)) {
            manipulation.x += camMoveSpeed * Time.deltaTime;
        }
    }

    public void FollowAttractor(bool follow) {
        m_isFollowing = follow;
        offset = transform.position - target.position;

        Camera.main.fieldOfView = initFOV;
        if (follow) {
            manipulation = Vector3.zero;
        }
    }
}