using System.Collections;
using UnityEngine;

public class Kirby_sleep : MonoBehaviour
{
    // 無効化フラグとタイマー
    private bool isFrozen = false;  // プレイヤーが無効化中かどうか
    private float frozenTime = 5f;  // 無効化する時間（秒）
    private float frozenTimer = 0f; // タイマー

    private bool isGrounded = false; // 地面に接しているかどうか
    private Rigidbody rb; // Rigidbodyコンポーネント

    // プレイヤーの色やRenderer
    private Renderer playerRenderer;
    private Color originalColor;
    private Color frozenColor = Color.green; // 冷却時の色（緑色）

    // HP管理用
    private Kirby_basic1 kirbyBasicScript;

    private float eKeyHoldTime = 0f;   // Eキー押し続け時間
    private float requiredHoldTime = 5f; // 押し続ける必要時間

    void Start()
    {
        // Rigidbody コンポーネントを取得
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        // プレイヤーの元の色を保存
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        // Kirby_basic1 スクリプトを取得
        kirbyBasicScript = GetComponent<Kirby_basic1>();
        if (kirbyBasicScript == null)
        {
            Debug.LogWarning("Kirby_basic1 スクリプトが見つかりません。HP回復は実行されません。");
        }
    }

    void Update()
    {
        // Eキーを押し続けている時間を計測
        if (Input.GetKey(KeyCode.E))
        {
            eKeyHoldTime += Time.deltaTime;

            if (eKeyHoldTime >= requiredHoldTime)
            {
                // 5秒間押し続けたら色をピンクに戻す
                playerRenderer.material.color = Color.magenta;
                eKeyHoldTime = 0f; // カウンターリセット（連続で何度も戻らないように）
            }
        }
        else
        {
            // Eキー離したらリセット
            eKeyHoldTime = 0f;
        }

        // 色が緑色になった場合にフリーズ開始
        if (playerRenderer.material.color == frozenColor && !isFrozen)
        {
            FreezePlayer();
        }

        // 無効化されていなければ通常の操作
        if (!isFrozen)
        {
            Move();
            Jump();
        }
        else
        {
            // 無効化中の場合、5秒経過後にフリーズ解除
            frozenTimer += Time.deltaTime;
            if (frozenTimer >= frozenTime)
            {
                // フリーズ解除
                isFrozen = false;
                frozenTimer = 0f;
                Debug.Log("Frozen time ended, player is active again.");

                //色が緑色（= フリーズ状態）だった場合のみ HP 全回復
                if (playerRenderer.material.color == frozenColor && kirbyBasicScript != null)
                {
                    kirbyBasicScript.Heal(100);
                    Debug.Log("HP fully restored because player was frozen (green).");
                }

                ResetColor(); // 色を元に戻す
            }
        }
    }

    void Move()
    {
        // 無効化されていない場合に移動
        if (!isFrozen)
        {
            float horizontal = Input.GetAxis("Horizontal"); // A/D or ←/→
            float vertical = Input.GetAxis("Vertical"); // W/S or ↑/↓
            Vector3 movement = new Vector3(horizontal, 0, vertical) * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    void Jump()
    {
        // ジャンプは無効化されていない場合のみ
        if (!isFrozen && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse); // ジャンプ力は他のスクリプトで設定されている前提
        }
    }

    // 無効化開始メソッド
    public void FreezePlayer()
    {
        isFrozen = true;
        frozenTimer = 0f; // タイマーリセット
        Debug.Log("Player is frozen for 5 seconds.");
        playerRenderer.material.color = frozenColor; // プレイヤーの色を緑に変更
    }

    // 色を元に戻すメソッド
    private void ResetColor()
    {
        playerRenderer.material.color = originalColor; // 元の色に戻す
    }

    // 地面に接しているかを確認するための判定
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }
}
