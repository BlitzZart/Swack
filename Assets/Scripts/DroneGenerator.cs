using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneGenerator : Follower {
    static bool heightIsFixed = true;
    static bool directedDrones = false;
    static bool singleTarget = false;

    public Leader leitHammel;

    private List<Follower> drones;
    private List<Leader> leaders;
    private static System.Random shuffleRnd = new System.Random();

    public static int COLUMS = 0;
    public static int ROWS = 0;

    public Follower dronePrefab;
    public Leader leaderPrefab;
    public int colums = 10;
    public int rows = 10;
    public float offset = 1.5f;

    public Vector3 startPos;
    Vector3 nextPos;

	void Start () {
        if (heightIsFixed) {
            dronePrefab.heightFixed = true;
            Leader[] leaders = FindObjectsOfType<Leader>();

            foreach (Leader leader in leaders) {
                leader.transform.position = new Vector3(leader.transform.position.x, 0, leader.transform.position.z);
            }


        } else {
            dronePrefab.heightFixed = false;
        }

        drones = new List<Follower>();
        leaders = new List<Leader>();
        if (COLUMS != 0) {
            colums = COLUMS;
        } else {
            COLUMS = colums;
        }
        if (ROWS != 0) {
            rows = ROWS;
        } else {
            ROWS = rows;
        }

        startPos = new Vector3(-colums / 2 * offset, 0, rows / 2 * offset);

        int idCount = 0;

        GameObject droneHolder = new GameObject();
        droneHolder.name = "DroneHolder";
        GameObject targetHolder = new GameObject();
        targetHolder.name = "TargetHolder";

        bool isCentralized = false;
        if (dronePrefab.GetType() == typeof(CentralizedDrone))
        {
            isCentralized = true;
        }

        CentralProcessor cp = FindObjectOfType<CentralProcessor>();

        for (int c = 0; c < colums; c++) {
            for (int r = 0; r < rows; r++) {
                Follower f = Instantiate(dronePrefab, startPos + nextPos, Quaternion.identity);
                f.transform.parent = droneHolder.transform;

                Leader l = Instantiate(leaderPrefab, startPos + nextPos, Quaternion.identity);
                l.transform.parent = targetHolder.transform;
                l.ID = idCount;
                l.name = "Leader " + l.ID.ToString();
                leaders.Add(l);
                if (!heightIsFixed)
                    l.transform.Translate(0, 10, 0);

                drones.Add(f);

                nextPos.z -= offset;
                f.ID = idCount++;
                f.name = "Drone " + f.ID.ToString();


                f.SetAttractor(l);

                if (isCentralized)
                {
                    CentralizedDrone cd = (CentralizedDrone)f;
                    cd.SetAttractor(l);
                    cp.AddDrone(cd);                 
                }
            }

            nextPos.z = 0;
            nextPos.x += offset;
        }
	}

    private void Update() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            Restart(2, 2);
        }
        else
        if (Input.GetKey(KeyCode.Alpha2)) {
            Restart(5, 5);
        }
        else
        if (Input.GetKey(KeyCode.Alpha3)) {
            Restart(10, 10);
        }
        else
        if (Input.GetKey(KeyCode.Alpha4)) {
            Restart(20, 10);
        }
        else
        if (Input.GetKey(KeyCode.Alpha5)) {
            Restart(20, 20);
        }
        else
        if (Input.GetKey(KeyCode.Alpha6)) {
            Restart(2, 200);
        }
        else
        if (Input.GetKey(KeyCode.Alpha7)) {
            Restart(25, 20);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ToggleSensorVisualization();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            ToggleTragetLineVisualization();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            ToggleFixedHeight();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            ShuffleLeaders();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleSingleMultiTargets();
        }
    }

    public void ToggleFixedHeight()
    {
        heightIsFixed = !heightIsFixed;
        foreach (Follower item in drones)
            item.FixHeight(heightIsFixed);
    }

    public void ToggleTragetLineVisualization()
    {
        foreach (Follower item in drones)
        {
            item.EnablePathDrawing(!item.drawPath);
        }
    }

    public void ToggleSensorVisualization()
    {
        foreach (AutonomousDrone item in drones)
        {
            item.EnableSensorRangeDrawing(!item.showSensor);
        }
    }

    public void ToggleSingleMultiTargets()
    {
        singleTarget = !singleTarget;
        if (singleTarget)
        {
            foreach(Follower f in drones)
            {
                f.SetAttractor(leitHammel);
            }
        }
        else
        {
            for (int i = 0; i < leaders.Count; i++)
            {
                drones[i].SetAttractor(leaders[i]);
            }
        }

               
    }

    public void ShuffleLeaders()
    {
        singleTarget = false;
        Shuffle(leaders);
        for (int i = 0; i < leaders.Count; i++)
        {
            drones[i].SetAttractor(leaders[i]);
        }
    }

    private void Shuffle(List<Leader> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = shuffleRnd.Next(n + 1);
            Leader value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void Restart(int c, int r) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            heightIsFixed = true;
        } else {
            heightIsFixed = false;
        }

        COLUMS = c;
        ROWS = r;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}