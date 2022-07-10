using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    public void Quit()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
