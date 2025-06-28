using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // ---【変更点1：HP関連の変数を追加】---
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

    // プレイヤーにダメージを与える処理（これは前回と同じ）
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

    // ---【変更点3：ダメージを受ける関数を追加】---
    // この関数は他のスクリプト（例：プレイヤーの攻撃）から呼び出される
    public void TakeDamage(int damage)
    {
        // 現在のHPからダメージ量を引く
        currentHealth -= damage;

        Debug.Log(gameObject.name + " が " + damage + " のダメージを受けた！ 残りHP: " + currentHealth);

        // もしHPが0以下になったら
        if (currentHealth <= 0)
        {
            Die(); // 死亡処理を呼び出す
        }
    }

    // ---【変更点4：死亡処理の関数を追加】---
    private void Die()
    {
        Debug.Log(gameObject.name + " 死亡");

        // このゲームオブジェクト（敵自身）をシーンから削除する
        Destroy(gameObject);
    }
}