using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // ---�y�ύX�_1�FHP�֘A�̕ϐ���ǉ��z---
    [Header("�X�e�[�^�X")]
    public int maxHealth = 30; // �G�̍ő�HP�i�C���X�y�N�^�[����ύX�\�j
    private int currentHealth;  // ���݂�HP

    [Header("�U���ݒ�")]
    public int attackDamage = 10; // �v���C���[�ɗ^����_���[�W

    // ---�y�ύX�_2�FStart�֐���HP���������z---
    void Start()
    {
        // �Q�[���J�n����HP���ő�l�ŏ���������
        currentHealth = maxHealth;
        // ������₷���悤�ɁA�ǂ̓G�����������ꂽ�����O���\��
        Debug.Log(gameObject.name + " HP������ ����HP: " + currentHealth);
    }

    // �v���C���[�Ƀ_���[�W��^���鏈���i����͑O��Ɠ����j
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.HP(attackDamage);
            }
        }
    }

    // ---�y�ύX�_3�F�_���[�W���󂯂�֐���ǉ��z---
    // ���̊֐��͑��̃X�N���v�g�i��F�v���C���[�̍U���j����Ăяo�����
    public void TakeDamage(int damage)
    {
        // ���݂�HP����_���[�W�ʂ�����
        currentHealth -= damage;

        Debug.Log(gameObject.name + " �� " + damage + " �̃_���[�W���󂯂��I �c��HP: " + currentHealth);

        // ����HP��0�ȉ��ɂȂ�����
        if (currentHealth <= 0)
        {
            Die(); // ���S�������Ăяo��
        }
    }

    // ---�y�ύX�_4�F���S�����̊֐���ǉ��z---
    private void Die()
    {
        Debug.Log(gameObject.name + " ���S");

        // ���̃Q�[���I�u�W�F�N�g�i�G���g�j���V�[������폜����
        Destroy(gameObject);
    }
}