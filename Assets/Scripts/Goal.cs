using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    // �G�ꂽ��؂�ւ������V�[����
    public string GAMECLEAR;
    
    // �v���C���[���u���b�N�ɐG�ꂽ���̏���
    private void OnCollisionEnter(Collision collision)
    {
        // �v���C���[�Ƃ̐ڐG���m�F
        if (collision.gameObject.CompareTag("ka-bi"))
        {
            // �V�[����؂�ւ�
            SceneManager.LoadScene("GAMECLEAR");
        }
    }
}