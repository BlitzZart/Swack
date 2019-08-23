// Swarmify
// Contact: blitzzartgames@gmail.com
// Author: Daniel Rammer

using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Scenes : MonoBehaviour
{
    public void LoadAutonomOmni()
    {
        SceneManager.LoadScene("Autonomous");
    }
    public void LoadAutonomDirect()
    {
        SceneManager.LoadScene("AutonomousDirected");
    }
    public void LoadCentralized()
    {
        SceneManager.LoadScene("Centralized");
    }
}
