using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private PolygonCollider2D playerCollider;

    [SerializeField] private GameObject particleContainer;
    [SerializeField] private ParticleSystem destroyParticles;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Shield shield;
    [SerializeField] private Armor armor;
    [SerializeField] private Vector3 cameraOriginalPosition;

    [SerializeField] private bool test;
    [SerializeField] private bool isHit;
    private float invulnerabilityTimer;
    private float cameraShakeTime;
    private float cameraShakeValue;

    private void Awake()
    {
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer");
        playerCollider = GetComponent<PolygonCollider2D>();
        render = GameObject.Find("PlayerSprite").GetComponent<SpriteRenderer>();
        cameraTransform = Camera.main.transform;
    }

    void Start()
    {
        shield = UpgradeManager._upgradeInstance.getEquipment("Shield").GetComponent<Shield>();
        armor = UpgradeManager._upgradeInstance.getEquipment("Armor").GetComponent<Armor>();

        cameraShakeTime = 0.2f;
        cameraShakeValue = 0.2f;
        cameraOriginalPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            //Boolean para pruebas
            test = false;

        }
        //aqui controlamos los efectos de recibir daño y invulnerabilidad que dependen del Time.DeltaTime.
        #region damage Effects / Invulnerability control
        if (isHit)
        {
            if (cameraShakeTime >= 0)
            {
                cameraTransform.localPosition = cameraOriginalPosition + Random.insideUnitSphere * cameraShakeValue;

                cameraShakeTime -= Time.deltaTime;
            }else
            {
                cameraShakeTime = 0;
                cameraTransform.localPosition = cameraOriginalPosition;
            }
                
            invulnerabilityTimer += Time.deltaTime;
            if(invulnerabilityTimer >= 1)
            {
                invulnerabilityTimer = 0;
                isHit = false;
            }
        }else
        {

        cameraTransform.localPosition = cameraOriginalPosition;
        cameraShakeTime = 0.2f;
        }
        #endregion

    }
    private void playParticle()
    {
        Instantiate(destroyParticles, transform.position, Quaternion.identity, particleContainer.transform);

    }
    private void takeDmg()
    {
        //control de fase de invulnerable a no invulnerable
        if (!shield.Invulnerable)
        {
            //animacion de recibir daño
            damageTick();
            //Control de daños del personaje
            armor.loseHealth(1);
            if (armor.Life <= 0)
            {
                InGameMenuManager._instance.openGameOverMenu();
                playParticle();
                Destroy(gameObject);
            }
        }
        else if (shield.Invulnerable)
        {
            //aqui preparamos un pequeño delay para que en medio de la invulnerabilidad el escudo se rompa
            StartCoroutine(shield.delayInvulnerability());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //con la variable isHit controlamos un segundo de invulnerabilidad, para poder usar correctamente
        // on trigger stay, debido a que on trigger enter supone algunos problemas en cuanto a permanecer 
        //dentro del enemigo.
        if (!isHit)
        {
            if (collision.CompareTag("Asteroid") || collision.CompareTag("EnemyBullet"))
            {
                isHit = true;
                takeDmg();
            }
        }
    }

    private void damageTick()
    {
            render.color = Color.red;
            StartCoroutine(whitecolor());
    }
    private IEnumerator whitecolor()
    {
        yield return new WaitForSeconds(0.2f);
        render.color = Color.white;
    }
}
