using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public GameObject raqueta;
    public Transform centroRaqueta;
    public GameObject pelota;

    public GameObject PanelStart;

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

        playerController = raqueta.GetComponent <PlayerController>();

        ReStartGame();
    }

    void Update()
    {
        //si pulsamos Espacio o botón de salto en un mando
        if (Input.GetButtonDown("Jump"))
        {
            //si el juego no está iniciado, lo inicia
            if (!gameStarted)
            {
                StartGame();
            }
            //si el juego si está iniciado, inicia el movimiento de la pelota
            else
            {
                ballController.IniciarMovimiento();
            }
        }

    }

    public void ReStartGame()
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

    public void StartGame()
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
}
