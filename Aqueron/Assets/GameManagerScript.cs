using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private int playerLayer;
    private int boundariesLayer;
    private int groundLayer;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        boundariesLayer = LayerMask.NameToLayer("Boundaries");
        //Colliders de boundaries ignoran a las de player y ground
        Physics2D.IgnoreLayerCollision(playerLayer, boundariesLayer);
        Physics2D.IgnoreLayerCollision(groundLayer, boundariesLayer);

    }
}
