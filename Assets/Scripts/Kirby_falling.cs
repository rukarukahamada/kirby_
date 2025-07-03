using System.Collections;
using UnityEngine;

public class Kirby_falling : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isFalling;

    private Kirby_basic1 kirbyScript; // HP�Ǘ��p�X�N���v�g�ւ̎Q��

    public int damageOnFall = 20; // �������Ɏ󂯂�_���[�W��

    void Start()
    {
        startPosition = transform.position;
        isFalling = false;

        // ����GameObject�ɂ���Kirby_basic���擾
        kirbyScript = GetComponent<Kirby_basic1>();

        // �O�̂��߁A������Ȃ������Ƃ��ɃG���[�\��
        if (kirbyScript == null)
        {
            Debug.LogError("Kirby_basic �X�N���v�g��������܂���I");
        }
        else
        {
            Debug.Log("Kirby_basic �X�N���v�g���擾���܂����I");
        }

    }

    void Update()
    {
        if (transform.position.y < -10f && !isFalling)
        {
            isFalling = true;

            // HP�����炷������ǉ�
            if (kirbyScript != null)
            {
                kirbyScript.TakeDamage(damageOnFall);
            }

            ResetPlayerPosition();
        }
    }

    void ResetPlayerPosition()
    {
        transform.position = startPosition;
        StartCoroutine(ResetFallingFlag());
    }

    IEnumerator ResetFallingFlag()
    {
        yield return new WaitForSeconds(0.5f);
        isFalling = false;
        Debug.Log("�v���C���[���������ăX�^�[�g�n�_�ɖ߂�A�_���[�W���󂯂܂����B");
    }
}
