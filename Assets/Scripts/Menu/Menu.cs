using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject _creditsScreen;
    [SerializeField] private GameObject _tutorialScreen;

    [Header("Tuto Pages")]
    [SerializeField] private List<GameObject> _pages;

    [Header("Buttons")]
    [SerializeField] private MenuButton _creditsButton;
    [SerializeField] private MenuButton _tutorialButton;
    [SerializeField] private MenuButton _quitCredits;
    [SerializeField] private MenuButton _quitTutorial;

    [SerializeField] private List<MenuButton> _leftButtons;
    [SerializeField] private List<MenuButton> _rightButtons;

    private int _currentTutoPage;

    private void Start()
    {
        _creditsButton.OnButtonPressed.AddListener(() => ShowHideCredits(true));
        _quitCredits.OnButtonPressed.AddListener(() => ShowHideCredits(false));

        _tutorialButton.OnButtonPressed.AddListener(() => ShowHideTutorial(true));
        _quitTutorial.OnButtonPressed.AddListener(() => ShowHideTutorial(false));
        
        foreach (MenuButton button in _leftButtons)
        {
            button.OnButtonPressed.AddListener(() => LeftRightButton(false));
        }

        foreach (MenuButton button in _rightButtons)
        {
            button.OnButtonPressed.AddListener(() => LeftRightButton(true));
        }
    }

    private void OnDestroy()
    {
        _creditsButton.OnButtonPressed.RemoveAllListeners();
        _quitCredits.OnButtonPressed.RemoveAllListeners();

        _tutorialButton.OnButtonPressed.RemoveAllListeners();
        _quitTutorial.OnButtonPressed.RemoveAllListeners();

        foreach (MenuButton button in _leftButtons)
        {
            button.OnButtonPressed.RemoveAllListeners();
        }

        foreach (MenuButton button in _rightButtons)
        {
            button.OnButtonPressed.RemoveAllListeners();
        }
    }

    public void ShowHideCredits(bool show)
    {
        _creditsScreen.SetActive(show);
        Debug.Log(show);
    }

    public void ShowHideTutorial(bool show)
    {
        _tutorialScreen.SetActive(show);
        if (show)
        {
            DisplayPage();
        }
        Debug.Log(show);
    }

    public void LeftRightButton(bool isRight)
    {
        if (_tutorialScreen.activeInHierarchy)
        {
            _currentTutoPage += isRight ? 1 : -1;
            _currentTutoPage = (_currentTutoPage + _pages.Count) % _pages.Count;
            Debug.Log(_currentTutoPage);
             DisplayPage();
        }
        Debug.Log(isRight);
    }

    public void DisplayPage()
    {
        for (int i = 0; i < _pages.Count; i++)
        {
            if (_currentTutoPage == i)
            {
                _pages[i].SetActive(true);
            }
            else
            {
                _pages[i].SetActive(false);
            }
            Debug.Log("called");
        }
    }
}
