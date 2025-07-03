using System.Collections;
using UnityEngine;

public class Kirby_falling : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isFalling;

    private Kirby_basic1 kirbyScript; // HP管理用スクリプトへの参照

    public int damageOnFall = 20; // 落下時に受けるダメージ量

    void Start()
    {
        startPosition = transform.position;
        isFalling = false;

        // 同じGameObjectにあるKirby_basicを取得
        kirbyScript = GetComponent<Kirby_basic1>();

        // 念のため、見つからなかったときにエラー表示
        if (kirbyScript == null)
        {
            Debug.LogError("Kirby_basic スクリプトが見つかりません！");
        }
        else
        {
            Debug.Log("Kirby_basic スクリプトを取得しました！");
        }

    }

    void Update()
    {
        if (transform.position.y < -10f && !isFalling)
        {
            isFalling = true;

            // HPを減らす処理を追加
            if (kirbyScript != null)
            {
                kirbyScript.TakeDamage(damageOnFall);
            }

            ResetPlayerPosition();
        }
    }

    void ResetPlayerPosition()
    {
        transform.position = startPosition;
        StartCoroutine(ResetFallingFlag());
    }

    IEnumerator ResetFallingFlag()
    {
        yield return new WaitForSeconds(0.5f);
        isFalling = false;
        Debug.Log("プレイヤーが落下してスタート地点に戻り、ダメージを受けました。");
    }
}
