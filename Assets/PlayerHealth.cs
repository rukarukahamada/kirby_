using UnityEngine;

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
            Debug.Log("�v���C���[�͗͐s����...");
            // �����ɃQ�[���I�[�o�[�����Ȃǂ�����
            // ��FgameObject.SetActive(false); // �v���C���[���\���ɂ���
        }
    }
}