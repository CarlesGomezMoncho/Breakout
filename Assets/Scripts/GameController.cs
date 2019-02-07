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

    public int lives = 3;

    public Animator startAnim;
    public Animator fadeAnim;
    public Animator levelCompletedAnim;

    private List<GameObject> listaPelotas;

    private PlayerController playerController;

    private bool gameStarted = false;
    private int numBalls = 0;

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
            gameStarted = true;
            StartLevel();
            livesText.text = "Lives: " + lives;
        }
        else
        {
            StartDemo();
            livesText.text = "";
        }
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
            //si pulsamos Espacio o botón de salto en un mando
            if (Input.GetButtonDown("Jump"))
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
                    Debug.Log("Game Over");
                }
            }

            livesText.text = "Lives: " + lives;
        }
    }

    private void AddPelota()
    {
        GameObject pelota;

        pelota = Instantiate(pelotaBase);

        pelota.GetComponent<BallController>().centroRaqueta = centroRaqueta;
        pelota.GetComponent<BallController>().tileMap = tileMap;
        pelota.GetComponent<BallController>().text = DebugText;
        listaPelotas.Add(pelota);
        numBalls++;

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
        gameStarted = false;

        //añadimos una pelota
        AddPelota();

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
        gameStarted = true;

        //quita el panel start
        PanelStart.SetActive(false);

        //añadimos una pelota
        AddPelota();

        //reinicia posición pelota
        listaPelotas[numBalls - 1].transform.position = new Vector2(centroRaqueta.position.x, centroRaqueta.transform.position.y);

        //para el movimiento de la pelota
        //ballController.PararMovimiento();

        //desactiva IA Raqueta
        playerController.StartIA(false);


    }

    public void EndLevel()
    {
        //si no está en pantalla inicial
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            levelCompletedAnim.gameObject.SetActive(true);
            levelCompletedAnim.SetTrigger("endLevel");
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


}
