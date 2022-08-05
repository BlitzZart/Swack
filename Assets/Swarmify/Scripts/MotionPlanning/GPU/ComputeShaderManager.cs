// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using Swarmify;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderManager : MonoBehaviour
{
    public ComputeShader computeShader;

    [Tooltip("Force based movement utilized rigidbodies and results in smoother motion.")]
    [SerializeField] private bool _useForce = true;

    // use those to tweak motion behavior
    [SerializeField, Range(0.0f, 20.0f)]
    private float maxMovementSpeed = 5.0f; // 8 m/s are 36kph and ~22mph. only correct if _useForce is false
    [SerializeField, Range(1.0f, 7.0f), Tooltip("This is the exponent in the repulsive part of the equation.")]
    private float repulsionFarPower = 3.0f;
    [SerializeField, Range(1.0f, 7.0f), Tooltip("This is the exponent in the repulsive part of the equation.")]
    private float repulsionClosePower = 5.0f;
    [SerializeField, Range(0.01f, 3.0f)]
    private float repulsionScale = 0.1f;
    [SerializeField, Range(0.0f, 40.0f)]
    private float sensingDistance = 20.0f;
    private bool fixHeight = false;

    public struct Drone
    {
        public Drone(Vector3 dronePos, Vector3 attrPos)
        {
            this.dronePos = dronePos;
            this.attrPos = attrPos;
        }
        public void UpdateAttractor(Vector3 attrPos)
        {
            this.attrPos = attrPos;
        }
        public Vector3 dronePos;
        public Vector3 attrPos;
    }

    private Drone[] _drones = new Drone[]{ };
    private CentralProcessor _centralProcessor;

    private List<Rigidbody> _bodies = new List<Rigidbody>();

    private void Start()
    {
        _centralProcessor = FindObjectOfType<CentralProcessor>();
    }

    private void FixedUpdate()
    {
        if (_centralProcessor.Mode == ComputationMode.GPU)
        {
            if (_centralProcessor.Drones.Count == 0)
            {
                return;
            }

            fixHeight = DroneGenerator.heightIsFixed;

            if (_centralProcessor.Drones.Count != _drones.Length)
            {
                List<Drone> droneList = new List<Drone>();
                foreach (CentralizedDrone drone in _centralProcessor.Drones)
                {
                    droneList.Add(new Drone(drone.transform.position, drone.Attractor.position));
                    _bodies.Add(drone.GetComponent<Rigidbody>());
                }

                _drones = droneList.ToArray();
            }

            UpdateMotion();
        }
    }
    private void UpdateMotion()
    {
        int size = sizeof(float) * 3 * 2;
        ComputeBuffer droneBuffer = new ComputeBuffer(_drones.Length, size);
        droneBuffer.SetData(_drones);

        computeShader.SetBuffer(0, "drones", droneBuffer);
        computeShader.SetFloat("maxMovementSpeed", maxMovementSpeed * Time.fixedDeltaTime);
        computeShader.SetFloat("repulsionFarPower", repulsionFarPower);
        computeShader.SetFloat("repulsionClosePower", repulsionClosePower);
        computeShader.SetFloat("repulsionScale", repulsionScale);
        computeShader.SetFloat("sensingDistance", sensingDistance);
        computeShader.SetBool("fixHeight", fixHeight);
        int groups = _drones.Length > 100 ? _drones.Length / 10 : _drones.Length;
        computeShader.Dispatch(0, groups, 1, 1);

        droneBuffer.GetData(_drones);

        for (int i = 0; i < _drones.Length; i++)
        {
            if (_useForce)
            {
                _bodies[i].AddForce((_drones[i].dronePos - _centralProcessor.Drones[i].transform.position) * 5.0f);
            }
            else
            {
                _centralProcessor.Drones[i].transform.position = _drones[i].dronePos;
            }

            _drones[i].UpdateAttractor(_centralProcessor.Drones[i].Attractor.position);
        }

        droneBuffer.Dispose();  
    }
}
