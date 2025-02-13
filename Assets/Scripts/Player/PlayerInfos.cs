using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfos : MonoBehaviour
{
    public string username;
    public int id;
    public int skinIndex;
    private static int index = 0;

    private string _apiUrl = "http://192.168.1.226/GetDatas.php";

    public void Name(PlayerShowInfoUI playerShowInfoUI)
    {
        // username = "player" + index;
        // index++;
        // transform.name = username;

        StartCoroutine(GetPlayerInfos(playerShowInfoUI));
    }

    private IEnumerator GetPlayerInfos(PlayerShowInfoUI playerShowInfoUI)
    {
        UnityWebRequest request = UnityWebRequest.Get(_apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            PlayerInfos response = JsonUtility.FromJson<PlayerInfos>(jsonResponse);

            if (!string.IsNullOrEmpty(response.username))
            {
                Debug.Log("Get Players Infos successfully");
                playerShowInfoUI.SetName(response.username);
            }
            else
            {
                Debug.Log("User not logged in");
            }
        }
        else
        {
            Debug.Log("Error fetching username");
        }
    }
}