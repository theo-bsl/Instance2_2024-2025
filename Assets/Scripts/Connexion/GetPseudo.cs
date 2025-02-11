using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class GetPseudo : MonoBehaviour
{
    public TMP_Text _warning;
    public TMP_Text _text; 
    private string _apiUrl = "http://192.168.1.226/GetDatas.php";
    [SerializeField] private string _pseudo;
    [SerializeField] private GetGlobalScore _globalScore;

    void Start()
    {
        StartCoroutine(GetUsername());
    }

    private  IEnumerator GetUsername()
    {
        UnityWebRequest request = UnityWebRequest.Get(_apiUrl);
        yield return request.SendWebRequest();

        //if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        //{
        //    Debug.LogError("Erreur de connexion " + request.error);
        //}
        //else
        //{
        //    string json = request.downloadHandler.text;
        //    UsernameResponse usernameResponse = JsonUtility.FromJson<UsernameResponse>(json);
        //    _text.text = usernameResponse.username;
        //}

         if (request.result == UnityWebRequest.Result.Success)
        {
            _text.text = Application.absoluteURL;
            string jsonResponse = request.downloadHandler.text;

            UsernameResponse response = JsonUtility.FromJson<UsernameResponse>(jsonResponse);

            if (!string.IsNullOrEmpty(response.username))
            {
                _text.text = response.username;
                _pseudo = response.username;
                _globalScore.ShowGlobalScore(_pseudo);
            }
            else
            {
                _text.text = "User not logged in";
            }
        }
        else
        {
            _text.text = "Error fetching username";
        }

    }

    [System.Serializable]
    class UsernameResponse
    {
        public string username;
        public int id;
        public int skinChosen;
    }
}
