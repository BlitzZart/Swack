using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour {
    public Transform target;
    public Vector3 offset;

    public float camFollowSpeed = 3;
    public float camMoveSpeed = 3;

    public Vector3 manipulation;

    private void Start() {
        offset = transform.position - target.position;
        manipulation = Vector3.zero;
    }

    void Update() {
        transform.position = Vector3.Lerp(transform.position, target.position + offset + manipulation, camFollowSpeed * Time.deltaTime);
        transform.LookAt(target.position);

        if (Input.GetKey(KeyCode.UpArrow)) {
            Camera.main.fieldOfView -= Time.deltaTime * 25;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            Camera.main.fieldOfView += Time.deltaTime * 25;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            manipulation.x -= camMoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            manipulation.x += camMoveSpeed * Time.deltaTime;
        }
    }
}