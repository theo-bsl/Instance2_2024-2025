using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private Canvas _creditsScreen;
    [SerializeField] private Canvas _tutorialScreen;

    [Header("Tuto Pages")]

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
    }

    public void ShowHideCredits(bool show)
    {
        _creditsScreen.enabled = show;
    }

    public void ShowHideTutorial(bool show)
    {
        _tutorialScreen.enabled = show;
    }

    public void LeftRightButton()
    {
        if (!_tutorialScreen.active)
            return;    }
}
