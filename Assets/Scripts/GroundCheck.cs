using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public const string TAG = "Floor";

    private bool isGround;

    private bool isGroundEnter, isGroundStay, isGroundExit;

    private float? platformYEntered;

    // Start is called before the first frame update
    void Start()
    {
        isGround = false;
        isGroundEnter = isGroundStay = isGroundExit = false;
        platformYEntered = null;
    }

    public bool Consume(out float? platformY)
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }else if (isGroundExit)
        {
            isGround = false;
        }
        isGroundEnter = isGroundStay = isGroundExit = false;

        platformY = platformYEntered;
        platformYEntered = null;

        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TAG)
        {
            isGroundEnter = true;
            platformYEntered = GetPlatformTop(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGroundStay = isGroundStay || (collision.tag == TAG);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGroundExit = isGroundExit || (collision.tag == TAG);
    }

    private float GetPlatformTop(Collider2D collision)
    {
        float y = collision.gameObject.transform.position.y;
        float height = collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        return y + height / 2;
    }

}
