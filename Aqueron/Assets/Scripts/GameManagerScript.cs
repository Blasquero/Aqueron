using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManagerScript : MonoBehaviour
{
    private int playerLayer;
    private int boundariesLayer;
    private int groundLayer;
    private int enemyLayer;
    private CinemachineVirtualCamera vcam;
    public bool recolocarCamara;
    public static GameManagerScript Instance;
    public static bool inputEnabled = true;

    void Start()
    {
        recolocarCamara = false;
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        boundariesLayer = LayerMask.NameToLayer("Boundaries");
        //Colliders de boundaries ignora otros colliders
        Physics2D.IgnoreLayerCollision(playerLayer, boundariesLayer);
        Physics2D.IgnoreLayerCollision(groundLayer, boundariesLayer);
        Physics2D.IgnoreLayerCollision(enemyLayer, boundariesLayer);
        Instance = this;
    }

    private void FixedUpdate()
    {
        if(recolocarCamara && vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping >= 0.3001)
        {
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping -= .3f;
            if (vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping == 0) recolocarCamara = false;
        }
    }
}