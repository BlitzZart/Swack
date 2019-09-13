// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using UnityEngine;
using UnityEngine.UI;

namespace Swarmify
{
    public class UI_Manager : MonoBehaviour
    {

        public Button sensorButton;

        private DroneGenerator m_droneGenerator;
        private MaskableGraphic[] m_texts;
        private bool m_showUI = true;
        private void Start()
        {
            m_texts = GetComponentsInChildren<MaskableGraphic>();
            m_droneGenerator = FindObjectOfType<DroneGenerator>();

            if (m_droneGenerator.dronePrefab.GetType() == typeof(CentralizedDrone))
            {
                sensorButton.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                m_showUI = !m_showUI;
                foreach (MaskableGraphic item in m_texts)
                    item.enabled = m_showUI;
            }
        }

        public void QuitApp()
        {
            Application.Quit();
        }
    }
}