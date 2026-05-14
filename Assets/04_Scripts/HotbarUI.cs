using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    public RectTransform[] slots;
    public Image[] slotImages;
    public Image[] slotIcons;
    public TextMeshProUGUI[] countTexts;

    public PlayerInteraction playerInteraction;

    public Vector3 normalScale = Vector3.one;
    public Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    public Vector2 selectedOffset = new Vector2(0, 10f);

    public float smoothSpeed = 10f;

    public Color normalColor = new Color(1f, 1f, 1f, 0.6f);
    public Color selectedColor = Color.white;

    public int selectedSlot = 0;

    private Vector2[] originalPositions;

    void Start()
    {
        originalPositions = new Vector2[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            originalPositions[i] = slots[i].anchoredPosition;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);

        UpdateHotbarVisual();
    }

    void SelectSlot(int index)
    {
        selectedSlot = index;
    }

    void UpdateHotbarVisual()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Vector3 targetScale = (i == selectedSlot) ? selectedScale : normalScale;
            Vector2 targetPosition = originalPositions[i];

            if (i == selectedSlot)
                targetPosition += selectedOffset;

            slots[i].localScale = Vector3.Lerp(
                slots[i].localScale,
                targetScale,
                Time.deltaTime * smoothSpeed
            );

            slots[i].anchoredPosition = Vector2.Lerp(
                slots[i].anchoredPosition,
                targetPosition,
                Time.deltaTime * smoothSpeed
            );

            if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
            {
                Color targetColor = (i == selectedSlot) ? selectedColor : normalColor;

                slotImages[i].color = Color.Lerp(
                    slotImages[i].color,
                    targetColor,
                    Time.deltaTime * smoothSpeed
                );
            }

            bool hasItem = HasItemInSlot(i);
            int count = GetCountForSlot(i);

            if (slotIcons != null && i < slotIcons.Length && slotIcons[i] != null)
            {
                Color iconTargetColor = hasItem
                    ? ((i == selectedSlot) ? Color.white : new Color(1f, 1f, 1f, 0.5f))
                    : new Color(1f, 1f, 1f, 0f);

                slotIcons[i].color = Color.Lerp(
                    slotIcons[i].color,
                    iconTargetColor,
                    Time.deltaTime * smoothSpeed
                );
            }

            if (countTexts != null && i < countTexts.Length && countTexts[i] != null)
            {
                if (count > 0)
                {
                    countTexts[i].text = "x" + count;
                    countTexts[i].gameObject.SetActive(true);
                }
                else
                {
                    countTexts[i].gameObject.SetActive(false);
                }
            }
        }
    }

    bool HasItemInSlot(int index)
    {
        if (playerInteraction != null)
        {
            if (index == 0 && playerInteraction.hasFlashlight) return true;
            if (index == 1 && playerInteraction.hasCrucifix) return true;
            if (index == 2 && playerInteraction.smallFoodCount > 0) return true;
            if (index == 3 && playerInteraction.mediumFoodCount > 0) return true;
            if (index == 4 && playerInteraction.largeFoodCount > 0) return true;
        }

        if (index == 5 && GameManager.Instance != null && GameManager.Instance.entityFood > 0)
            return true;

        return false;
    }

    int GetCountForSlot(int index)
    {
        if (playerInteraction != null)
        {
            if (index == 2) return playerInteraction.smallFoodCount;
            if (index == 3) return playerInteraction.mediumFoodCount;
            if (index == 4) return playerInteraction.largeFoodCount;
        }

        if (index == 5 && GameManager.Instance != null)
            return GameManager.Instance.entityFood;

        return 0;
    }

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }
}