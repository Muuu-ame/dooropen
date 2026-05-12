using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public UnityEvent onDoorOpened;

    public void Open()
    {
        onDoorOpened?.Invoke();
    }
}
