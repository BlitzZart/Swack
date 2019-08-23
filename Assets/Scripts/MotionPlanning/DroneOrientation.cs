// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using UnityEngine;

public class DroneOrientation : MonoBehaviour
{
    private AutonomousDrone drone;

    private void Start()
    {
        drone = GetComponentInParent<AutonomousDrone>();
    }

    private void Update()
    {
        if (drone.TargetDistance > 0.1f)
        {
            transform.LookAt(drone.Attractor);
        }
    }
}
