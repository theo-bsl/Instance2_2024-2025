using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _topPlayers;
    [SerializeField] private string _apiUrl;

    void Start()
    {
        _apiUrl = $"http://localhost/Test/GetScores.php";
        StartCoroutine(GetUsername());
    }

    public void ShowTopPlayers()
    {
        StartCoroutine(GetUsername());
    }

    private IEnumerator GetUsername()
    {
        UnityWebRequest request = UnityWebRequest.Get(_apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Erreur de connexion " + request.error);
        }
        else if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log(json);
            ScoreList scoresPlayer = JsonUtility.FromJson<ScoreList>(json);

            if (scoresPlayer.scores.Length != 0)
            {
                foreach (Score score in scoresPlayer.scores)
                {
                    Debug.Log(score.username);
                    _topPlayers.text += $"{score._rank}.  {score.username}  {score._score} \n";
                }
            }
            else
            {
                _topPlayers.text = "Any score registered yet";
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
