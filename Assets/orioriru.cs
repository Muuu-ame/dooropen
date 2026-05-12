using UnityEngine;

public class orioriru : MonoBehaviour
{
    [Header("移動速度")]
    public float speed = 2f;      // 下に降りる速度
    public float targetY = 0f;    // 目標のY座標

    void Update()
    {
        Vector3 pos = transform.position;

        // 目標位置
        Vector3 targetPos = new Vector3(pos.x, targetY, pos.z);

        // 下にまだ余地があるなら移動
        if (pos.y > targetY)
        {
            transform.position = Vector3.MoveTowards(pos, targetPos, speed * Time.deltaTime);
        }
    }
}
