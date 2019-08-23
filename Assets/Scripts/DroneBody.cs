using UnityEngine;

/// <summary>
/// only used to detect collisions
/// </summary>
public class DroneBody : MonoBehaviour {
    private SphereCollider sphere;
    public Renderer modelRenderer;

    private void OnCollisionEnter(Collision collision) {
        DroneBody d = collision.gameObject.GetComponentInChildren<DroneBody>();
        if (d != null) {
            print("WARNING: Collision occured!");
            modelRenderer.material.color = Color.red;
        }
    }
}