using System.Collections;
using UnityEngine;

public class Kirby_falling : MonoBehaviour
{
    // �X�^�[�g�n�_
    private Vector3 startPosition;

    // �����������ǂ����̃t���O
    private bool isFalling;

    void Start()
    {
        // �X�^�[�g�n�_���L�^
        startPosition = transform.position;
        isFalling = false;
    }

    void Update()
    {
        // �v���C���[���������Ă��邩�ǂ����𔻒�i��FY���W������l�ȉ��Ȃ痎���j
        if (transform.position.y < -10f && !isFalling) // �Ⴆ��Y���W��-10�����Ȃ痎�������Ɣ���
        {
            isFalling = true;
            ResetPlayerPosition();
        }
    }

    // �v���C���[���X�^�[�g�n�_�ɖ߂��֐�
    void ResetPlayerPosition()
    {
        // �X�^�[�g�n�_�Ƀv���C���[��߂�
        transform.position = startPosition;

        // �����ň�莞�ԑ҂��Ă���t���O�����Z�b�g����
        StartCoroutine(ResetFallingFlag());
    }

    // �t���O�����Z�b�g���邽�߂̃R���[�`��
    IEnumerator ResetFallingFlag()
    {
        // �����ҋ@�i�Ⴆ��0.5�b�j���Ă���t���O�����Z�b�g
        yield return new WaitForSeconds(0.5f);
        isFalling = false;
        Debug.Log("�v���C���[���������ăX�^�[�g�n�_�ɖ߂�܂����B");
    }
}
