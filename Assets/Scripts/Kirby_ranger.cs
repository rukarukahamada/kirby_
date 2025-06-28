using UnityEngine;

public class Kirby_ranger : MonoBehaviour
{
    public GameObject bulletPrefab; // �e��Prefab
    public float bulletSpeed = 10f; // �e�̔��ˑ��x
    public Transform firePoint; // �e�𔭎˂���ʒu�i�v���C���[�̑O���j

    private Renderer playerRenderer;

    void Start()
    {
        playerRenderer = GetComponent<Renderer>(); // �v���C���[��Renderer���擾
    }

    void Update()
    {
        // �v���C���[�̐F���Ԃ̏ꍇ�AQ�{�^������������e�𔭎�
        if (playerRenderer.material.color == Color.red && Input.GetKeyDown(KeyCode.Q))
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        // �e��Prefab���v���C���[�̑O���ɔ���
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // �e�ɗ͂������Ĕ���
                rb.linearVelocity = firePoint.up * bulletSpeed;
            }
        }
    }
}

