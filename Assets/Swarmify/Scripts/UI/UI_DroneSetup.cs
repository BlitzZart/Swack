// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using UnityEngine;
using UnityEngine.UI;

namespace Swarmify
{
    public class UI_DroneSetup : MonoBehaviour
    {
        public Toggle fixHight;

        public void Set4()
        {
            Setup(2, 2);
        }
        public void Set25()
        {
            Setup(5, 5);
        }
        public void Set100()
        {
            Setup(10, 10);
        }
        public void Set200()
        {
            Setup(20, 10);
        }
        public void Set400()
        {
            Setup(20, 20);
        }
        public void Set400line()
        {
            Setup(2, 200);
        }
        public void Set500()
        {
            Setup(25, 20);
        }

        private void Setup(int c, int r)
        {
            DroneGenerator dg = FindObjectOfType<DroneGenerator>();
            dg.Restart(c, r, fixHight.isOn);
        }

    }
}