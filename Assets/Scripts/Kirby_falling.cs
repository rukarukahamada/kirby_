using System.Collections;
using UnityEngine;

public class Kirby_falling : MonoBehaviour
{
    // スタート地点
    private Vector3 startPosition;

    // 落下したかどうかのフラグ
    private bool isFalling;

    void Start()
    {
        // スタート地点を記録
        startPosition = transform.position;
        isFalling = false;
    }

    void Update()
    {
        // プレイヤーが落下しているかどうかを判定（例：Y座標がある値以下なら落下）
        if (transform.position.y < -10f && !isFalling) // 例えばY座標が-10未満なら落下したと判定
        {
            isFalling = true;
            ResetPlayerPosition();
        }
    }

    // プレイヤーをスタート地点に戻す関数
    void ResetPlayerPosition()
    {
        // スタート地点にプレイヤーを戻す
        transform.position = startPosition;

        // ここで一定時間待ってからフラグをリセットする
        StartCoroutine(ResetFallingFlag());
    }

    // フラグをリセットするためのコルーチン
    IEnumerator ResetFallingFlag()
    {
        // 少し待機（例えば0.5秒）してからフラグをリセット
        yield return new WaitForSeconds(0.5f);
        isFalling = false;
        Debug.Log("プレイヤーが落下してスタート地点に戻りました。");
    }
}
