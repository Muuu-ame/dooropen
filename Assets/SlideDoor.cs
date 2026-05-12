using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideDoor : MonoBehaviour
{
    public enum DoorDirection { Right, Left, Up, Down }
    [SerializeField] private DoorDirection allowedDirection;
    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveDuration = 1.0f;

    private Vector2 startPos;
    private Vector2 endPos;
    private bool isActive;
    private bool isMoving;
    private bool isOpened;
    private Vector3 moveDir;

    public Door doorComponent;

    void Update()
    {
        if (!isActive || isMoving || isOpened) return;
        HandleInput();
    }

    private void HandleInput()
    {
        Pointer pointer = Pointer.current;
        if (pointer == null) return;

        if (pointer.press.wasPressedThisFrame)
            startPos = pointer.position.ReadValue();
        if (pointer.press.wasReleasedThisFrame)
        {
            endPos = pointer.position.ReadValue();
            CheckSwipe();
        }
    }

    private void CheckSwipe()
    {
        Vector2 delta = endPos - startPos;
        if (delta.magnitude < minSwipeDistance) return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (allowedDirection == DoorDirection.Right && delta.x > 0) StartMoving(Vector3.right);
            else if (allowedDirection == DoorDirection.Left && delta.x < 0) StartMoving(Vector3.left);
        }
        else
        {
            if (allowedDirection == DoorDirection.Up && delta.y > 0) StartMoving(Vector3.up);
            else if (allowedDirection == DoorDirection.Down && delta.y < 0) StartMoving(Vector3.down);
        }
    }

    private void StartMoving(Vector3 dir)
    {
        moveDir = dir.normalized;
        isMoving = true;
        StartCoroutine(MoveDoor());
    }

    private IEnumerator MoveDoor()
    {
        float elapsed = 0f;
        Vector3 start = transform.position;
        Vector3 end = start + moveDir * moveSpeed * moveDuration;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isMoving = false;
        isOpened = true;
        doorComponent?.Open();
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
