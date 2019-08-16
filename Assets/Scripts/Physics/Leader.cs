using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// can be controlled via key input
/// is default target for drones (followers) - if not set to other tartet
/// </summary>
public class Leader : MonoBehaviour {

    private Color idleColor, markedColor;
    private Renderer leaderRenderer;
    private Vector3 oldPos;

    public int ID;
    public bool marked;

    private float speed = 25;

    void Start() {
        leaderRenderer = GetComponentInChildren<Renderer>();
        idleColor = leaderRenderer.material.color;

        markedColor = new Color(0, 1, 0, idleColor.a);
        LeaderMarker.LeaderMarkedEvent += OnMarked;
        LeaderMarker.LeaderDraggedEvent += OnDragged;
    }

    private void OnDestroy() {
        LeaderMarker.LeaderMarkedEvent -= OnMarked;
        LeaderMarker.LeaderDraggedEvent -= OnDragged;
    }

    private void Update() {
        if (!marked) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            oldPos = transform.position;
        }

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

    private void OnDragged(Vector3 target) {
        if (!marked) {
            return;
        }
        transform.position = target;
    }


    private void OnMarked(Leader target) {
        if (target == this) {
            if (!marked) {
                leaderRenderer.material.color = markedColor;
                marked = true;
            }
        }
        else {
            if (marked) {
                leaderRenderer.material.color = idleColor;
                marked = false;
            }
        }
    }
}