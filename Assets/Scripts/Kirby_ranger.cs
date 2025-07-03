using UnityEngine;

public class Kirby_ranger : MonoBehaviour
{
    public GameObject projectilePrefab; // 発射する球のプレハブ
    public Transform shotPoint;         // 発射位置
    public float shotForce = 700f;      // 発射力
    public float cooldownTime = 0.5f;   // クールタイム（秒）

    private float nextShotTime = 0f;    // 次に発射できる時刻

    // プレイヤーの色を管理（ここは簡単にRendererの色判定）
    private Renderer playerRenderer;

    private float eKeyHoldTime = 0f;   // Eキー押し続け時間
    private float requiredHoldTime = 5f; // 押し続ける必要時間

    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null)
        {
            Debug.LogWarning("Rendererコンポーネントが見つかりません。");
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

        if (playerRenderer == null) return;

        // プレイヤーの色が赤色か判定（赤の成分が強く、緑・青が弱いかどうか）
        Color playerColor = playerRenderer.material.color;
        bool isPlayerRed = playerColor.r > 0.8f && playerColor.g < 0.3f && playerColor.b < 0.3f;

        // プレイヤーが赤色の時だけQキーで射撃可能
        if (isPlayerRed && Input.GetKeyDown(KeyCode.Q) && Time.time >= nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + cooldownTime;
        }
    }


    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(shotPoint.forward * shotForce);
        }

        // プレイヤーの色が赤色か判定（ほぼ赤ならOK）
        bool isPlayerRed = false;
        if (playerRenderer != null)
        {
            Color playerColor = playerRenderer.material.color;
            // 赤色判定（赤の成分が強く、緑・青が弱いかどうか）
            isPlayerRed = playerColor.r > 0.8f && playerColor.g < 0.3f && playerColor.b < 0.3f;
        }

        // 発射したProjectileにProjectileCollisionをつけて、有効フラグを渡す
        ProjectileCollision pc = projectile.AddComponent<ProjectileCollision>();
        pc.isActive = isPlayerRed;

        Destroy(projectile, 3f);
    }
}

// ProjectileCollision側にフラグを用意して判定
public class ProjectileCollision : MonoBehaviour
{
    // trueのときのみ敵を消す処理を行う
    public bool isActive = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return; // 色が赤じゃないなら何もしない

        if (collision.gameObject.CompareTag("enemy_nodiy") || collision.gameObject.CompareTag("enemy_ba-na-do"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
