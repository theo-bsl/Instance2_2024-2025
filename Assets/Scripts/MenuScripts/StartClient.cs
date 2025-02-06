using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartClient : MonoBehaviour
{
    public void StartGame()
    {
        NetworkManager.Singleton.StartClient();
        SceneManager.LoadScene(1);
    }
}