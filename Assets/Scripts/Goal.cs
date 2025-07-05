using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    // 触れたら切り替えたいシーン名
    public string GAMECLEAR;
    
    // プレイヤーがブロックに触れた時の処理
    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーとの接触を確認
        if (collision.gameObject.CompareTag("ka-bi"))
        {
            // シーンを切り替え
            SceneManager.LoadScene("GAMECLEAR");
        }
    }
}