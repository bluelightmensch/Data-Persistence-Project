using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TMP_InputField player;
    public void StartNew()
    {
        if (player.text != string.Empty){
            NewNameSelected();
            SceneManager.LoadScene(1);
        }
        
    }

    public void NewNameSelected(){
        GameManager.Instance.playerName = player.text;
    }
}
