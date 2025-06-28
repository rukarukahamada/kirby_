using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // �v���C���[�̍ő�HP
    public int maxHealth = 100;
    // ���݂�HP
    public int currentHealth;

    // �Q�[���J�n���ɌĂ΂��֐�
    void Start()
    {
        // HP���ő�l�ŏ���������
        currentHealth = maxHealth;
        Debug.Log("HP������ ����HP: " + currentHealth);
    }

    // �_���[�W���󂯂�HP�����炷�֐�
    // ���v�]�̒ʂ�A�֐������uHP�v�ɂ��Ă��܂��B
    public void HP(int damageAmount)
    {
        // ���݂�HP����_���[�W�ʂ�����
        StartCoroutine(DieAnimation());
        
        currentHealth -= damageAmount;

        // HP���}�C�i�X�ɂȂ�Ȃ��悤�ɒ���
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // HP�����������Ƃ��R���\�[���ɕ\��
        Debug.Log("�v���C���[���_���[�W���󂯂��I �c��HP: " + currentHealth);

        // ����HP��0�ɂȂ�����
        if (currentHealth <= 0)
        {
            Debug.Log("�v���C���[���S");
            Die();
            SceneManager.LoadScene("GAMEOVER");

        }
    }
    // ���S�����̊֐�
    private void Die()
    {
        Debug.Log(gameObject.name + " ���S");

        ///�y�ύX�_�z������Destroy����̂ł͂Ȃ��A�_�ŏ����̃R���[�`�����J�n����
        StartCoroutine(DieAnimation());
        Destroy(gameObject);
        
        SceneManager.LoadScene("GAMEOVER");
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
        // ���ŏ����������蔻������ɖ߂�
        GetComponent<Collider>().enabled = true;



    }
}