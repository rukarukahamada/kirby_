using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        StartCoroutine(DieAnimation());
        
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
            Debug.Log("プレイヤー死亡");
            Die();
            SceneManager.LoadScene("GAMEOVER");

        }
    }
    // 死亡処理の関数
    private void Die()
    {
        Debug.Log(gameObject.name + " 死亡");

        ///【変更点】すぐにDestroyするのではなく、点滅処理のコルーチンを開始する
        StartCoroutine(DieAnimation());
        Destroy(gameObject);
        
        SceneManager.LoadScene("GAMEOVER");
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
        // ↑で消した当たり判定を元に戻す
        GetComponent<Collider>().enabled = true;



    }
}