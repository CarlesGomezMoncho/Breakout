using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float limiteHorizontal = 10;
    public float velocidad = 10;

    private float spriteWidth;

    void Start()
    {
        GetSpriteWidth();
    }

    void Update()
    {
        float horizontal;
        float posicionX, posicionY;

        horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            posicionX = transform.position.x + horizontal * velocidad * Time.deltaTime;
            posicionY = transform.position.y;

            //si supera el limite máximo por la derecha (- la mitad de su tamaño)
            if (posicionX > limiteHorizontal - spriteWidth/2)
            {
                //si sobrepasa el limite, lo pone en el limite
                posicionX = limiteHorizontal - spriteWidth/2;
            }
            else if (posicionX < -limiteHorizontal + spriteWidth/2)
            {
                posicionX = -limiteHorizontal + spriteWidth/2;
            }
            
            transform.position = new Vector2(posicionX, posicionY);
        }
    }

    public float GetSpriteWidth()
    {
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        
        return spriteWidth;
    }

}
