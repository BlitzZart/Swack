// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using System.Collections.Generic;
using UnityEngine;

public class CentralProcessor : MonoBehaviour
{
    public List<CentralizedDrone> drones;

    public void AddDrone(CentralizedDrone drone)
    {
        drones.Add(drone);
    }

    private void FixedUpdate()
    {
        for (int o = 0; o < drones.Count; o++)
        {
            for(int i = 0; i < drones.Count; i++)
            {
                if (o != i)
                {
                    drones[o].AddRepulsor(drones[i].transform.position);
                }
            }
            drones[o].ApplyResault();
        }
    }
}
