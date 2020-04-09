using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private GameObject scoreTextObject;

    private long floor;
    private long Floor
    {
        get => floor;
        set
        {
            floor = value;
            scoreTextObject.GetComponent<Text>().text = $"{floor,4} F";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        scoreTextObject = GameObject.Find("ScoreText");
        this.Floor = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReachFloor(long floorNumber)
    {
        this.Floor = Math.Max(this.Floor, floorNumber);
    }
}
