using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {
    private List<Transform> m_close;  
    private Leader m_leader;

    public List<Transform> CloseEntities {
        get {
            return m_close;
        }
    }

    private void Awake() {
        m_close = new List<Transform>();
    }

    private void OnTriggerEnter(Collider other) {
        Follower f = other.GetComponentInParent<Follower>();
        if (f != null) {
            m_close.Add(f.transform);
            return;
        }

        Leader l = other.GetComponentInParent<Leader>();
        if (l != null) {
            m_close.Add(l.transform);
        }

    }
    private void OnTriggerExit(Collider other) {


        Follower f = other.GetComponentInParent<Follower>();
        if (f != null) {
            m_close.Remove(f.transform);
            return;
        }

        Leader l = other.GetComponentInParent<Leader>();
        if (l != null) {
            m_close.Remove(l.transform);
        }
    }
}
