using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SkinSelection : MonoBehaviour
{
    [Header("Skins")]
    [SerializeField] private List<Image> _skinsImages;
    [SerializeField] private List<Sprite> _skinsSprites;

    [Header("Buttons")]
    [SerializeField] private SelectionButton _decreaseButton;
    [SerializeField] private SelectionButton _increaseButton;

    public int _currentSkin = 0;

    private void Start()
    {
        _decreaseButton.OnButtonPressed.AddListener(() => IncrementDecrementNumber(false));
        _increaseButton.OnButtonPressed.AddListener(() => IncrementDecrementNumber(true));

        EnableUnabledImages();
    }

    public void IncrementDecrementNumber(bool increment)
    {
        _currentSkin += increment ? 1 : -1;
        _currentSkin = (_currentSkin + 3) % 3;
        Debug.Log(_currentSkin);

        EnableUnabledImages();
    }

    private void OnDestroy()
    {
        _decreaseButton.OnButtonPressed.RemoveAllListeners();
        _decreaseButton.OnButtonPressed.RemoveAllListeners();
    }

    private void EnableUnabledImages()
    {
        for (int i = 0; i < _skinsImages.Count; i++)
        {
            if (i == _currentSkin)
            {
                _skinsImages[i].enabled = true;
            } 
            else
            {
                _skinsImages[i].enabled = false;
            }
        }
    }

    private void ApplySkin()
    {
        StartCoroutine(SetNewSkin());
    }

    IEnumerator SetNewSkin()
    { 
        UnityWebRequest request = UnityWebRequest.Get("https://192.168.1.226/skinSelector.php?skin=" + _currentSkin);
        yield return request.SendWebRequest();

        Debug.Log("Successfully set new skin into database");
    }
}
