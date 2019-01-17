using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BallController : MonoBehaviour
{
    public Transform centroRaqueta;
    public Tilemap tileMap;
    public TextMeshProUGUI text;

    public float fuerzaInicial = 2;

    private bool iniciada = false;
    private bool impulsa = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!iniciada)
            transform.position = centroRaqueta.position;

        //si se queda fijo en un eje, le modificamos un poco su velocidad
        if(rb.velocity.x == 0)
        {
            rb.velocity = rb.velocity + new Vector2(1, 0);
        }

        if (rb.velocity.y == 0)
        {
            rb.velocity = rb.velocity + new Vector2(0, 1);
        }

        if (text)
        {
            text.text = rb.velocity.ToString();
        }
    }

    private void FixedUpdate()
    {
        if(impulsa)
        {
            Impulsa(fuerzaInicial);
        }
    }

    public void IniciarMovimiento()
    {
        iniciada = true;
        impulsa = true; //impulso inicial
    }

    //impulsa aleatoriamente la pelota hacia arriba
    public void Impulsa(float impulso)
    {
        rb.AddForce(GetNewDirection() * impulso, ForceMode2D.Impulse);
        impulsa = false;
    }

    //impulsa la pelota en una dirección concreta
    public void Impulsa(float impulso, Vector2 direccion)
    {
        rb.AddForce(direccion * impulso, ForceMode2D.Impulse);
        impulsa = false;
    }

    private Vector2 GetNewDirection()
    {
        Vector2 direction;

        int randY = 1;
        float randX = Random.Range(-0.5f, 0.5f); //valores entre -45º y 45º
        //float randX = 0;  //para debug, sale completamente vertical

        direction = new Vector2(randX, randY);

        return direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap")
        {
            Vector3 hitPosition = Vector3.zero;
            Vector3 hitPositionI = Vector3.zero;    //por si golpea muy cerca de otro tile a la izquierda
            Vector3 hitPositionD = Vector3.zero;    //igual pero derecha

            //Solo si está cerca de otro tile se crearán distintos hitPosition, sinó está cerca se pondrá a null el mismo tile 3 veces
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x;
                hitPosition.y = hit.point.y;

                hitPositionI.x = hit.point.x - 0.1f;
                hitPositionI.y = hit.point.y;

                hitPositionD.x = hit.point.x + 0.1f;
                hitPositionD.y = hit.point.y;

                tileMap.SetTile(tileMap.WorldToCell(hitPosition), null);
                tileMap.SetTile(tileMap.WorldToCell(hitPositionI), null);
                tileMap.SetTile(tileMap.WorldToCell(hitPositionD), null);
            }
        }
        else if (collision.gameObject.name == "Raqueta")
        {
            //Modificamos el grado de la dirección de la pelota dependiendo donde colisiona la pelota con la raqueta
            //cuando más se acerque al borde derecho, más se acercará 1 (la desviación)
            //si esta en el centro será 0
            //cuando más se acerque al borde izquierdo, más se acercará a -1

            //restamos a la posición de la raqueta, la posición de la pelota, para conocer la posición relativa entre ellas dos en el eje x
            float x = (transform.position.x - collision.transform.position.x);

            //si es menor que 0.5 o mayor que -0.5 vamos a indicar que golpea en el centro de la raqueta, por tanto no habrá una modficación en la dirección
            if (x < 0.5f && x > -0.5f)
                x = 0;

            //modificamos la velocidad actual normalizando el valor obtenido antes, de tal forma que si golpea en el centro de la raqueta la pelota no modifica
            //la dirección. En cambio si golpea cerca de los bordes, se modifica la dirección actual en +1 (si golpea en la parte derecha) o en -1
            //(si golpea en la parte izquierda) como el valor sera un valor decimal se normaliza, para que si es mayor que 0 sea 1 y si es menor que 0
            //sea -1 y asi se modificará en cada golpeo la dirección de la pelota en +1, 0 o -1 dependiendo de la zona en la que golpee la pelota en la raqueta.
            rb.velocity = rb.velocity + new Vector2(x, 0).normalized;
        }
    }
}
