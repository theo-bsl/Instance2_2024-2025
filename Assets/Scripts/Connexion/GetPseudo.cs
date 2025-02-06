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
    public TMP_Text _text; 
    private string _apiUrl = "https://192.168.1.226/GetDatas.php";

     void Start()
    {
        StartCoroutine(GetUsername());
    }

    private  IEnumerator GetUsername()
    {
        //HttpClient client = new HttpClient();
        //HttpResponseMessage response = await client.GetAsync(_apiUrl);
        UnityWebRequest request = UnityWebRequest.Get(_apiUrl);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Erreur de connexion " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            UsernameResponse usernameResponse = JsonUtility.FromJson<UsernameResponse>(json);
            _text.text = usernameResponse.username;
        }
        
        //_text.text = await response.Content.ReadAsStringAsync();

        //yield return (re9+));

        //_text.text = "LAAAAAAAAAAAAAAAAAAAAAAAAAAA";


        //if (request.isNetworkError || request.isHttpError)
        //{
        //    _text.text = "Failed webrequest";
        //}

        //else if (request.result == UnityWebRequest.Result.Success)
        //{
        //    _text.text = Application.absoluteURL;
        //    string jsonResponse = request.downloadHandler.text;

        //    UsernameResponse response = JsonUtility.FromJson<UsernameResponse>(jsonResponse);

        //    if (!string.IsNullOrEmpty(response.username))
        //    {
        //        _text.text = response.username;
        //    }
        //    else
        //    {
        //        _text.text = "User not logged in";
        //    }
        //}
        //else
        //{
        //    _text.text = "Error fetching username";
        //}

    }

    [System.Serializable]
    class UsernameResponse
    {
        public string username;
    }
}
