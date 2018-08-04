using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneGenerator : MonoBehaviour {
    static bool heightIsFixed = true;
    private List<Follower> drones;

    public static int COLUMS = 0;
    public static int ROWS = 0;

    public Follower prefab;
    public int colums = 10;
    public int rows = 10;
    public float offset = 1.5f;

    public Vector3 startPos;
    Vector3 nextPos;

	void Start () {
        if (heightIsFixed) {
            prefab.heightFixed = true;
            Transform leader = FindObjectOfType<Leader>().transform;
            leader.position = new Vector3(leader.position.x, 0, leader.position.z);
        } else {
            prefab.heightFixed = false;
        }

        drones = new List<Follower>();
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

		for (int c = 0; c < colums; c++) {
            for (int r = 0; r < rows; r++) {
                drones.Add(Instantiate(prefab, startPos + nextPos, Quaternion.identity));
                nextPos.z -= offset;
            }

            nextPos.z = 0;
            nextPos.x += offset;
        }
	}

    private void Update() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            Restart(1, 1);
        }
        else
        if (Input.GetKey(KeyCode.Alpha2)) {
            Restart(2, 2);
        }
        else
        if (Input.GetKey(KeyCode.Alpha3)) {
            Restart(5, 5);
        }
        else
        if (Input.GetKey(KeyCode.Alpha4)) {
            Restart(10, 10);
        }
        else
        if (Input.GetKey(KeyCode.Alpha5)) {
            Restart(20, 20);
        }
        else
        if (Input.GetKey(KeyCode.Alpha6)) {
            Restart(40, 10);
        }
        else
        if (Input.GetKey(KeyCode.Alpha7)) {
            Restart(25, 20);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            foreach (Follower item in drones) {
                item.EnablePathDrawing(!item.drawPath);
            }
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            foreach (Follower item in drones) {
                item.EnableSensorRangeDrawing(!item.showSensor);
            }
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            heightIsFixed = !heightIsFixed;
            foreach (Follower item in drones)
                item.FixHeight(heightIsFixed);
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
