using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Scenes : MonoBehaviour
{
    public void LoadAutonomOmni()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadAutonomDirect()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadCentralized()
    {
        SceneManager.LoadScene(2);
    }
}
