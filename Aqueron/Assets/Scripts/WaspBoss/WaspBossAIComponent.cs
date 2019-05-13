using System.Collections;
using UnityEngine;


public class WaspBossAIComponent : MonoBehaviour {

    [SerializeField]
    private GameObject[] holes;
    private GameObject[] minions;

    private GameObject eva;
    private BaseStatsComponent statsComponent;
    private BaseDamageComponent damageComponent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bodyCollider;
    private CircleCollider2D stingCollider;
    private Coroutine actionCoroutine;
    private Vector2 nextHolePosition;

    [Header("Cambio de agujero")]
    [SerializeField] private float idleTime;
    [SerializeField] private float flightTime;
    [SerializeField] private float fadeOutTime;

    [Header("Ataque Escondido")]
    [SerializeField] private float fadeInTime;
    [SerializeField] private float hidingTime;
    [SerializeField] private float minionsCastingTime;

    [Header("Ataque picotazo")]
    [SerializeField] private float percentageHealth;
    [SerializeField] private float stingCastingTime;

    private bool runningAction;
    private float numberHoles, fadeOutRatio;

    public enum StateMachine {
        Start, ChangingHole, HidingAttack, StingAttack
    }
    private StateMachine waspState;

    #region Getters-Setters
    public StateMachine WaspState {
        get {
            return waspState;
        }
    }
    #endregion

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        statsComponent = gameObject.GetComponent<BaseStatsComponent>() as BaseStatsComponent;
        if (statsComponent == null) {
            Debug.LogError("ERROR: No se ha detectado componente de Stats en Jefe Avispa");
            Debug.Break();
        }
        damageComponent = gameObject.GetComponent<BaseDamageComponent>() as BaseDamageComponent;
        if (damageComponent == null) {
            Debug.LogError("ERROR: No se ha detectado componente de Damage en Jefe Avispa");
        }
        numberHoles = holes.Length;
        fadeOutRatio = 1 / fadeOutTime;
    }

    private void Update() {

        if (!runningAction) {
            if (statsComponent.PercentageHealth < percentageHealth) {
                runningAction = true;
                actionCoroutine = StartCoroutine("StingAttack");
            }
            else {
                switch (waspState) {
                    case StateMachine.Start:
                        runningAction = true;
                        actionCoroutine = StartCoroutine("StartCombat");
                        break;
                    case StateMachine.ChangingHole:
                        runningAction = true;
                        actionCoroutine = StartCoroutine("ChangeHole");
                        break;
                    case StateMachine.HidingAttack:
                        runningAction = true;
                        actionCoroutine = StartCoroutine("HideAttack");
                        break;
                }
            }
        }
    }

    private IEnumerator StartCombat() {
        //Cosas de inicio de combate: Cerrar puertas, sonidos, blablablabla
        //Cambio a siguiente estado
        waspState = StateMachine.ChangingHole;
        runningAction = false;
        yield return null;
    }

    private IEnumerator ChangeHole() {

        // Duración total: idleTime + flightTime + fadeout segundos

        // Elegir agujero del panal al que moverse (idleTime segundos)
        int nextHole = (int)Random.Range(0, numberHoles);
        nextHolePosition = holes[nextHole].transform.position;
        yield return new WaitForSeconds(idleTime);

        while (transform.position != holes[nextHole].transform.position) {
            transform.position = Vector2.MoveTowards(transform.position, nextHolePosition, statsComponent.Speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // FadeOut (fadeOutTime)
        bodyCollider.enabled = false;
        while (spriteRenderer.color.a != 0) {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - fadeOutRatio * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Cambio a siguiente estado
        waspState = StateMachine.HidingAttack;
        runningAction = false;
        yield return null;
    }

    private IEnumerator HideAttack() {
        // Duración total: 10 segundos
        // Fade In en centro (1 segundo)
        // Casteo de avispas pequeñas (.5 segundos)
        // Ataques de abejas (8 segundos)

        //Cambio a siguiente estado
        waspState = StateMachine.ChangingHole;
        runningAction = false;
        yield return null;
    }

    private IEnumerator StingAttack() {
        // Duracion total: Indefinida (termina al morir)
        // Acercarse a Eva (?? segundos)
        // Castear ataque (1.5 segundos)
        // Atacar (?? segundos)
        yield return null;
    }

    public void Death() {
        //Detener ataque actual
        StopCoroutine(actionCoroutine);
        runningAction = true;
        //Animación de muerte

        //Abrir puertas, dar recompensas bla bla bla


        //Eliminar gameobject
        Destroy(gameObject);
    }
}
