 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour {

    private float speed = 25;

    void Start() {

    }

    void Update() {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.R)) {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        else
        if (Input.GetKey(KeyCode.F)) {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
    }
}