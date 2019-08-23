using UnityEngine;
using System.Collections;

public class ScreenShooter : MonoBehaviour {

    HiResScreenShots hrss;

    void Start () {
        hrss = GetComponent<HiResScreenShots>();
	}
	
	void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl)) {
                print("Knips");
                ScreenCapture.CaptureScreenshot(ScreenShotName());
                //hrss.TakeHiResShot();
            }
        }
	}
    public static string ScreenShotName() {
        return string.Format("{0}/Screenshots/screen_{1}.png",
                             Application.dataPath,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
}
