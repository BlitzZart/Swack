using UnityEngine;

public class DroneBody : MonoBehaviour {
    private SphereCollider sphere;
    public Renderer modelRenderer;

    private void Start() {
        //modelRenderer.material.color = Color.cyan;
    }

    private void OnCollisionEnter(Collision collision) {
        DroneBody d = collision.gameObject.GetComponentInChildren<DroneBody>();
        if (d != null) {
            print("WARNING: Collision occured!");
            modelRenderer.material.color = Color.red;
        }
    }
}