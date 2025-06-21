using UnityEngine;

public class Kirby_basic : MonoBehaviour
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

    void Start()
    {
        // Rigidbody コンポーネントを取得
        rb = GetComponent<Rigidbody>();
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
        if (Input.GetKeyDown(KeyCode.H))  // Hキーで回復
        {
            Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.D))  // Dキーでダメージ
        {
            TakeDamage(10);
        }
    }

    // 移動処理
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
        if (jumpCount < 3)
        {
            // 最初の3回は通常通りジャンプ
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            jumpCount++;
        }
        else if (jumpCount >= 3 && transform.position.y < maxJumpHeight)
        {
            // 4回目以降は高さが maxJumpHeight を越えないように制限
            float targetHeight = Mathf.Min(maxJumpHeight, transform.position.y + currentJumpForce);
            if (transform.position.y < targetHeight)
            {
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            }
            jumpCount++;
        }

        // 10回までジャンプ力を一定にキープし、それ以降は徐々に下げる
        if (jumpCount >= maxJumpCount)
        {
            currentJumpForce = Mathf.Lerp(currentJumpForce, 0f, 0.1f); // 徐々にジャンプ力を減少
            canFall = true;  // 10回ジャンプ後は落下フラグを立てる
            canJump = false; // 10回ジャンプ後はジャンプできない
        }

        // 高さが maxJumpHeight を超えたら、制限
        if (transform.position.y >= maxJumpHeight)
        {
            // Y軸方向の速度を手動で調整
            Vector3 velocity = rb.linearVelocity;
            velocity.y = Mathf.Min(velocity.y, 0); // Y軸の速度が正なら0に設定（上方向の力をキャンセル）
            rb.linearVelocity = velocity;

            // 位置もmaxJumpHeightで強制的に制限
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
            canFall = false;  // 地面に着地したら落下フラグをリセット
            canJump = true;  // 地面に着地したらジャンプ可能に戻す
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
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    // プレイヤーが死んだ時の処理
    private void Die()
    {
        Debug.Log("Player has died.");
        // 死亡処理（例えば、ゲームオーバー画面を表示するなど）
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
}
