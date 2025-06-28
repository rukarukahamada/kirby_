using UnityEngine;

public class KirbySuction : MonoBehaviour
{
    public float suctionForce = 2f; // �z�����ޗ͂̋���
    public float maxDistance = 5f; // �z�����ލő勗��
    public float destroyDistance = 1f; // �z�����ރI�u�W�F�N�g�������鋗��
    public Transform playerTransform; // �v���C���[��Transform
    public GameObject suctionRangeColliderPrefab; // �z�����ޔ͈͂�Collider�iPrefab�j
    public Color suctionColor = Color.green; // �z�����ݎ��̃L�����N�^�[�̐F
    private Color originalColor; // ���̃L�����N�^�[�̐F

    private GameObject suctionRangeColliderInstance; // ���ۂɕ\������Collider�̃C���X�^���X
    private Renderer playerRenderer; // �v���C���[��Renderer

    private void Start()
    {
        // �v���C���[��Renderer���擾
        playerRenderer = GetComponent<Renderer>();

        // ���̐F��ۑ�
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        // �z�����ݔ͈͂�Collider�𐶐�
        if (suctionRangeColliderPrefab != null)
        {
            suctionRangeColliderInstance = Instantiate(suctionRangeColliderPrefab, playerTransform.position, Quaternion.identity);
            suctionRangeColliderInstance.SetActive(false); // ���߂͔�\���ɂ��Ă���
        }
    }

    private void Update()
    {
        // Q�{�^����������Ă���ԁA�z�����ݏ��������s
        if (Input.GetKey(KeyCode.Q))
        {
            TrySuckObjects();
            ShowSuctionRange(true); // �z�����ݔ͈͂�\��
            ChangePlayerColor(suctionColor); // �L�����N�^�[�̐F��ύX
        }
        else
        {
            ShowSuctionRange(false); // �z�����ݔ͈͂��\��
            ChangePlayerColor(originalColor); // �L�����N�^�[�̐F�����ɖ߂�
        }
    }

    private void TrySuckObjects()
    {
        RaycastHit hit;
        bool hasSucked = false; // �z�����݂������������𔻒肷��t���O

        // �v���C���[�̑O����Ray���΂�
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, maxDistance))
        {
            // �q�b�g�����I�u�W�F�N�g���z�����߂���̂ł���Ώ���
            if (hit.collider.CompareTag("enemy_ranger") || hit.collider.CompareTag("enemy_sleep")) // �z�����ޑΏۂ̃^�O��ݒ�
            {
                Vector3 direction = playerTransform.position - hit.collider.transform.position; // �z�����ޕ���
                hit.collider.attachedRigidbody.AddForce(direction.normalized * suctionForce); // �z�����ޗ͂�������

                // �z�����܂ꂽ�I�u�W�F�N�g�����̋����ɋ߂Â�����폜
                if (Vector3.Distance(hit.collider.transform.position, playerTransform.position) < destroyDistance)
                {
                    Destroy(hit.collider.gameObject); // �I�u�W�F�N�g���폜

                    // �z�����񂾂̂� enemy_ranger �̏ꍇ�͐F��ԂɕύX
                    if (hit.collider.CompareTag("enemy_ranger"))
                    {
                        hasSucked = true; // �z�����݂����������t���O�𗧂Ă�
                    }
                }
            }
        }

        // �z�����݂�����������F��ԂɕύX
        if (hasSucked)
        {
            ChangePlayerColor(Color.red); // �L�����N�^�[�̐F��ԂɕύX
        }
    }




    private void ShowSuctionRange(bool isActive)
    {
        if (suctionRangeColliderInstance != null)
        {
            suctionRangeColliderInstance.SetActive(isActive); // �z�����ݔ͈͂�\��/��\��
        }
    }

    private void ChangePlayerColor(Color color)
    {
        // �v���C���[�̐F��ύX
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }

    // �z�����ݔ͈͂����o�����邽�߂�Gizmos��`��i�G�f�B�^�ł̂ݓ���j
    private void OnDrawGizmos()
    {
        if (playerTransform != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f); // �������̗ΐF
            Gizmos.DrawWireSphere(playerTransform.position + playerTransform.forward * (maxDistance / 2), destroyDistance); // �z�����ݔ͈͂̕`��
        }
    }
}