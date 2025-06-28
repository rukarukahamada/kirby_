using UnityEngine;
using System.Collections;

public class E_beas : MonoBehaviour
{
    // HP関連の変数を追加
    [Header("ステータス")]
    public int maxHealth = 30; // 敵の最大HP（インスペクターから変更可能）
    private int currentHealth;  // 現在のHP

    [Header("攻撃設定")]
    public int attackDamage = 10; // プレイヤーに与えるダメージ

    // ---【変更点2：Start関数でHPを初期化】---
    void Start()
    {
        // ゲーム開始時にHPを最大値で初期化する
        currentHealth = maxHealth;
        // 分かりやすいように、どの敵が初期化されたか名前も表示
        Debug.Log(gameObject.name + " HP初期化 現在HP: " + currentHealth);
    }

    // プレイヤーにダメージを与える処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.HP(attackDamage);
            }
        }
    }

    // ダメージを受ける関数
    // この関数は他のスクリプト（例：プレイヤーの攻撃）から呼び出される
    public void TakeDamage(int damage)
    {
        StartCoroutine(DieAnimation());
        // 現在のHPからダメージ量を引く
        currentHealth -= damage;

        Debug.Log(gameObject.name + " が " + damage + " のダメージを受けた！ 残りHP: " + currentHealth);

        // もしHPが0以下になったら
        if (currentHealth <= 0)
        {
            Die(); // 死亡処理を呼び出す
        }
    }

    // 死亡処理の関数
    private void Die()
    {
        Debug.Log(gameObject.name + " 死亡");

        ///【変更点】すぐにDestroyするのではなく、点滅処理のコルーチンを開始する
        StartCoroutine(DieAnimation());
    }
    private IEnumerator DieAnimation()
    {
        // 1. まず当たり判定を無効にする
        // これをしないと、点滅中にプレイヤーが接触してダメージを受けたりする可能性がある
        GetComponent<Collider>().enabled = false;

        // 2. 点滅処理
        Renderer enemyRenderer = GetComponent<Renderer>(); // オブジェクトの見た目を管理するコンポーネント
        int blinkCount = 5;          // 点滅する回数（往復で1回）
        float blinkInterval = 0.1f;  // 点滅の間隔（秒）

        for (int i = 0; i < blinkCount; i++)
        {
            // オブジェクトの見た目を非表示にする
            enemyRenderer.enabled = false;
            // blinkInterval秒だけ処理を待つ
            yield return new WaitForSeconds(blinkInterval);

            // オブジェクトの見た目を表示する
            enemyRenderer.enabled = true;
            // 再びblinkInterval秒だけ処理を待つ
            yield return new WaitForSeconds(blinkInterval);
        }

        // 3. 点滅が終わったら、最後にオブジェクトをシーンから完全に削除する
        Destroy(gameObject);
    }
}