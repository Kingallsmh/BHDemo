using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAddition : MonoBehaviour {

    public bool MoveWithPlatform;
    public Rigidbody2D rb2d;

    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        Vector3 boxSize = col.bounds.extents;
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(col.offset.x, col.offset.y, 0), transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxSize.x * 2, boxSize.y * 2, 1));
        
    }
}
