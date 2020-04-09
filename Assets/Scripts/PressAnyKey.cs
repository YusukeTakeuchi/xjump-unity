using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PressAnyKey : MonoBehaviour
{
    public string SceneName;

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Fire();
        }
    }

    protected virtual void Fire()
    {
        if (SceneName != null)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
