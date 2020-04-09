using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : MonoBehaviour
{
    public const float PPU = 100;

    public const float FRICTION_X = 1.5f / PPU;

    public const float MAX_SPEED_X = 16.0f / PPU;

    public const float Gravity = -2.0f / PPU;

    public const float TerminalVelocityY = -16.0f / PPU;

    public const float MIN_X = Global.TOWER_LEFT_WALL_X + 16.0f / PPU;
    public const float MAX_X = Global.TOWER_RIGHT_WALL_X - 16.0f / PPU;

    // velocity X
    private float vx;

    // velocity Y
    private float vy;

    private int jumpAccelerationCount;

    private bool isOnGround = false;

    private Vector3 SpriteSize =>
        GetComponent<SpriteRenderer>().bounds.size;

    private void OnGUI()
    {
        var guiStyle = new GUIStyle();
        guiStyle.fontSize = 32;
        guiStyle.alignment = TextAnchor.MiddleRight;
        GUI.Label(
            new Rect()
            {
                width = 128,
                height = 32,
                x = Screen.width / 2,
                y = Screen.height - 32,
            }, (isOnGround ? "Ground" : "Air") + $" {DebugText()}", guiStyle
        );

    }

    private string DebugText()
    {
        return "";
    }

    // Start is called before the first frame update
    void Start()
    {
        this.vx = 0;
        this.vy = 0;
        this.jumpAccelerationCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePosition();

        CheckHorizontalWallCollision();

        // TODO: judge fallen

        //float? standingFloorTopMaybe = StandingFloorTop();
        GameObject standingFloor = GetStandingFloor();
        bool standing = false;

        if (standingFloor != null)
        {
            // update score
            FloorReached(standingFloor);

            fixYOnPlatform(GetFloorTopY(standingFloor));
            vy = 0;

            // jump has started?
            if (Keyboard.current.upArrowKey.isPressed)
            {
                jumpAccelerationCount = (int)(7 + Mathf.Abs(vx) * PPU / 4);
                vy = VelocityYWhileJumping(jumpAccelerationCount);
            }
            else
            {
                standing = true;
            }
        }

        isOnGround = standing;


        ProcessHorizontalAcceleration(standing);
        ProcessVerticalAcceleration(standing);

        // TODO: set direction



    }

    private void CheckHorizontalWallCollision()
    {
        if (transform.position.x < MIN_X)
        {
            // collides with the left wall
            SetPosition(MIN_X, null);
            vx = -vx / 2.0f;
        }

        if (MAX_X < transform.position.x)
        {
            // collides with the right wall
            SetPosition(MAX_X, null);
            vx = -vx / 2.0f;
        }
    }

    private void fixYOnPlatform(float platformTop)
    {
        Vector3 pos = transform.position;
        pos.y = platformTop + SpriteSize.y / 2 - 8 / PPU;
        transform.position = pos;
    }

    private void ProcessHorizontalAcceleration(bool standing)
    {
        vx = Mathf.Clamp(vx + ComputeAccelerationX(standing), -MAX_SPEED_X, MAX_SPEED_X);
    }

    private float ComputeAccelerationX(bool standing)
    {
        bool left = Keyboard.current.leftArrowKey.isPressed;
        bool right = Keyboard.current.rightArrowKey.isPressed;

        float absAccX = (standing ? 1.5f : 1.0f) / PPU;

        if (left && !right)
        {
            return -absAccX;
        }

        if (!left && right)
        {
            return absAccX;
        }

        if (standing)
        {
            if (Mathf.Abs(vx) < FRICTION_X)
            {
                // will set vx to 0
                return -vx;
            }
            else
            {
                return -Mathf.Sign(vx) * FRICTION_X;
            }
        }

        return 0;
    }

    private void ProcessVerticalAcceleration(bool standing)
    {
        if (!standing)
        {
            if (jumpAccelerationCount > 0)
            {
                vy = VelocityYWhileJumping(jumpAccelerationCount);
                if (Keyboard.current.upArrowKey.isPressed)
                {
                    jumpAccelerationCount--;
                }
                else
                {
                    jumpAccelerationCount = 0;
                }

            }
            else
            {
                vy += Gravity;
                if (vy < TerminalVelocityY)
                {
                    vy = TerminalVelocityY;
                }
            }
        }
    }

    private void UpdatePosition()
    {
        Vector3 pos = transform.position;
        pos.x += vx;
        pos.y += vy;
        transform.position = pos;
    }

    private void SetPosition(float? mx, float? my)
    {
        Vector3 pos = transform.position;
        if (mx is float x)
            pos.x = x;
        if (mx is float y)
            pos.y += vy;
        transform.position = pos;
    }



    private GameObject GetStandingFloor()
    {
        if (vy > 0)
        {
            // going up
            return null;
        }
        var myCollisionPoint = new Vector2()
        {
            x = transform.position.x,
            y = transform.position.y - SpriteSize.y / 2,
        };
        var myCollisionSize = new Vector2()
        {
            x = SpriteSize.x,
            y = 1.0f / PPU,
        };
        var colliders = Physics2D.OverlapBoxAll(myCollisionPoint, myCollisionSize, 0)
                           .Where((c) => c.CompareTag("Floor"))
                           .ToList();
        if (colliders.Count() == 0)
        {
            return null;
        }

        return colliders[0].gameObject;
    }

    private float GetFloorTopY(GameObject gameObject)
    {
        float y = gameObject.transform.position.y;
        float height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        return y + height / 2;
    }

    private float VelocityYWhileJumping(int count) =>
        (count / 2.0f + 12) / PPU;

    private void FloorReached(GameObject floor)
    {
        long floorNum = floor.GetComponent<Floor>().FloorNumber;
        var score = GameObject.FindWithTag("Score");
        score.GetComponent<Score>().ReachFloor(floorNum);
    }
}
