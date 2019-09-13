// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Swarmify
{
    public class UI_ShowFPS : MonoBehaviour
    {
        private Text txt;
        private int drones;
        private FollowTransform followAttractor;

        private void Start()
        {
            StartCoroutine(InitializedDelayed());
            StartCoroutine(UpdateData());
        }

        private IEnumerator InitializedDelayed()
        {
            yield return 0;
            txt = GetComponent<Text>();
            DroneGenerator dg = FindObjectOfType<DroneGenerator>();
            drones = DroneGenerator.COLUMS * DroneGenerator.ROWS;

            followAttractor = FindObjectOfType<FollowTransform>();
        }

        private IEnumerator UpdateData()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);

                if (followAttractor.IsFollowing)
                    txt.text = "Camera Locked";
                else
                    txt.text = "Camera Free";
                txt.text += "\n" + drones + " Drones @ " + (1.0f / Time.smoothDeltaTime).ToString("0") + " FPS";
            }
        }
    }
}