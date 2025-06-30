
using UnityEngine;

public class Wrap : MonoBehaviour
{
    private void Update()
    {
        //Lo que detecta si estas fuera de la camara y se debe transformar su ubicacion
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        //Lo que se encarga de mover al jugador en el eje de cordenadas comprobando si esta
        //en 0 o en 1 contrario sumando o restando en su valor en las cordenadas
        Vector3 moveAdjustment = Vector3.zero;
        if (viewportPosition.x < 0)
        {
            moveAdjustment.x += 1;
        }
        else if (viewportPosition.x > 1) 
        {
            moveAdjustment.x -= 1;
        }
        else if (viewportPosition.y < 0)
        {
            moveAdjustment.y += 1;
        }
        else if (viewportPosition.y > 1)
        {
            moveAdjustment.y -= 1;
        }
        //Despues de modificar el valor se mueve al jugador añadiendo el nuevo valor
        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjustment);
    }
}
