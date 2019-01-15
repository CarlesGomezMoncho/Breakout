using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public GameObject raqueta;
    public Transform centroRaqueta;
    public GameObject pelota;

    private BallController ballController;

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
        StartGame();

        ballController = pelota.GetComponent<BallController>();
        ballController.centroRaqueta = centroRaqueta;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !gameStarted)
        {
            ballController.Iniciar();
            gameStarted = true;
        }

    }

    public void StartGame()
    {
        pelota.transform.position = new Vector2(centroRaqueta.position.x, centroRaqueta.transform.position.y);
    }
}
