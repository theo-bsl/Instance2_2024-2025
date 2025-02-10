using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class SetNewScore : MonoBehaviour
{
    private int score;
    private int id;
    private int gameid;
    private string _apiUrl;

    [SerializeField] private PlayerInfos _playerInfos;

    private void Start()
    {
        id = _playerInfos.id;

    }

    public void SetNewScoreIntoDBB(int score)
    {
        StartCoroutine(SetNewScoreInDBB(score));
    }

    private IEnumerator SetNewScoreInDBB(int score)
    {
        _apiUrl = $"http://localhost/Test/SetNewScore.php?playerId={id}&score={score}&gameId={gameid}";

        using (UnityWebRequest request = UnityWebRequest.Get(_apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur de connexion : " + request.error);
            }
        }
    }
}
