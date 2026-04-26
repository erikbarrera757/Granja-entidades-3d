using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public RectTransform[] slots;
    public Image[] slotImages;
    public Image[] slotIcons;

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
            {
                targetPosition += selectedOffset;
            }

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

            if (slotIcons != null && i < slotIcons.Length && slotIcons[i] != null)
            {
                bool hasItem = false;

                if (playerInteraction != null)
                {
                    if (i == 0 && playerInteraction.hasFlashlight)
                        hasItem = true;

                    if (i == 1 && playerInteraction.hasCrucifix)
                        hasItem = true;
                }

                Color iconTargetColor;

                if (hasItem)
                {
                    iconTargetColor = (i == selectedSlot)
                        ? Color.white
                        : new Color(1f, 1f, 1f, 0.5f);
                }
                else
                {
                    iconTargetColor = new Color(1f, 1f, 1f, 0f);
                }

                slotIcons[i].color = Color.Lerp(
                    slotIcons[i].color,
                    iconTargetColor,
                    Time.deltaTime * smoothSpeed
                );
            }
        }
    }

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }
}