using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
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

        // 点滅に関連する変数
        private Color originalColor; // 元のキャラクターの色
        private Renderer playerRenderer; // プレイヤーのRenderer

        void Start()
        {
            // Rigidbody コンポーネントを取得
            rb = GetComponent<Rigidbody>();
            playerRenderer = GetComponent<Renderer>();

            // 元の色を保存
            if (playerRenderer != null)
            {
                originalColor = playerRenderer.material.color;

                // ゲーム開始時にピンク色に設定
                playerRenderer.material.color = Color.red; // ピンク色（magenta）
            }

            currentJumpForce = JumpForce; // 初期ジャンプ力を設定
        }

        void Update()
        {
            // 移動処理
            Move(); // ここでMoveメソッドを呼び出す

            // ジャンプ処理（スペースキーでジャンプ）
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                Debug.Log("ボタンが押されました");
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

            if (Input.GetKeyDown(KeyCode.G))  // Gキーでダメージ
            {
                TakeDamage(10);
            }

            Debug.Log("isGrounded: " + isGrounded);  // 追加
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                Debug.Log("Space pressed! Jumping.");
                Jump();
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
            Debug.Log("Jump called");

            // isGrounded または maxJumpCount 未満でジャンプ可能
            if (isGrounded || jumpCount < maxJumpCount)
            {
                // 最初のジャンプ力（最大ジャンプ力に達するまでの力）
                if (jumpCount < 3)
                {
                    float jumpForceForThisJump = Mathf.Lerp(0, maxJumpHeight - transform.position.y, jumpCount / 3f);
                    Debug.Log("Jump Force for this jump: " + jumpForceForThisJump);  // 追加
                    rb.AddForce(Vector3.up * jumpForceForThisJump, ForceMode.Impulse);
                    jumpCount++;
                }
                // 4回目以降は一定のジャンプ力
                else if (jumpCount >= 3 && jumpCount <= 10)
                {
                    Debug.Log("Applying fixed jump force: " + currentJumpForce);  // 追加
                    rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
                    currentJumpForce = maxJumpHeight - transform.position.y;
                    jumpCount++;
                }
                // 11回目以降
                else if (jumpCount > 10)
                {
                    currentJumpForce = Mathf.Lerp(currentJumpForce, 0f, 0.1f);
                    Debug.Log("Applying reduced jump force: " + currentJumpForce);  // 追加
                    rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
                    jumpCount++;
                }

                // 落下処理を可能にする
                canFall = true;
                canJump = false;  // ジャンプ中はジャンプできないようにする
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 地面と接触した場合
            if (collision.collider.CompareTag("ground"))
            {
                isGrounded = true;
                jumpCount = 0; // ジャンプ回数をリセット
                currentJumpForce = JumpForce; // 初期ジャンプ力をリセット
                canFall = false;  // 地面に着地したので落下はしない
                canJump = true;  // ジャンプ可能
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
}

