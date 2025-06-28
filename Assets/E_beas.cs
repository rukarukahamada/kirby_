using UnityEngine;
using System.Collections;

public class E_beas : MonoBehaviour
{
    // HP�֘A�̕ϐ���ǉ�
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

    // �v���C���[�Ƀ_���[�W��^���鏈��
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

    // �_���[�W���󂯂�֐�
    // ���̊֐��͑��̃X�N���v�g�i��F�v���C���[�̍U���j����Ăяo�����
    public void TakeDamage(int damage)
    {
        StartCoroutine(DieAnimation());
        // ���݂�HP����_���[�W�ʂ�����
        currentHealth -= damage;

        Debug.Log(gameObject.name + " �� " + damage + " �̃_���[�W���󂯂��I �c��HP: " + currentHealth);

        // ����HP��0�ȉ��ɂȂ�����
        if (currentHealth <= 0)
        {
            Die(); // ���S�������Ăяo��
        }
    }

    // ���S�����̊֐�
    private void Die()
    {
        Debug.Log(gameObject.name + " ���S");

        ///�y�ύX�_�z������Destroy����̂ł͂Ȃ��A�_�ŏ����̃R���[�`�����J�n����
        StartCoroutine(DieAnimation());
    }
    private IEnumerator DieAnimation()
    {
        // 1. �܂������蔻��𖳌��ɂ���
        // ��������Ȃ��ƁA�_�Œ��Ƀv���C���[���ڐG���ă_���[�W���󂯂��肷��\��������
        GetComponent<Collider>().enabled = false;

        // 2. �_�ŏ���
        Renderer enemyRenderer = GetComponent<Renderer>(); // �I�u�W�F�N�g�̌����ڂ��Ǘ�����R���|�[�l���g
        int blinkCount = 5;          // �_�ł���񐔁i������1��j
        float blinkInterval = 0.1f;  // �_�ł̊Ԋu�i�b�j

        for (int i = 0; i < blinkCount; i++)
        {
            // �I�u�W�F�N�g�̌����ڂ��\���ɂ���
            enemyRenderer.enabled = false;
            // blinkInterval�b����������҂�
            yield return new WaitForSeconds(blinkInterval);

            // �I�u�W�F�N�g�̌����ڂ�\������
            enemyRenderer.enabled = true;
            // �Ă�blinkInterval�b����������҂�
            yield return new WaitForSeconds(blinkInterval);
        }

        // 3. �_�ł��I�������A�Ō�ɃI�u�W�F�N�g���V�[�����犮�S�ɍ폜����
        Destroy(gameObject);
    }
}