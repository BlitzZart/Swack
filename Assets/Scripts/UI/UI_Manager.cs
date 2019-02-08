using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {
    private MaskableGraphic[] texts;
    private bool showUI = true;
    private void Start() {
        texts = GetComponentsInChildren<MaskableGraphic>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            showUI = !showUI;
            foreach (MaskableGraphic item in texts)
                item.enabled = showUI;
        }
    }
}
