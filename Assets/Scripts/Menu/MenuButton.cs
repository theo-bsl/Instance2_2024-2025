using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    private readonly UnityEvent _onButtonPressed = new();

    public void OnClicked()
    {
        _onButtonPressed.Invoke();
    }

    public UnityEvent OnButtonPressed => _onButtonPressed;
}
