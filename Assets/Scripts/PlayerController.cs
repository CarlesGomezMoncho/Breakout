using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float limiteHorizontal = 10;
    public float velocidad = 10;

    private float spriteWidth;
    private GameObject pelota;
    private bool IAEnabled = false;

    private float oldHorizontal = 0;

    void Start()
    {
        GetSpriteWidth();
        pelota = GameObject.FindGameObjectsWithTag("Pelota")[0];
    }

    void Update()
    {
        float horizontal;
        float posicionX = 0;
        float posicionY = transform.position.y;

        if (IAEnabled)
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
        else
        {
            horizontal = Input.GetAxis("Horizontal");

            if (horizontal != 0)
            {
                posicionX = transform.position.x + horizontal * velocidad * Time.deltaTime;
                posicionY = transform.position.y;

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
}
