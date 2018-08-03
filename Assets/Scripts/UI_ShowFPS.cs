using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShowFPS : MonoBehaviour {
    private Text txt;
    private int drones;

    private void Start() {
        txt = GetComponent<Text>();
        DroneGenerator dg = FindObjectOfType<DroneGenerator>();
        drones = DroneGenerator.COLUMS * DroneGenerator.ROWS;
    }

    private void Update() {
        txt.text = drones + " Drones @ " + (1.0f / Time.smoothDeltaTime).ToString("0") + " FPS";
    }
}
