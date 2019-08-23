using UnityEngine;
using System.Collections;

public class HiResScreenShots : MonoBehaviour {

    public Camera camera;

    private bool takeHiResShot = false;

    [HideInInspector]
    public Texture2D thePainting;
    [HideInInspector]
    public RenderTexture thePaintingRT;

    public static string ScreenShotName(int width, int height) {
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    public void TakeHiResShot() {
        takeHiResShot = true;
    }
    void LateUpdate() {
        if (takeHiResShot) {
            RenderTexture rt = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);

            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, camera.pixelWidth, camera.pixelHeight), 0, 0);
            thePainting = screenShot;
            thePaintingRT = rt;

            // don't write image
            camera.targetTexture = null;
            RenderTexture.active = null;
            //Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(camera.pixelWidth, camera.pixelHeight);
            System.IO.File.WriteAllBytes(filename, bytes);

            Debug.Log(string.Format("Took screenshot to: {0}", camera.pixelHeight));
            takeHiResShot = false;
        }
    }
}
