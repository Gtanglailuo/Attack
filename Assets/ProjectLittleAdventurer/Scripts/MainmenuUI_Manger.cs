using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainmenuUI_Manger : MonoBehaviour
{
    public void Button_Start()
    {
        SceneManager.LoadScene("MyScene");
    }

    public void Button_Quit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }
    
}
