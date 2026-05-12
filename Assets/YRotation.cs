using UnityEngine;

public class YRotation : MonoBehaviour
{
    [Header("回転速度 (度/秒)")]
    public float rotationSpeed = 90f; // 1秒で90度回転

    void Update()
    {
        // y軸を中心に回転
        transform.RotateAround(new Vector3(0f, 4f, 0f), Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
