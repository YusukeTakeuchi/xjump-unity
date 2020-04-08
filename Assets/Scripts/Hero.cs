using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : MonoBehaviour
{
    public const float PPU = 100;

    public const float FRICTION_X = 1.5f / PPU;

    public const float MAX_SPEED_X = 16.0f / PPU;

    public const float Gravity = -2.0f / PPU;

    public const float TerminalVelocityY = -16.0f / PPU;

    // velocity X
    private float vx;

    // velocity Y
    private float vy;

    private int jumpAccelerationCount;

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
            }, "Hello", guiStyle
        );

    }

    // Start is called before the first frame update
    void Start()
    {
        this.vx = 0;
        this.vy = 0;
        this.jumpAccelerationCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        bool standing = IsStanding();
        float absAccX = 1.0f / PPU;

        // TODO: detect horizontal collision

        // TODO: scroll up

        // TODO: judge fallen



        if (standing)
        {
            // TODO: update score

            vy = 0;

            // jump has started?
            if (Keyboard.current.upArrowKey.isPressed)
            {
                jumpAccelerationCount = (int)(7 + Mathf.Abs(vx) * PPU / 4);
                vy = VelocityYWhileJumping(jumpAccelerationCount);
                standing = false;
            }
            absAccX = 1.5f / PPU;
        }

        vx = Mathf.Clamp(vx + ComputeAccelerationX(absAccX, standing), -MAX_SPEED_X, MAX_SPEED_X);

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

        // TODO: set direction
        

    }

    private void UpdatePosition()
    {
        Vector3 pos;
        pos = GetComponent<Transform>().position;
        pos.x += vx;
        pos.y += vy;
        GetComponent<Transform>().position = pos;
    }

    private float ComputeAccelerationX(float absAccX, bool standing)
    {
        bool left = Keyboard.current.leftArrowKey.isPressed;
        bool right = Keyboard.current.rightArrowKey.isPressed;

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

    private bool IsStanding()
    {
        // TODO: implement
        return transform.position.y <= 0;
    }

    private float VelocityYWhileJumping(int count) =>
        (count / 2.0f + 12) / PPU;

}
