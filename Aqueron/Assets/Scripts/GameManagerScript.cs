using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

public class GameManagerScript : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject boss1;
    private int playerLayer;
    private int boundariesLayer;
    private int groundLayer;
    private int enemyLayer;
    private CinemachineVirtualCamera vcam;
    private System.Random rnd;

    public static GameManagerScript Instance;
    private bool repositionCamera;
    private bool inputEnabled;

    //Para que nose destruya entre escenas y si en estas escenas hay otro como este, el de esa escena se destruye
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        rnd = new System.Random();
        inputEnabled = true;
        player = GameObject.FindGameObjectWithTag("Player");
        repositionCamera = false;
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        boundariesLayer = LayerMask.NameToLayer("Boundaries");
        Physics2D.IgnoreLayerCollision(playerLayer, boundariesLayer);
        Physics2D.IgnoreLayerCollision(groundLayer, boundariesLayer);
        Physics2D.IgnoreLayerCollision(enemyLayer, boundariesLayer);
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        vcam.m_Follow = player.transform;
    }

    private void FixedUpdate() {
        if(repositionCamera && vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping >= 0.3001) {
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping -= .3f;
            if (vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping == 0)
            {
                repositionCamera = false;
            }
        }
    }

    #region Getters-Setters
    public bool RecolocarCamara {
        get { return repositionCamera; }
        set { this.repositionCamera = value; }
    }

    public bool InputEnabled {
        get { return inputEnabled; }
        set { this.inputEnabled = value; }
    }

    public System.Random Random {
        get { return rnd; }
    }
    #endregion

}