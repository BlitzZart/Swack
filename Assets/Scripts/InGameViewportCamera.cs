using UnityEngine;

/// <summary>
/// A simple component which enables in-game navigation with a 3D camera
/// zoom, move up, down, left, right is applied locally
/// rotation is applied in worldspace
/// </summary>
public class InGameViewportCamera : MonoBehaviour {
    // key binding
    KeyCode upKey = KeyCode.W;
    KeyCode downKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;

    private FollowTransform followAttractor;

    float moveSpeed = 0.75f;
    
    float roationSpeed = 2.0f;
    float scaleSpeed = 20.0f;

    Vector3 startPosition;
    Quaternion startRotation;

    Quaternion qRotation = Quaternion.identity;

    void Start() {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        Initialize();

        followAttractor = GetComponent<FollowTransform>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftShift))
            return;

        Vector3 translation = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        // zoom
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
        translation += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * scaleSpeed);
        }
        if (Input.GetMouseButton(2)) {
            translation = new Vector3(0, 0, -Input.GetAxis("Mouse Y") * moveSpeed);
        }

        if (followAttractor != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                followAttractor.FollowAttractor(!followAttractor.IsFollowing);

                if (!followAttractor.IsFollowing)
                    Initialize();
            }

            if (!followAttractor.IsFollowing)
            {
                // translate
                if (Input.GetKey(upKey))
                {
                    translation += new Vector3(0, 0, moveSpeed) * Time.deltaTime * 50;
                }
                else if (Input.GetKey(downKey))
                {
                    translation += new Vector3(0, 0, -moveSpeed * Time.deltaTime * 50);
                }
                if (Input.GetKey(leftKey))
                {
                    translation += new Vector3(-moveSpeed * Time.deltaTime * 50, 0, 0);
                }
                else if (Input.GetKey(rightKey))
                {
                    translation += new Vector3(moveSpeed * Time.deltaTime * 50, 0, 0);
                }
            }
        }

        // drag
        if (Input.GetMouseButton(0)) {
            translation = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0) * moveSpeed;
        }
        // apply translation
        transform.Translate(translation);

        // rotate
        if (Input.GetMouseButton(1)) {
            rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * roationSpeed;

            // apply translation
            qRotation.eulerAngles += rotation;
            transform.rotation = qRotation;
        }
    }

    private void Initialize() {
        // initialize with current rotation
        qRotation = transform.rotation;
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
}