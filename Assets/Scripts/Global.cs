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

    public static Global GetInstance()
    {
        return GameObject.Find("Global").GetComponent<Global>();
    }

    private Hero hero;
    public Hero Hero
    {
        get
        {
            if (hero == null)
            {
                hero = GameObject.Find("Hero").GetComponent<Hero>();
            }
            return hero;
        }
    }

    private Score score;
    public Score Score
    {
        get
        {
            if (score == null)
            {
                score = GameObject.Find("Score").GetComponent<Score>();
            }
            return score;
        }
    }

    private Tower tower;
    public Tower Tower
    {
        get
        {
            if (tower == null)
            {
                tower = GameObject.Find("Tower").GetComponent<Tower>();
            }
            return tower;
        }
    }

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
