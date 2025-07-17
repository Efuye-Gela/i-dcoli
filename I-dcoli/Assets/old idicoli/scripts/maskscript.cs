using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeRenderer))]
public class MaskScript : MonoBehaviour
{
    private SpriteShapeRenderer shapeRenderer;
    private MaterialPropertyBlock propertyBlock;
    private bool isPlayerInside = false;

    public float fadeSpeed = 1f;
    public float minAlpha = 0f;
    public float maxAlpha = 1f;

    private Color baseColor;

    void Start()
    {
        shapeRenderer = GetComponent<SpriteShapeRenderer>();
        propertyBlock = new MaterialPropertyBlock();

        baseColor = shapeRenderer.sharedMaterial.GetColor("_Color");

        SetAlpha(maxAlpha);
    }

    void Update()
    {
        float currentAlpha = GetAlpha();
        float targetAlpha = isPlayerInside ? minAlpha : maxAlpha;
        float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        SetAlpha(newAlpha);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer"))
        {
           
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer"))
        {
        
            isPlayerInside = false;
        }
    }

    private float GetAlpha()
    {
        shapeRenderer.GetPropertyBlock(propertyBlock);
        Color color = propertyBlock.GetColor("_Color");

        if (color == default)
            return baseColor.a;

        return color.a;
    }

    private void SetAlpha(float alpha)
    {
        shapeRenderer.GetPropertyBlock(propertyBlock);
        Color color = baseColor;
        color.a = alpha;
        propertyBlock.SetColor("_Color", color);
        shapeRenderer.SetPropertyBlock(propertyBlock);
    }
}
