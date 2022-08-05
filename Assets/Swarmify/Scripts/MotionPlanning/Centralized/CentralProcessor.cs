// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using System.Collections.Generic;
using UnityEngine;

namespace Swarmify
{
    public enum ComputationMode
    {
        CPU, GPU
    };

    public class CentralProcessor : MonoBehaviour
    {
        public ComputationMode Mode { get => _mode; }
        [SerializeField] ComputationMode _mode = ComputationMode.GPU;

        private List<CentralizedDrone> _drones = new List<CentralizedDrone>();

        public List<CentralizedDrone> Drones { get => _drones; }


        public void AddDrone(CentralizedDrone drone)
        {
            _drones.Add(drone);
        }

        private void FixedUpdate()
        {
            if (_mode == ComputationMode.CPU)
            {
                // CPU brute force
                for (int o = 0; o < _drones.Count; o++)
                {
                    for (int i = 0; i < _drones.Count; i++)
                    {
                        if (o != i)
                        {
                            _drones[o].AddRepulsor(_drones[i].transform.position);
                        }
                    }
                    _drones[o].ApplyResault();
                }
            }
        }
    }
}