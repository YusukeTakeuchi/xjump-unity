using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private GameObject floorPrefab;

    public const float PPU = Global.PPU;

    public const long FALLING_SPEED_MAX = 4999;
    public const long FALL_COUNT_LIMIT = 20000;

    public const float BOTTOM_FLOOR_Y = -3.0f;
    public const float FLOOR_INTERVAL = 80.0f / PPU;
    // indicates the area where objects are available by
    // designating the length from the center of the camera
    public const float PRESENTATION_HEIGHT = 4.8f;
    public const float CAMERA_WRAP_LIMIT = 9.6f;

    private GameObject mainCamera;

    private long bottomFloorNumber;
    private long lastCreatedFloorNumber;

    private long fallingSpeed = 0;
    private long fallCount = 0;

    private readonly Queue<GameObject> floors = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        floorPrefab = Resources.Load("Prefabs/Floor") as GameObject;

        mainCamera = Camera.main.gameObject;

        bottomFloorNumber = 0;
        lastCreatedFloorNumber = 0;

        PrepareAvailableFloors();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fallingSpeed < FALLING_SPEED_MAX)
        {
            fallingSpeed++;
        }
        fallCount += fallingSpeed;
        if (fallCount > FALL_COUNT_LIMIT)
        {
            fallCount -= FALL_COUNT_LIMIT;
            Fall();            
        }
    }

    private void PrepareAvailableFloors()
    {
        float cameraY = mainCamera.transform.position.y;
        float minY = cameraY - PRESENTATION_HEIGHT;
        float maxY = cameraY + PRESENTATION_HEIGHT;

        long floorNum = lastCreatedFloorNumber + 1;
        float y = BOTTOM_FLOOR_Y + (floorNum - bottomFloorNumber) * FLOOR_INTERVAL;
        while (y <= maxY)
        {
            if (minY <= y)
            {
                // TODO: implement checking big floor
                CreateRandomFloor(floorNum, y);
                lastCreatedFloorNumber = floorNum;
            }
            floorNum++;
            y = BOTTOM_FLOOR_Y + (floorNum - bottomFloorNumber) * FLOOR_INTERVAL;
        }
    }

    private void CreateRandomFloor(long floorNum, float y)
    {
        float width = (4 + Random.Range(0.0f, 2.0f) + Random.Range(0.0f, 2.0f)) * 32.0f / PPU;
        float x_min = Global.TOWER_LEFT_WALL_X + width / 2;
        float x_max = Global.TOWER_RIGHT_WALL_X - width / 2;
        CreateFloor(floorNum, Random.Range(x_min, x_max), y, width);
    }

    private void CreateFloor(long floorNum, float x, float y, float width)
    {
        GameObject floor = Instantiate(floorPrefab) as GameObject ;

        var pos = floor.transform.position;
        pos.x = x;
        pos.y = y;
        floor.transform.position = pos;

        var sprite = floor.GetComponent<SpriteRenderer>();
        var size = sprite.size;
        size.x = width;
        sprite.size = size;

        floor.GetComponent<Floor>().FloorNumber = floorNum;

        floors.Enqueue(floor);

        Debug.Log($"Floor Created: {floorNum}");
    }

    private void Fall()
    {
        PrepareAvailableFloors();
        var position = mainCamera.transform.position;
        position.y += 0.08f;
        mainCamera.transform.position = position;
    }
}
