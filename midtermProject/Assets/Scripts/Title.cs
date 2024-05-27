using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void GameStart()
    {
        PlayerPrefs.SetFloat("X", 0f);
        PlayerPrefs.SetFloat("Y", -2f);
        PlayerPrefs.SetFloat("PlayerHp", 100f);
        SceneManager.LoadScene("Main");
    }
}
