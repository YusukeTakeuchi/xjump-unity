using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private GameObject gameOverCanvasPrefab;

    private GameObject scoreTextObject;

    private long floor;
    private long Floor
    {
        get => floor;
        set
        {
            floor = value;
            SetScoreText($"{floor,4} F");
        }
    }

    public long Speed { get; set; }

    public bool gameOver;


    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvasPrefab = Resources.Load("Prefabs/GameOverCanvas") as GameObject;
        scoreTextObject = GameObject.Find("ScoreText");
        this.Floor = 0;
        this.gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReachFloor(long floorNumber)
    {
        this.Floor = Math.Max(this.Floor, floorNumber);
    }

    public void Gameover()
    {
        this.gameOver = true;

        var global = Global.GetInstance();
        Destroy(global.Hero.gameObject);
        Destroy(global.Tower.gameObject);

        // show "GAME OVER"
        Instantiate(gameOverCanvasPrefab);
    }

    private void SetScoreText(string text)
    {
        scoreTextObject.GetComponent<Text>().text = text;
    }
}
