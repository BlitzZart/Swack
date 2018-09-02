using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderMarker : MonoBehaviour {
    public delegate void LeaderDelegate(Leader target);
    public static event LeaderDelegate LeaderMarkedEvent;

    public LayerMask layerMask;

    private Leader ownLeader;

    private void Start() {
        ownLeader = GetComponent<Leader>();
    }

    private void Update () {
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.black, 1);


        RaycastHit hitInfo = new RaycastHit();
        // 13 = Leader Layer Mask
        Physics.Raycast(ray, out hitInfo, 10000, layerMask);

        if (hitInfo.transform == null)
            return;

        LeaderMarkedEvent(hitInfo.collider.GetComponent<Leader>());


	}
}
