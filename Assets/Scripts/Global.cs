using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public const float PPU = 100f;

    public const float TOWER_SCREEN_WIDTH = 512 / PPU;

    public const float TOWER_SCREEN_HEIGHT = 480 / PPU;

    public const float TOWER_LEFT_WALL_X = -TOWER_SCREEN_WIDTH / 2.0f;
    public const float TOWER_RIGHT_WALL_X = TOWER_SCREEN_WIDTH / 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Application.targetFrameRate = 30;
    }
}
