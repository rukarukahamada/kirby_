using UnityEngine;

public class Kirby_ranger : MonoBehaviour
{
    public GameObject projectilePrefab; // ���˂��鋅�̃v���n�u
    public Transform shotPoint;         // ���ˈʒu
    public float shotForce = 700f;      // ���˗�
    public float cooldownTime = 0.5f;   // �N�[���^�C���i�b�j

    private float nextShotTime = 0f;    // ���ɔ��˂ł��鎞��

    // �v���C���[�̐F���Ǘ��i�����͊ȒP��Renderer�̐F����j
    private Renderer playerRenderer;

    private float eKeyHoldTime = 0f;   // E�L�[������������
    private float requiredHoldTime = 5f; // ����������K�v����

    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null)
        {
            Debug.LogWarning("Renderer�R���|�[�l���g��������܂���B");
        }
    }

    void Update()
    {
        // E�L�[�����������Ă��鎞�Ԃ��v��
        if (Input.GetKey(KeyCode.E))
        {
            eKeyHoldTime += Time.deltaTime;

            if (eKeyHoldTime >= requiredHoldTime)
            {
                // 5�b�ԉ�����������F���s���N�ɖ߂�
                playerRenderer.material.color = Color.magenta;
                eKeyHoldTime = 0f; // �J�E���^�[���Z�b�g�i�A���ŉ��x���߂�Ȃ��悤�Ɂj
            }
        }
        else
        {
            // E�L�[�������烊�Z�b�g
            eKeyHoldTime = 0f;
        }

        if (playerRenderer == null) return;

        // �v���C���[�̐F���ԐF������i�Ԃ̐����������A�΁E���ア���ǂ����j
        Color playerColor = playerRenderer.material.color;
        bool isPlayerRed = playerColor.r > 0.8f && playerColor.g < 0.3f && playerColor.b < 0.3f;

        // �v���C���[���ԐF�̎�����Q�L�[�Ŏˌ��\
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

        // �v���C���[�̐F���ԐF������i�قڐԂȂ�OK�j
        bool isPlayerRed = false;
        if (playerRenderer != null)
        {
            Color playerColor = playerRenderer.material.color;
            // �ԐF����i�Ԃ̐����������A�΁E���ア���ǂ����j
            isPlayerRed = playerColor.r > 0.8f && playerColor.g < 0.3f && playerColor.b < 0.3f;
        }

        // ���˂���Projectile��ProjectileCollision�����āA�L���t���O��n��
        ProjectileCollision pc = projectile.AddComponent<ProjectileCollision>();
        pc.isActive = isPlayerRed;

        Destroy(projectile, 3f);
    }
}

// ProjectileCollision���Ƀt���O��p�ӂ��Ĕ���
public class ProjectileCollision : MonoBehaviour
{
    // true�̂Ƃ��̂ݓG�������������s��
    public bool isActive = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return; // �F���Ԃ���Ȃ��Ȃ牽�����Ȃ�

        if (collision.gameObject.CompareTag("enemy_nodiy") || collision.gameObject.CompareTag("enemy_ba-na-do"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
