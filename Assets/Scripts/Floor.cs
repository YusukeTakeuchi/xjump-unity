using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public long FloorNumber { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        var collider = GetComponent<BoxCollider2D>();
        var size = collider.size;
        size.x = GetComponent<SpriteRenderer>().size.x;
        collider.size = size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
