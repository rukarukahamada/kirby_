using UnityEngine;

public class KirbySuction : MonoBehaviour
{
    public float suctionForce = 2f; // 吸い込む力の強さ
    public float maxDistance = 5f; // 吸い込む最大距離
    public float destroyDistance = 1f; // 吸い込むオブジェクトが消える距離
    public Transform playerTransform; // プレイヤーのTransform
    public GameObject suctionRangeColliderPrefab; // 吸い込む範囲のCollider（Prefab）
    public Color suctionColor = Color.blue; // 吸い込み時のキャラクターの色
    private Color originalColor; // 元のキャラクターの色

    private GameObject suctionRangeColliderInstance; // 実際に表示するColliderのインスタンス
    private Renderer playerRenderer; // プレイヤーのRenderer

    private void Start()
    {
        // プレイヤーのRendererを取得
        playerRenderer = GetComponent<Renderer>();

        // Rendererが取得できなかった場合、再度試す
        if (playerRenderer == null)
        {
            Debug.LogError("Player Renderer is not found! Trying again...");
            playerRenderer = GetComponentInChildren<Renderer>(); // 子オブジェクトのRendererを試す

            if (playerRenderer == null)
            {
                Debug.LogError("Player Renderer is still not found! Make sure the GameObject has a Renderer component.");
            }
            else
            {
                Debug.Log("Player Renderer found in child object.");
            }
        }
        else
        {
            Debug.Log("Player Renderer found.");
        }

        // 元の色を保存
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        // 吸い込み範囲のColliderを生成
        if (suctionRangeColliderPrefab != null)
        {
            suctionRangeColliderInstance = Instantiate(suctionRangeColliderPrefab, playerTransform.position, Quaternion.identity);
            suctionRangeColliderInstance.SetActive(false); // 初めは非表示にしておく
        }
    }


    private void Update()
    {
        if (playerRenderer == null)
        {
            Debug.LogError("Player Renderer is not found! Make sure the GameObject is active.");
            return; // Rendererが見つからない場合、処理を中止
        }

        // Qボタンが押されている間、吸い込み処理を実行
        if (Input.GetKey(KeyCode.Q))
        {
            TrySuckObjects();
            ShowSuctionRange(true); // 吸い込み範囲を表示
            ChangePlayerColor(suctionColor); // キャラクターの色を変更
        }
        else
        {
            ShowSuctionRange(false); // 吸い込み範囲を非表示
            ChangePlayerColor(originalColor); // キャラクターの色を元に戻す
        }

        // 吸い込み範囲をプレイヤーに合わせて移動
        if (suctionRangeColliderInstance != null)
        {
            suctionRangeColliderInstance.transform.position = playerTransform.position + playerTransform.forward * (maxDistance / 2);
        }
    }


    private Vector3 lastInputDirection = Vector3.forward; // 初期方向（前）

    private void TrySuckObjects()
    {
        // プレイヤーが向いている方向を使用
        Vector3 direction = playerTransform.forward;

        RaycastHit hit;
        Vector3 origin = playerTransform.position;

        // レイキャストのデバッグ表示
        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        // Raycastで引き寄せる対象を見つける
        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            Debug.Log("Hit object: " + hit.collider.name);  // ヒットしたオブジェクトをデバッグログに表示

            // 衝突したオブジェクトが敵タグを持つかチェック
            if (hit.collider.CompareTag("enemy_nodiy") || hit.collider.CompareTag("enemy_ba-na-do"))
            {
                Debug.Log("Enemy detected: " + hit.collider.name);  // 敵が見つかった場合のログ

                Vector3 toPlayer = playerTransform.position - hit.collider.transform.position;

                // Rigidbodyがあれば力を加える
                if (hit.collider.attachedRigidbody != null)
                {
                    hit.collider.attachedRigidbody.AddForce(toPlayer.normalized * suctionForce);
                }

                // プレイヤーに近づきすぎた場合、オブジェクトを消去
                if (Vector3.Distance(hit.collider.transform.position, playerTransform.position) < destroyDistance)
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Enemy destroyed: " + hit.collider.name);  // 消えたオブジェクトのログ
                }
            }
        }
    }

    private void ShowSuctionRange(bool isActive)
    {
        if (suctionRangeColliderInstance != null)
        {
            suctionRangeColliderInstance.SetActive(isActive); // 吸い込み範囲を表示/非表示
        }
    }

    private void ChangePlayerColor(Color color)
    {
        // プレイヤーの色を変更
        if (playerRenderer != null)
        {
            Debug.Log("Changing player color to: " + color);  // 色変更を確認するためのデバッグメッセージ

            // 新しいMaterialを作成せずに、直接playerRendererの色を変更
            playerRenderer.material.color = color; // 直接色を変更
        }
    }


    // 吸い込み範囲を視覚化するためにGizmosを描画（エディタでのみ動作）
    private void OnDrawGizmos()
    {
        if (playerTransform != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f); // 半透明の緑色
            Gizmos.DrawWireSphere(playerTransform.position + playerTransform.forward * (maxDistance / 2), destroyDistance); // 吸い込み範囲の描画
        }
    }
}
