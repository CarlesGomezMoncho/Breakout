using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public GameObject raqueta;
    public Transform centroRaqueta;
    public GameObject pelotaBase;

    public GameObject PanelStart;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI DebugText;

    public Tilemap tileMap;

    public int initialLives = 2;
    public static int lives = 2;    //para acceso desde cualquier escena y siempre con el mismo valor LOS STATICS NO SALEN EN EL INSPECTOR

    public Animator startAnim;
    public Animator fadeAnim;
    public Animator levelCompletedAnim;
    public Animator GameOverAnim;

    public List<GameObject> itemsInLevel;

    private List<GameObject> listaPelotas;

    private PlayerController playerController;

    //private bool gameStarted = false;
    private int numBalls = 0;

    private AudioSource levelCompletedSound;

    private void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    void Start()
    {

        listaPelotas = new List<GameObject>();

        playerController = raqueta.GetComponent <PlayerController>();

        levelCompletedAnim.gameObject.SetActive(false);

        //si no estamos en la intro
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //gameStarted = true;
            StartLevel();
            livesText.text = "Lives: " + lives;
        }
        else
        {
            StartDemo();
            livesText.text = "";
        }

        levelCompletedSound = GetComponents<AudioSource>()[0];
    }

    void Update()
    {
        //si estamos en demo
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //si pulsamos Espacio o botón de salto en un mando
            if (Input.GetButtonDown("Jump"))
            {
                // inicia el juego 
                startAnim.SetTrigger("StartGame");
                //inicializamos las vidas
                lives = initialLives;
            }

            //si se destruye una pelota, iniciamos de nuevo
            if (numBalls <= 0)
            {
                StartDemo();
            }
        }
        //si no estamos en demo
        else
        {
            //si hay pelotas y si pulsamos Espacio o botón de salto en un mando
            if (listaPelotas.Count > 0 && Input.GetButtonDown("Jump"))
            {
                //si el juego si está iniciado, inicia el movimiento de la primera (y unica) pelota 
                listaPelotas[0].GetComponent<BallController>().IniciarMovimiento();
            }

            //si no hay pelotas en juego y aun quedan tiles
            if (numBalls <= 0 && TileCount() > 0)
            {
                //si quedan vidas
                if (lives > 0)
                {
                    lives--;
                    StartLevel();
                }
                else
                {
                    //game over
                    GameOverAnim.SetTrigger("Start");

                    //si pulsamos salto
                    if (Input.GetButtonDown("Jump"))
                    {
                        GameOverAnim.SetTrigger("End");    //reinicia juego
                    }
                }
            }

            livesText.text = "Balls: " + lives;
        }
    }

    public GameObject AddPelota(Transform posicion = null)
    {
        GameObject pelota;
        BallController bController; 

        pelota = Instantiate(pelotaBase);
        bController = pelota.GetComponent<BallController>();

        if (posicion == null)
        {
            posicion = listaPelotas[0].transform;   //la posición de la primera pelota
        }

        bController.centroRaqueta = posicion;
        bController.tileMap = tileMap;
        bController.text = DebugText;
        listaPelotas.Add(pelota);
        numBalls++;

        return pelota;
    }

    //quitamos una pelota
    public void RemovePelota(GameObject pelotaAQuitar)
    {
        numBalls--;
        listaPelotas.Remove(pelotaAQuitar);
        Destroy(pelotaAQuitar);
    }


    public void StartDemo()
    {
        //iniciamos pantalla Start
        PanelStart.SetActive(true);

        //Pone el juego en estado no activo
        //gameStarted = false;

        //añadimos una pelota
        AddPelota(centroRaqueta);

        //reinicia posición pelota
        listaPelotas[numBalls - 1].transform.position = new Vector2(centroRaqueta.position.x, centroRaqueta.transform.position.y);

        //inicia el movimiento de la pelota
        listaPelotas[numBalls - 1].GetComponent<BallController>().IniciarMovimiento();

        //inicia IA Raqueta
        playerController.StartIA();
    }

    public void StartLevel()
    {
        //pone el juego en estado activo
        //gameStarted = true;

        //quita el panel start
        PanelStart.SetActive(false);

        //añadimos una pelota
        AddPelota(centroRaqueta);

        //reinicia posición pelota
        listaPelotas[numBalls - 1].transform.position = new Vector2(centroRaqueta.position.x, centroRaqueta.transform.position.y);

        //para el movimiento de la pelota
        //ballController.PararMovimiento();

        //desactiva IA Raqueta
        playerController.StartIA(false);

        //reseteamos posibles estados alterados de la raqueta
        playerController.ResetState();

        //quitamos items en pantalla
        RemoveAllItems();
    }

    public void EndLevel()
    {
        //si no está en pantalla inicial
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            levelCompletedAnim.gameObject.SetActive(true);
            levelCompletedAnim.SetTrigger("endLevel");
            levelCompletedSound.Play();
        }
        else
            SceneManager.LoadScene(0);
    }

    public int TileCount()
    {
        //Contamos el numero de tiles que existen
        BoundsInt bounds = tileMap.cellBounds;
        TileBase[] allTiles = tileMap.GetTilesBlock(bounds);
        int numBlocks = 0;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    numBlocks++;
                }
            }
        }

        return numBlocks;
    }

    public void CreateItem(Vector2 position)
    {
        int numItems = itemsInLevel.Count;

        //si quedan items intentamos crear uno
        if (numItems > 0)
        {
            int numTiles = TileCount() + 1; //se suma 1 por que en este momento ya no se cuenta el tile que se acaba de destruir (y hay que contarlo)
            int rand = Random.Range(0, numTiles);

            //si el numero aleatoria cae dentro del numero de items que quedan
            if (rand < numItems)
            {
                GameObject item = Instantiate(itemsInLevel[rand]);
                item.transform.position = position;
                itemsInLevel.RemoveAt(rand);
            }
        }
    }

    public void RemoveAllItems()
    {
        foreach (GameObject g in FindObjectsOfType<GameObject>())
        {
            if (g.layer == LayerMask.NameToLayer("Item"))
            {
                Destroy(g);
            }
        }
    }

    //coge una pelota aleatoria si quedan
    public GameObject GetRandomBall()
    {
        GameObject g = null;

        while (g == null)
        {
            //coge una pelota aleatoria
            g = listaPelotas[Random.Range(0, listaPelotas.Count)];

            //si solo hay 1 y va a destruir-se
            if (listaPelotas.Count == 1 && g.GetComponent<BallController>().IsDestroying())
            {
                return null;
            }
        }

        return g;
    }
}
