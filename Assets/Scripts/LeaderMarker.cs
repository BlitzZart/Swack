using UnityEngine;

/// <summary>
/// LeaderMarker enables selecting and manipulating leader objects
/// </summary>
public class LeaderMarker : MonoBehaviour {
    public delegate void LeaderDelegate(Leader target);
    public static event LeaderDelegate LeaderMarkedEvent;

    public delegate void LeaderDragger(Vector3 target);
    public static event LeaderDragger LeaderDraggedEvent;
    private Vector3 dragStartPos;

    public LayerMask layerMask;

    private Leader markedLeader;

    private void Update () {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0) && markedLeader) {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(r.origin, r.direction * 1000, Color.green);

            RaycastHit hi = new RaycastHit();
            // 13 = Leader Layer Mask
            Physics.Raycast(r, out hi, 10000, layerMask);

            if (hi.transform == null)
                return;

            Vector3 t = (hi.point);
            t = Camera.main.transform.worldToLocalMatrix * t;
            t.z = (Camera.main.transform.worldToLocalMatrix * dragStartPos).z;
            print("--- " + t);

            LeaderDraggedEvent(Camera.main.transform.localToWorldMatrix * t);
        }

        if (!Input.GetMouseButtonDown(0)) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.black, 1);

        RaycastHit hitInfo = new RaycastHit();
        // 13 = Leader Layer Mask
        Physics.Raycast(ray, out hitInfo, 10000, layerMask);


        if (hitInfo.collider == null) {
            LeaderMarkedEvent(null);
            return;
        }
        dragStartPos = hitInfo.point;

        markedLeader = hitInfo.collider.GetComponent<Leader>();
        LeaderMarkedEvent(markedLeader);
	}
}
