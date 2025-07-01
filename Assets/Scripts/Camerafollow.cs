using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // プレイヤーのTransformを参照
    public Vector3 offset = new Vector3(0, 5, -10);  // カメラのオフセット（位置）
    public float smoothSpeed = 0.125f;  // 追従のスムーズさ

    void LateUpdate()
    {
        // プレイヤーの位置にオフセットを加えた目標位置
        Vector3 desiredPosition = player.position + offset;

        // 目標位置に向かってカメラが滑らかに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // カメラの位置を更新
        transform.position = smoothedPosition;

        // カメラが常にプレイヤーを向くようにする
        transform.LookAt(player);
    }
}
