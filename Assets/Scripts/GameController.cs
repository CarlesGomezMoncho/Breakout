using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public GameObject raqueta;
    public Transform centroRaqueta;
    public GameObject pelota;

    public GameObject PanelStart;

    public Tilemap tileMap;

    public Animator startAnim;
    public Animator fadeAnim;
    public Animator levelCompletedAnim;

    private BallController ballController;
    private PlayerController playerController;

    private bool gameStarted = false;

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
        ballController = pelota.GetComponent<BallController>();
        ballController.centroRaqueta = centroRaqueta;
        ballController.tileMap = tileMap;

        playerController = raqueta.GetComponent <PlayerController>();

        levelCompletedAnim.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            gameStarted = true;
            StartLevel();
        }
        else
        {
            StartDemo();
        }
    }

    void Update()
    {
        //si pulsamos Espacio o botón de salto en un mando
        if (Input.GetButtonDown("Jump"))
        {
            //si el juego no está iniciado, lo inicia y carga el primer nivel
            if (!gameStarted)
            {
                startAnim.SetTrigger("StartGame");
                //SceneManager.LoadScene(1);
            }
            //si el juego si está iniciado, inicia el movimiento de la pelota
            else
            {
                ballController.IniciarMovimiento();
            }
        }

    }

    public void StartDemo()
    {
        //iniciamos pantalla Start
        PanelStart.SetActive(true);

        //Pone el juego en estado no activo
        gameStarted = false;

        //reinicia posición pelota
        pelota.transform.position = new Vector2(centroRaqueta.position.x, centroRaqueta.transform.position.y);

        //inicia el movimiento de la pelota
        ballController.IniciarMovimiento();

        //inicia IA Raqueta
        playerController.StartIA();
    }

    public void StartLevel()
    {
        //pone el juego en estado activo
        gameStarted = true;

        //quita el panel start
        PanelStart.SetActive(false);

        //reinicia posición pelota
        pelota.transform.position = new Vector2(centroRaqueta.position.x, centroRaqueta.transform.position.y);

        //para el movimiento de la pelota
        ballController.PararMovimiento();

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
