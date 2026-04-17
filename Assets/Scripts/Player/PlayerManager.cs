using UnityEngine;
using System.Collections;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] float hazardDamage = 5f;

    public float colorDelay;
    public Color damageColor = Color.red;
    public Color startColor = Color.white;
    public Color transColor = Color.white;


    Renderer rend;
    BoxCollider2D feetCollider;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rend = GetComponent<Renderer>();
        feetCollider = GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {
        Hazards();
    }

    void Hazards()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            HealthController.instance.DamageTaken(hazardDamage);

        }
    }

    public void turnRed()
    {
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        rend.material.color = damageColor;
        yield return new WaitForSeconds(colorDelay);
        rend.material.color = startColor;
    }

    public void turnTransparent()
    {
        StartCoroutine(FlashTransparent());
    }

    IEnumerator FlashTransparent()
    {
        rend.material.color = transColor;
        yield return new WaitForSeconds(colorDelay);
        rend.material.color = startColor;
    }

}
