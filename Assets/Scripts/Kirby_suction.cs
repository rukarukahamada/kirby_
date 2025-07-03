using UnityEngine;

public class KirbySuction : MonoBehaviour
{
    public float suctionForce = 2f; // �z�����ޗ͂̋���
    public float maxDistance = 5f; // �z�����ލő勗��
    public float destroyDistance = 1f; // �z�����ރI�u�W�F�N�g�������鋗��
    public Transform playerTransform; // �v���C���[��Transform
    public GameObject suctionRangeColliderPrefab; // �z�����ޔ͈͂�Collider�iPrefab�j
    public Color suctionColor = Color.blue; // �z�����ݎ��̃L�����N�^�[�̐F
    private Color originalColor; // ���̃L�����N�^�[�̐F

    private GameObject suctionRangeColliderInstance; // ���ۂɕ\������Collider�̃C���X�^���X
    private Renderer playerRenderer; // �v���C���[��Renderer

    private void Start()
    {
        // �v���C���[��Renderer���擾
        playerRenderer = GetComponent<Renderer>();

        // Renderer���擾�ł��Ȃ������ꍇ�A�ēx����
        if (playerRenderer == null)
        {
            Debug.LogError("Player Renderer is not found! Trying again...");
            playerRenderer = GetComponentInChildren<Renderer>(); // �q�I�u�W�F�N�g��Renderer������

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
        if (playerRenderer == null)
        {
            Debug.LogError("Player Renderer is not found! Make sure the GameObject is active.");
            return; // Renderer��������Ȃ��ꍇ�A�����𒆎~
        }

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

        // �z�����ݔ͈͂��v���C���[�ɍ��킹�Ĉړ�
        if (suctionRangeColliderInstance != null)
        {
            suctionRangeColliderInstance.transform.position = playerTransform.position + playerTransform.forward * (maxDistance / 2);
        }
    }


    private Vector3 lastInputDirection = Vector3.forward; // ���������i�O�j

    private void TrySuckObjects()
    {
        // �v���C���[�������Ă���������g�p
        Vector3 direction = playerTransform.forward;

        RaycastHit hit;
        Vector3 origin = playerTransform.position;

        // ���C�L���X�g�̃f�o�b�O�\��
        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        // Raycast�ň����񂹂�Ώۂ�������
        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            Debug.Log("Hit object: " + hit.collider.name);  // �q�b�g�����I�u�W�F�N�g���f�o�b�O���O�ɕ\��

            // �Փ˂����I�u�W�F�N�g���G�^�O�������`�F�b�N
            if (hit.collider.CompareTag("enemy_nodiy") || hit.collider.CompareTag("enemy_ba-na-do"))
            {
                Debug.Log("Enemy detected: " + hit.collider.name);  // �G�����������ꍇ�̃��O

                Vector3 toPlayer = playerTransform.position - hit.collider.transform.position;

                // Rigidbody������Η͂�������
                if (hit.collider.attachedRigidbody != null)
                {
                    hit.collider.attachedRigidbody.AddForce(toPlayer.normalized * suctionForce);
                }

                // �v���C���[�ɋ߂Â��������ꍇ�A�I�u�W�F�N�g������
                if (Vector3.Distance(hit.collider.transform.position, playerTransform.position) < destroyDistance)
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Enemy destroyed: " + hit.collider.name);  // �������I�u�W�F�N�g�̃��O
                }
            }
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
            Debug.Log("Changing player color to: " + color);  // �F�ύX���m�F���邽�߂̃f�o�b�O���b�Z�[�W

            // �V����Material���쐬�����ɁA����playerRenderer�̐F��ύX
            playerRenderer.material.color = color; // ���ڐF��ύX
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
