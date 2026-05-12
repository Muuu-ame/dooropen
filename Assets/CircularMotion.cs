using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public Transform center;   // 回転の中心になるオブジェクト
    public float spinSpeed = 180f; // 回転速度（度/秒）

    void Update()
    {
        if (center != null)
        {
            // 中心を基準に、自分を回転させる（Z軸まわり）
            transform.RotateAround(center.position, Vector3.forward, spinSpeed * Time.deltaTime);
        }
    }
}
