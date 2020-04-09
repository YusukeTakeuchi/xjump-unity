using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private GameObject floorPrefab;

    public const float PPU = Global.PPU;

    public const long FALLING_SPEED_MAX = 4999;
    public const long FALL_COUNT_LIMIT = 20000;

    public const int BIG_FLOOR_INTERVAL = 5;

    public const float BOTTOM_FLOOR_Y = -3.0f;
    public const float FLOOR_INTERVAL = 160.0f / PPU;
    // indicates the area where objects are available by
    // designating the length from the center of the camera
    public const float PRESENTATION_HEIGHT = 4.8f;
    public const float CAMERA_WRAP_LIMIT = 9.6f;

    private GameObject mainCamera;

    private long bottomFloorNumber;
    //private long lastCreatedFloorNumber;
    private long nextFloorNumberToCreate;

    public long InitialFallingSpeed;

    private long fallingSpeed = 0;
    private long fallCount = 0;

    private readonly Queue<GameObject> floors = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        floorPrefab = Resources.Load("Prefabs/Floor") as GameObject;

        mainCamera = Camera.main.gameObject;

        bottomFloorNumber = 0;
        nextFloorNumberToCreate = 0;

        fallingSpeed = InitialFallingSpeed;

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
        GetAvailableRange(out float minY, out float maxY);
        float y = GetYForFloorNumber(nextFloorNumberToCreate);
        while (y <= maxY)
        {
            if (minY <= y)
            {
                if (nextFloorNumberToCreate % BIG_FLOOR_INTERVAL == 0)
                {
                    CreateBigFloor(nextFloorNumberToCreate, y);
                }
                else
                {
                    CreateRandomFloor(nextFloorNumberToCreate, y);
                }
                nextFloorNumberToCreate++;
            }
            y = GetYForFloorNumber(nextFloorNumberToCreate);
        }
    }

    private float GetYForFloorNumber(long floorNumber) =>
        BOTTOM_FLOOR_Y + (floorNumber - bottomFloorNumber) * FLOOR_INTERVAL;


    private void CleanUnavailableFloors()
    {
        GetAvailableRange(out float minY, out float maxY);

        while (floors.Count > 0)
        {
            var floor = floors.Peek();
            float y = floor.transform.position.y;
            if (y < minY || maxY < y)
            {
                Debug.Log($"Floor Destroyed: {floor.GetComponent<Floor>().FloorNumber}");
                floors.Dequeue();
                Destroy(floor);
            }
            else
            {
                break;
            }
        }
    }

    private void CreateBigFloor(long floorNum, float y)
    {
        float x = (Global.TOWER_LEFT_WALL_X + Global.TOWER_RIGHT_WALL_X) / 2.0f;
        float width = Global.TOWER_RIGHT_WALL_X - Global.TOWER_LEFT_WALL_X;
        CreateFloor(floorNum, x, y, width);
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
        CleanUnavailableFloors();
        var position = mainCamera.transform.position;
        position.y += 0.08f;
        mainCamera.transform.position = position;
    }

    // The available range is the area where floors are prepared
    private void GetAvailableRange(out float minY, out float maxY)
    {
        float cameraY = mainCamera.transform.position.y;
        minY = cameraY - PRESENTATION_HEIGHT;
        maxY = cameraY + PRESENTATION_HEIGHT;

    }
}
