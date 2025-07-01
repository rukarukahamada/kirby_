using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // �v���C���[��Transform���Q��
    public Vector3 offset = new Vector3(0, 5, -10);  // �J�����̃I�t�Z�b�g�i�ʒu�j
    public float smoothSpeed = 0.125f;  // �Ǐ]�̃X���[�Y��

    void LateUpdate()
    {
        // �v���C���[�̈ʒu�ɃI�t�Z�b�g���������ڕW�ʒu
        Vector3 desiredPosition = player.position + offset;

        // �ڕW�ʒu�Ɍ������ăJ���������炩�Ɉړ�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // �J�����̈ʒu���X�V
        transform.position = smoothedPosition;

        // �J��������Ƀv���C���[�������悤�ɂ���
        transform.LookAt(player);
    }
}
