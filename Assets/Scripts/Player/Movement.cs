using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration;
    [SerializeField] private float shipMaxVelocity;

    //Variables de movimiento
    private float horizontal;
    private float vertical;
    private Vector2 movementVector;
    private Vector3 mousePosition;

    //Variables de control
    private bool isAlive = true;

    //Llamada a Componentes y otros objetos
    private Rigidbody2D shipRigidbody;
    [SerializeField] Camera mainCamera;

    private void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }
    void Start()
    {
        #region inciacion de variables
        shipAcceleration = 5f;
        shipMaxVelocity = 5f;
        #endregion
    }
    private void Update()
    {
        
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isAlive)
        {
            lookAtCursor();
            movimiento();
        }
    }

    private void movimiento()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        movementVector = new Vector2(horizontal, vertical);
        //shipRigidbody.AddForce(shipAcceleration * transform.up);
        shipRigidbody.AddForce(shipAcceleration * movementVector);

        shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
    }
    private void lookAtCursor()
    {
        //uso esta funcion con la finalidad de en base a unos calculos (rotacion = direccion?) tranformar los radianes a grados
        //del vector resultado de la resta de la posicion del objetivo en este caso el cursor y el jugador,
        //para poder en un entorno 2D transmitir la rotacion en grados

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
