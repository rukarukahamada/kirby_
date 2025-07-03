using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えのために追加

public class Kirby_basic1 : MonoBehaviour
{
    // 移動に関する変数
    public float Speed = 5f; // 移動速度
    public float JumpForce = 4f; // 初期ジャンプ力
    private bool isGrounded; // 地面にいるかどうかを判定
    private Rigidbody rb; // Rigidbody コンポーネント

    // HP管理に関する変数
    public int hp = 100;

    // ジャンプに関する変数
    private int jumpCount = 0; // 現在のジャンプ回数
    public int maxJumpCount = 10; // 最大ジャンプ回数
    private float currentJumpForce; // 現在のジャンプ力
    public float gravityMultiplier = 1f; // 重力補正値（ジャンプ後に落ちる速度を制御）
    private bool canFall = false; // 落下開始のフラグ
    private bool canJump = true; // ジャンプ可能かどうかを管理するフラグ
    public float maxJumpHeight = 10f; // 最大ジャンプ高さ

    // 点滅に関連する変数
    private Renderer playerRenderer;
    private Color originalColor; // 元のキャラクターの色
    private Coroutine blinkCoroutine; // プレイヤーのRenderer
    private bool isBlinking = false;
    private bool isInvincible = false;

    void Start()
    {
        // Rigidbody コンポーネントを取得
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        // 元の色を保存
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        currentJumpForce = JumpForce; // 初期ジャンプ力を設定
    }

    void Update()
    {
        // 移動処理
        Move();

        // ジャンプ処理（スペースキーでジャンプ）
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpCount < maxJumpCount) && canJump)
        {
            Jump();
        }

        // 重力補正（ジャンプ後に落ちる）
        if (canFall)
        {
            ApplyGravity();
        }

        // 任意で、ダメージや回復処理
        if (Input.GetKeyDown(KeyCode.H)) // Hキーで回復
        {
            Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.G)) // Gキーでダメージ
        {
            TakeDamage(10);
        }
    }

    void Move()
    {
        // WASDキーで移動
        float horizontal = Input.GetAxis("Horizontal"); // A/D or ←/→
        float vertical = Input.GetAxis("Vertical"); // W/S or ↑/↓
        Vector3 movement = new Vector3(horizontal, 0, vertical) * Speed * Time.deltaTime;
        transform.Translate(movement);
    }

    // ジャンプ処理
    void Jump()
    {
        // 最初の3回のジャンプ処理
        if (jumpCount < 3)
        {
            // 段階的にジャンプ力を増やし、maxJumpHeightに到達
            float jumpForceForThisJump = Mathf.Lerp(0, maxJumpHeight - transform.position.y, jumpCount / 3f);
            jumpCount++;
        }
        // 4回目から10回目まではmaxJumpHeightをキープ
        else if (jumpCount >= 3 && jumpCount <= 10)
        {
            // maxJumpHeightに到達したら、ジャンプ力を固定して高さをキープ
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            currentJumpForce = maxJumpHeight - transform.position.y; // maxJumpHeightに届かない場合にジャンプ力を調整
            jumpCount++;
        }
        // 11回目以降はジャンプ力が減少して落下
        else if (jumpCount > 10)
        {
            // 徐々にジャンプ力を減少させる
            currentJumpForce = Mathf.Lerp(currentJumpForce, 0f, 0.1f); // ジャンプ力を減少
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            jumpCount++;
        }

        // Y座標が maxJumpHeight を超えないように制限（位置と速度を両方制御）
        if (transform.position.y >= maxJumpHeight)
        {
            // Y軸方向の速度を手動で調整
            Vector3 velocity = rb.linearVelocity;
            velocity.y = Mathf.Min(velocity.y, 0); // 上方向の速度を0にキャンセル
            rb.linearVelocity = velocity;

            // 位置も maxJumpHeight で強制的に制限
            transform.position = new Vector3(transform.position.x, maxJumpHeight, transform.position.z);
        }
    }

    // 地面に接しているかを確認するための判定
    private void OnCollisionEnter(Collision collision)
    {
        // 地面と衝突した場合
        if (collision.collider.CompareTag("ground"))
        {
            isGrounded = true;
            jumpCount = 0; // 地面に接地したらジャンプ回数をリセット
            currentJumpForce = JumpForce; // ジャンプ力をリセット
            canFall = false; // 地面に着地したら落下フラグをリセット
            canJump = true; // 地面に着地したらジャンプ可能に戻す
        }

        if (collision.collider.CompareTag("enemy") && !isBlinking)
        {
            TakeDamage(10); // プレイヤーに10ダメージを与える
            blinkCoroutine = StartCoroutine(BlinkRenderer());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 地面から離れた場合
        if (collision.collider.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    // 重力補正を適用して落下させる
    void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

    // HPが減るメソッド
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    // プレイヤーが死んだ時の処理
    private void Die()
    {
        // シーンを切り替える処理
        SceneManager.LoadScene("GAMEOVER");
    }

    // HPを回復するメソッド
    public void Heal(int amount)
    {
        hp += amount;
        if (hp > 100) // 最大HPが100を超えないように
        {
            hp = 100;
        }
    }

    IEnumerator BlinkRenderer()
    {
        isBlinking = true;
        isInvincible = true;

        float blinkDuration = 2f;
        float timer = 0f;

        while (timer < blinkDuration)
        {
            if (playerRenderer != null)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
            }

            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }

        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }

        isBlinking = false;
        isInvincible = false;
    }
}
