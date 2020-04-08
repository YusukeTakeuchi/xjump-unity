using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private GameObject floorPrefab;

    public const float PPU = Global.PPU;

    // Start is called before the first frame update
    void Start()
    {
        floorPrefab = Resources.Load("Prefabs/Floor") as GameObject;

        CreateRandomFloor(-1.0f);
        CreateRandomFloor(0.0f);
        CreateRandomFloor(1.0f);
        CreateRandomFloor(2.0f);
        CreateRandomFloor(3.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateRandomFloor(float y)
    {
        float width = (4 + Random.Range(0.0f, 2.0f) + Random.Range(0.0f, 2.0f)) * 32.0f / PPU;
        float x_min = Global.TOWER_LEFT_WALL_X + width / 2;
        float x_max = Global.TOWER_RIGHT_WALL_X - width / 2;
        CreateFloor(Random.Range(x_min, x_max), y, width);
    }

    private void CreateFloor(float x, float y, float width)
    {
        GameObject floor = Instantiate(floorPrefab) as GameObject;

        var pos = floor.transform.position;
        pos.x = x;
        pos.y = y;
        floor.transform.position = pos;

        var sprite = floor.GetComponent<SpriteRenderer>();
        var size = sprite.size;
        size.x = width;
        sprite.size = size;

    }
}
