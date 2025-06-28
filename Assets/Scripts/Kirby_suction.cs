using UnityEngine;

public class KirbySuction : MonoBehaviour
{
    public float suctionForce = 2f; // 吸い込む力の強さ
    public float maxDistance = 5f; // 吸い込む最大距離
    public float destroyDistance = 1f; // 吸い込むオブジェクトが消える距離
    public Transform playerTransform; // プレイヤーのTransform
    public GameObject suctionRangeColliderPrefab; // 吸い込む範囲のCollider（Prefab）
    public Color suctionColor = Color.green; // 吸い込み時のキャラクターの色
    private Color originalColor; // 元のキャラクターの色

    private GameObject suctionRangeColliderInstance; // 実際に表示するColliderのインスタンス
    private Renderer playerRenderer; // プレイヤーのRenderer

    private void Start()
    {
        // プレイヤーのRendererを取得
        playerRenderer = GetComponent<Renderer>();

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
    }

    private void TrySuckObjects()
    {
        RaycastHit hit;
        bool hasSucked = false; // 吸い込みが成功したかを判定するフラグ

        // プレイヤーの前方にRayを飛ばす
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, maxDistance))
        {
            // ヒットしたオブジェクトが吸い込めるものであれば処理
            if (hit.collider.CompareTag("enemy_ranger") || hit.collider.CompareTag("enemy_sleep")) // 吸い込む対象のタグを設定
            {
                Vector3 direction = playerTransform.position - hit.collider.transform.position; // 吸い込む方向
                hit.collider.attachedRigidbody.AddForce(direction.normalized * suctionForce); // 吸い込む力を加える

                // 吸い込まれたオブジェクトが一定の距離に近づいたら削除
                if (Vector3.Distance(hit.collider.transform.position, playerTransform.position) < destroyDistance)
                {
                    Destroy(hit.collider.gameObject); // オブジェクトを削除

                    // 吸い込んだのが enemy_ranger の場合は色を赤に変更
                    if (hit.collider.CompareTag("enemy_ranger"))
                    {
                        hasSucked = true; // 吸い込みが成功したフラグを立てる
                    }
                }
            }
        }

        // 吸い込みが成功したら色を赤に変更
        if (hasSucked)
        {
            ChangePlayerColor(Color.red); // キャラクターの色を赤に変更
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
            playerRenderer.material.color = color;
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