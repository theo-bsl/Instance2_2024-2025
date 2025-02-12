using UnityEngine;
using UnityEngine.Events;

public class SelectionButton : MonoBehaviour
{
    private readonly UnityEvent _onButtonPressed = new();

    public void OnClicked()
    {
        _onButtonPressed.Invoke();
    }

    public UnityEvent OnButtonPressed => _onButtonPressed;
}
