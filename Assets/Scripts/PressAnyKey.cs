using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PressAnyKey : MonoBehaviour
{
    public string SceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.isPressed)
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
