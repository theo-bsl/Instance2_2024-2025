using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

[RequireComponent (typeof(SpriteRenderer))]

public class SkinChanger : MonoBehaviour
{
    [SerializeField] private SkinSelection _selection;
    [SerializeField] private List<Sprite> _skins;
    private SpriteRenderer _spriteRenderer;
    private NetworkVariable<int> _skinIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public bool IsOwner { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // Load the skin when the value changes
        _skinIndex.OnValueChanged += (oldValue, newValue) => UpdateSkin(newValue);

        if (IsOwner)
        {
            StartCoroutine(GetSkin());
        }
    }

    IEnumerator GetSkin()
    {
        //UnityWebRequest request = UnityWebRequest.Get("https://192.168.1.226/GetDatas.php");
        //yield return request.SendWebRequest();

        //string json = request.downloadHandler.text;
        //PlayerInfos playerInfos = JsonUtility.FromJson<PlayerInfos>(json);

        //ChangeSkin(playerInfos.skinIndex);

        ChangeSkin(_selection._currentSkin);
        yield return null;

    }

    public void ChangeSkin(int index)
    {
        if (IsOwner)
        {
            _skinIndex.Value = index; // Sync across network
        }
    }

    private void UpdateSkin(int index)
    {
        if (index >= 0 && index < _skins.Count)
        {
            _spriteRenderer.sprite = _skins[_skinIndex.Value];
        }
    }
}
