// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarmify
{
    public class Follower : MonoBehaviour
    {
        protected float m_rndAlpha;

        protected Rigidbody m_body;
        protected LineRenderer m_line;

        public Transform Attractor;

        public bool drawPath = false;
        public bool drawLineToTarget = false;
        public bool heightFixed = false;
        public int ID;

        #region unity callbacks
        protected virtual void Start()
        {
            m_line = GetComponent<LineRenderer>();
            m_body = GetComponent<Rigidbody>();
            m_rndAlpha = Random.Range(0.9f, 1.1f); // actually unused
        }
        protected virtual void Update()
        {
            if (drawLineToTarget)
            {
                UpdateLineRenererTarget();
            }
        }
        #endregion

        #region public
        public void SetAttractor(Leader leader)
        {
            Attractor = leader.transform;
        }
        public void FixHeight(bool fix)
        {
            if (fix)
            {
                heightFixed = true;
                m_body.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }
            else
            {
                heightFixed = false;
                m_body.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        public void EnablePathDrawing(bool draw)
        {
            drawPath = draw;
            m_line.enabled = drawPath;
        }
        #endregion

        #region private
        private void UpdateLineRenererTarget()
        {
            if (Attractor == null)
            {
                return;
            }
            m_line.SetPosition(0, transform.position);
            m_line.SetPosition(1, Attractor.transform.position);
        }
        #endregion
    }
}