using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    [Header("Heart Display Settings")]
    public GameObject heartPrefab;
    public Transform heartsContainer;
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;

    [Header("Layout Settings")]
    public int heartsPerRow = 5;
    public float heartSpacing = 35f;
    public Vector2 heartSize = new Vector2(30f, 30f);
    public float heartScale = 1f;

    [Header("Positioning Settings")]
    public bool alignLeft = true;
    public Vector2 startOffset = new Vector2(0f, 0f);

    private List<Image> heartImages = new List<Image>();
    private HealthController healthController;

    void Start()
    {
        healthController = HealthController.instance;
        ContainerSetup();
        CreateHeartDisplay();
        UpdateHeartDisplay();
    }

    void Update()
    {
        UpdateHeartDisplay();
    }

    void ContainerSetup()
    {
        RectTransform containerRect = heartsContainer.GetComponent<RectTransform>();

        if (alignLeft)
        {
            // set anchor of the hearts and mvoe it to top left of screen
            containerRect.anchorMin = new Vector2(0, 1);
            containerRect.anchorMax = new Vector2(0, 1);
            containerRect.pivot = new Vector2(0, 1);

            // the position from the top left corner
            containerRect.anchoredPosition = new Vector2(startOffset.x, -startOffset.y);
        }
    }

    void CreateHeartDisplay()
    {
        if (healthController == null) return;

        // clear any exisitng hearts 
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        // calculate how many heart objects we need (for half hearts, we need one heart per 2 health)
        int heartObjectsNeeded = Mathf.CeilToInt((float)healthController.maxHealth / 2f);

        // create heart objects
        for (int i = 0; i < heartObjectsNeeded; i++)
        {
            GameObject heartObj;

            if (heartPrefab != null)
            {
                heartObj = Instantiate(heartPrefab, heartsContainer);
            }
            else
            {
                // create heart from scratch if no prefab
                heartObj = new GameObject($"Heart_{i}");
                heartObj.transform.SetParent(heartsContainer);
                heartObj.AddComponent<Image>();
            }

            Image heartImage = heartObj.GetComponent<Image>();
            heartImage.sprite = fullHeartSprite;
            heartImages.Add(heartImage);

            // setup heart positioning and scaling
            RectTransform rectTransform = heartObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = heartSize * heartScale;
            rectTransform.localScale = Vector3.one; //  gonna keep scale at 1, use sizeDelta for sizing instead

            int row = i / heartsPerRow;
            int col = i % heartsPerRow;

            if (alignLeft)
            {
                // position from left edge
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);

                rectTransform.anchoredPosition = new Vector2(
                    col * (heartSize.x * heartScale + heartSpacing),
                    -row * (heartSize.y * heartScale + heartSpacing)
                );
            }
            else
            {
                // original center positioning
                rectTransform.anchoredPosition = new Vector2(
                    col * (heartSize.x * heartScale + heartSpacing),
                    -row * (heartSize.y * heartScale + heartSpacing)
                );
            }
        }
    }

    void UpdateHeartDisplay()
    {
        if (healthController == null || heartImages.Count == 0) return;

        float currentHealth = healthController.currentHealth;

        for (int i = 0; i < heartImages.Count; i++)
        {
            float healthForThisHeart = currentHealth - (i * 2); // treating each heart as 2 health

            // currenHealth = 5
            // i = 0  5-(0*2)= 5 >= 2 --> full heart
            // i = 1  5-(1*2)= 3 >= 2 --> full heart
            // i = 2  5-(2*2)= 1 >= 1 --> half heart

            //currentHealth = 3
            // i = 0 3-(0*2)= 3 >= 2 --> full heart
            // i = 1 3-(1*2)= 1 >= 1 --> half heart
            // i = 2 3-(2*2)= -1 doesnt meet both conditions --> empty heart


            if (healthForThisHeart >= 2)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else if (healthForThisHeart >= 1)
            {
                heartImages[i].sprite = halfHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    public void RefreshDisplay()
    {
        ContainerSetup();
        CreateHeartDisplay();
        UpdateHeartDisplay();
    }

    public void SetHeartScale(float newScale)
    {
        heartScale = newScale;
        RefreshDisplay();
    }

    public void SetHeartSize(Vector2 newSize)
    {
        heartSize = newSize;
        RefreshDisplay();
    }
}

