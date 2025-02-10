using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetGlobalScore : MonoBehaviour
{
    public TMP_Text _text;
    private string _apiUrl;

    public void ShowGlobalScore(string Pseudo)
    {
        StartCoroutine(GetUsername(Pseudo));
    }

    private IEnumerator GetUsername(string Username)
    {
        _apiUrl = $"http://localhost/Test/GlobalScore.php?username={Username}";

        UnityWebRequest request = UnityWebRequest.Get(_apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Erreur de connexion " + request.error);
        }
        else if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            try
            {
                Score globalScore = JsonUtility.FromJson<Score>(json);
                Debug.Log(globalScore._rank);
                if (globalScore._rank <= 100)
                {
                    _text.text = $"{globalScore._rank}.  {globalScore.username}  {globalScore._score} \n";
                }
                else
                {
                    _text.text = $"100+.  {globalScore.username}  {globalScore._score} \n";
                }
            }
            catch (Exception e)
            {
                _text.text = "Not ranked";
            }
        }
        else
        {
            Debug.Log("Error Unknow");
        }
    }

    [System.Serializable]
    class ScoreList
    {
        public Score[] scores;
    }

    [System.Serializable]
    class Score
    {
        public string n;

        public string sumScore;

        public string username;

        public int _rank
        {
            get
            {
                int.TryParse(n, out int result);
                return result;
            }
        }

        public int _score
        {
            get
            {
                int.TryParse(sumScore, out int result);
                return result;
            }
        }
    }
}

