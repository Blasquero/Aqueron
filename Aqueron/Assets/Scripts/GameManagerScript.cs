using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManagerScript : MonoBehaviour
{
    private GameObject player;
    private int playerLayer;
    private int boundariesLayer;
    private int groundLayer;
    private int enemyLayer;
    private CinemachineVirtualCamera vcam;
    public bool recolocarCamara;
    public static GameManagerScript Instance;
    public static bool inputEnabled = true;

    //Para que nose destruya entre escenas y si en estas escenas hay otro como este, el de esa escena se destruye
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
    private void Update()
    {
        if(vcam == null)
        {
            vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            vcam.m_Follow = player.transform;
        }
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