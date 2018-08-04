 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour {

    private float speed = 25;

    void Start() {

    }

    void Update() {

        if (Input.GetKey(KeyCode.UpArrow) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
            transform.Translate(0, speed * Time.deltaTime, 0); // UP
        }
        else if (Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(0, 0, speed * Time.deltaTime); // BACK
        }

        if (Input.GetKey(KeyCode.DownArrow) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
            transform.Translate(0, -speed * Time.deltaTime, 0);// DOWN
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(0, 0, -speed * Time.deltaTime);// FORWARD
        }


        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(-speed * Time.deltaTime, 0, 0); // LEFT
        }
        else
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(speed * Time.deltaTime, 0, 0); // RIGHT
        }

    }
}