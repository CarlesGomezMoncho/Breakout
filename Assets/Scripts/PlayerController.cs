using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float limiteHorizontal = 10;
    public float velocidad = 10;
<<<<<<< HEAD
    public float maxTimeBetweenBullets = 1;
    public float minHoldTime = 0.25f;    //temps maxim de apretar, si se sobrepasa se considera que no es tap, i per tant no dispararà o soltarà la pilota

    public GameObject bullet;
    public Transform gun1;
    public Transform gun2;
=======
>>>>>>> parent of 768f72d... Molts canvis i bugs arreglats

    private float spriteWidth;
    private GameObject pelota;
    private bool IAEnabled = false;

    private float posicionX;
    private float posicionY;

    private float oldHorizontal;

    private Vector2 initSpriteSize, initColliderSize;

    private Vector2 touchPosition;

    private float startHoldTime = 0;

    void Start()
    {
        GetSpriteWidth();

        oldHorizontal = 0;
        posicionX = 0;
        posicionY = transform.position.y;

<<<<<<< HEAD
        raquetaNormal.SetActive(true);

        //initSpriteSize = GetComponent<SpriteRenderer>().size;
        //initColliderSize = GetComponent<BoxCollider2D>().size;
        initSpriteSizeNormal = raquetaNormal.GetComponent<SpriteRenderer>().size;
        initColliderSizeNormal = raquetaNormal.GetComponent<BoxCollider2D>().size;
        initSpriteSizeDispar = raquetaDispar.GetComponent<SpriteRenderer>().size;
        initColliderSizeDispar = raquetaDispar.GetComponent<BoxCollider2D>().size;

        limiteHorizontal = limiteHorizontalRaquetaNormal;
=======
        initSpriteSize = GetComponent<SpriteRenderer>().size;
        initColliderSize = GetComponent<BoxCollider2D>().size;
>>>>>>> parent of 768f72d... Molts canvis i bugs arreglats
    }

    void Update()
    {
        float horizontal;

        if (IAEnabled)
        {
            //si no hay pelota asignada
            if (pelota == null)
            {
                if (GameObject.FindGameObjectsWithTag("Pelota").Length > 0)
                {

                    pelota = GameObject.FindGameObjectsWithTag("Pelota")[0];
                }
            }
            else
            {
                //raqueta más a la derecha que la pelota
                if (transform.position.x > pelota.transform.position.x + 0.5f)
                {
                    horizontal = Mathf.Lerp(oldHorizontal, -1f, 0.1f);
                    oldHorizontal = horizontal;

                }
                //raqueta más a la izquierda que la pelota
                else if (transform.position.x < pelota.transform.position.x - 0.5f)
                {
                    horizontal = Mathf.Lerp(oldHorizontal, 1f, 0.1f);
                    oldHorizontal = horizontal;
                }
                else
                    horizontal = 0;

                posicionX = transform.position.x + horizontal * velocidad * Time.deltaTime;
                posicionY = transform.position.y;
            }



        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");

            if (horizontal != 0)
            {
                posicionX = transform.position.x + horizontal * velocidad * 5 * Time.deltaTime; //multipliquem per a accelarar, ja que està configurat per a mobil i no per a teclat
                posicionY = transform.position.y;

            }
          
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                //si comença a presionar
                if (touch.phase == TouchPhase.Began)
                {
                    //guardem posició inicial, que es la que gastarem per a restar a la del moviment i la sumarem a la paleta
                    touchPosition = touch.position;

                    StarHolding();
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 pos = touch.position;

                    //canviem la posició actual per la inicial - la nova
                    posicionX = transform.position.x - ((touchPosition.x - pos.x) * velocidad * Time.deltaTime);
                    posicionY = transform.position.y;

                    //actualitzem posicio inicial
                    touchPosition = pos;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    List<GameObject> listaPelotas = GameController.instance.GetListaPelotas();
                    
                    //si hay pelotas y si tap pantalla
                    if (listaPelotas.Count > 0 && EndHolding() <= minHoldTime)
                    {
                        //iniciamos movimiento de todas las pelotas
                        foreach (GameObject g in listaPelotas)
                        {
                            g.GetComponent<BallController>().IniciarMovimiento();
                            g.GetComponent<BallController>().Despegar();
                        }

                        //si estamos en raqueta de disparo
                        if (GameController.instance.GetActiveRaqueta() == "DisparItem")
                        {
                            GameObject bullet = Shoot();
                            if (bullet)
                            {
                                bullet.GetComponent<BulletController>().tileMap = GameController.instance.tileMap;
                            }
                        }
                    }
                }
            }
            //
            else if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
            {
                List<GameObject> listaPelotas = GameController.instance.GetListaPelotas();

                if (listaPelotas.Count > 0)
                {
                    //iniciamos movimiento de todas las pelotas
                    foreach (GameObject g in listaPelotas)
                    {
                        g.GetComponent<BallController>().IniciarMovimiento();
                        g.GetComponent<BallController>().Despegar();
                    }
                    //si estamos en raqueta de disparo
                    if (GameController.instance.GetActiveRaqueta() == "DisparItem")
                    {
                        GameObject bullet = Shoot();
                        if (bullet)
                        {
                            bullet.GetComponent<BulletController>().tileMap = GameController.instance.tileMap;
                        }
                    }
                }
            }
        }

        //si supera el limite máximo por la derecha (- la mitad de su tamaño)
        if (posicionX > limiteHorizontal - spriteWidth / 2)
        {
            //si sobrepasa el limite, lo pone en el limite
            posicionX = limiteHorizontal - spriteWidth / 2;
        }
        else if (posicionX < -limiteHorizontal + spriteWidth / 2)
        {
            posicionX = -limiteHorizontal + spriteWidth / 2;
        }

        transform.position = new Vector2(posicionX, posicionY);
    }

    public float GetSpriteWidth()
    {
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        
        return spriteWidth;
    }

    public void StartIA(bool start = true)
    {
        if (start)
        {
            IAEnabled = true;
        }
        else
        {
            IAEnabled = false;
        }

    }

    public void IncreaseScale(Vector2 increaseScale)
    {
        GetComponent<SpriteRenderer>().size += increaseScale;
        GetComponent<BoxCollider2D>().size += increaseScale;
        GetSpriteWidth();   //para reajustar el limite de movimiento

    }

    public void ResetState()
    {
        GetComponent<SpriteRenderer>().size = initSpriteSize;
        GetComponent<BoxCollider2D>().size = initColliderSize;

        GetSpriteWidth();   //para reajustar el limite de movimiento
    }

    private void StarHolding()
    {
        startHoldTime = Time.time;
    }

    private float EndHolding()
    {
        return Time.time - startHoldTime;
    }
}
