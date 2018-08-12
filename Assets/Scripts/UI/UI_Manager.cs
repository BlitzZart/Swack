using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {
    private Text[] texts;
    private bool showUI = true;
    private void Start() {
        texts = GetComponentsInChildren<Text>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            showUI = !showUI;
            foreach (Text item in texts)
                item.enabled = showUI;
        }
    }
}
