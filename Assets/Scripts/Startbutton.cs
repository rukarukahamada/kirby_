using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startbutton : MonoBehaviour
{
    public void change_button()
    {
        SceneManager.LoadScene("Stage");
    }
}
