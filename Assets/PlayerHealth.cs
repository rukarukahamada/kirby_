using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // プレイヤーの最大HP
    public int maxHealth = 100;
    // 現在のHP
    public int currentHealth;

    // ゲーム開始時に呼ばれる関数
    void Start()
    {
        // HPを最大値で初期化する
        currentHealth = maxHealth;
        Debug.Log("HP初期化 現在HP: " + currentHealth);
    }

    // ダメージを受けてHPを減らす関数
    // ご要望の通り、関数名を「HP」にしています。
    public void HP(int damageAmount)
    {
        // 現在のHPからダメージ量を引く
        currentHealth -= damageAmount;

        // HPがマイナスにならないように調整
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // HPが減ったことをコンソールに表示
        Debug.Log("プレイヤーがダメージを受けた！ 残りHP: " + currentHealth);

        // もしHPが0になったら
        if (currentHealth <= 0)
        {
            Debug.Log("プレイヤーは力尽きた...");
            // ここにゲームオーバー処理などを書く
            // 例：gameObject.SetActive(false); // プレイヤーを非表示にする
        }
    }
}