using UnityEngine;

public class DroneBody : MonoBehaviour {
    private SphereCollider sphere;
    public Collider[] allSpheres;

    private void Start() {
        GetComponentInChildren<Renderer>().material.color = Color.cyan;
    }

    private void OnCollisionEnter(Collision collision) {
        DroneBody d = collision.gameObject.GetComponentInChildren<DroneBody>();
        if (d != null) {
            print("WARNING: Collision occured!");
            GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }
}