using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

public class BallController : MonoBehaviour
{
    public Transform centroRaqueta;
    public Tilemap tileMap;
    public TextMeshProUGUI text;

    public int intervaloIncremento = 10;
    public float impulsoIncremento = 0.05f;
    public float fuerzaInicial = 2;

    private bool iniciada = false;
    private bool impulsa = false;

    private int numCollisions;

    private Rigidbody2D rb;

    private AudioSource collisionSound;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionSound = GetComponents<AudioSource>()[0];
    }

    void Update()
    {
        if (!iniciada)
        {
            transform.position = centroRaqueta.position;
        }
        else
        {

            //si se queda fijo en un eje, le modificamos un poco su velocidad
            if (rb.velocity.x <= 0.1f && rb.velocity.x >= -0.1f) 
            //if (rb.velocity.x == 0)
            {
                float rnd = Random.value;
                float x;

                if (rnd > 0.5f)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }

                //rb.velocity = rb.velocity + new Vector2(1, 0);
                rb.velocity = new Vector2(x, rb.velocity.y - 1f).normalized * rb.velocity.magnitude;
            }
            if (rb.velocity.y <= 0.1f && rb.velocity.y >= -0.1f)
            //if (rb.velocity.y == 0)
            {
                float rnd = Random.value;
                float y;

                if (rnd > 0.5f)
                {
                    y = 1;
                }
                else
                {
                    y = -1;
                }

                //rb.velocity = rb.velocity + new Vector2(0, 1);
                rb.velocity = new Vector2(rb.velocity.x - 1f, y).normalized * rb.velocity.magnitude;
            }
        }

        if (text)
        {
            text.text = "direction: " + rb.velocity.normalized.ToString();
            text.text = text.text + "\nVelocity: " + rb.velocity.magnitude;
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
        if (!iniciada)
        {
            iniciada = true;
            impulsa = true; //impulso inicial
        }
    }

    public void PararMovimiento()
    {
        iniciada = false;
        rb.velocity = Vector2.zero; //frenamos la pelota
    }

    //impulsa aleatoriamente la pelota hacia arriba
    public void Impulsa(float impulso)
    {
        //si no hay una velocidad distinta de 0
        if (rb.velocity.magnitude != 0)
        {
            //solo añade velocidad
            rb.AddForce(rb.velocity * impulso, ForceMode2D.Impulse);
        }
        //si está parada
        else
        {
            //añade una dirección aleatoria y un impulso
            rb.AddForce(GetNewDirection() * impulso, ForceMode2D.Impulse);
        }
        
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

            //si limpiamos tiles cambiamos de nivel
            if (GameController.instance.TileCount() == 0)
            {
                GameController.instance.EndLevel();
            }

            collisionSound.pitch = 1.5f;
            collisionSound.Play();
            GameController.instance.CreateItem(hitPosition);
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
            //rb.velocity = rb.velocity + new Vector2(x, 0).normalized;
            rb.velocity = new Vector2(rb.velocity.x + x, rb.velocity.y).normalized * rb.velocity.magnitude;

            collisionSound.pitch = 1.2f;
            collisionSound.Play();
        }
        else if (collision.gameObject.name == "Indestructible")
        {
            collisionSound.pitch = 2.0f;
            collisionSound.Play();
        }
        else
        {
            //si colisiona con cualquier otra cosa
            collisionSound.pitch = 1f;
            collisionSound.Play();
        }

        numCollisions++;

        if (numCollisions % intervaloIncremento == 0)
        {
            Impulsa(impulsoIncremento);
        }
    }
    
}
