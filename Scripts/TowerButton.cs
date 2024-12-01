using UnityEngine;
using UnityEngine.UI;
using TMPro; // Добавляем для TextMeshProUGUI

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;
    private TowerManager.TowerOption towerOption;

    private void Awake()
    {
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        // Если компоненты не назначены через инспектор, пытаемся найти их
        if (iconImage == null) iconImage = GetComponentInChildren<Image>();
        if (costText == null) costText = transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();
        if (nameText == null) nameText = transform.Find("Name")?.GetComponent<TextMeshProUGUI>();

        // Логируем ошибки, если что-то не найдено
        if (iconImage == null) Debug.LogError($"Icon Image not found on {gameObject.name}", this);
        if (costText == null) Debug.LogError($"Cost Text not found on {gameObject.name}", this);
        if (nameText == null) Debug.LogError($"Name Text not found on {gameObject.name}", this);
    }

    public void Initialize(TowerManager.TowerOption tower)
    {
        if (tower == null)
        {
            Debug.LogError($"Attempting to initialize {gameObject.name} with null tower option", this);
            return;
        }

        towerOption = tower;

        if (iconImage != null && tower.icon != null)
        {
            iconImage.sprite = tower.icon;
        }
        else
        {
            Debug.LogError($"Icon or tower.icon is null on {gameObject.name}", this);
        }

        if (costText != null)
        {
            costText.text = tower.cost.ToString();
        }

        if (nameText != null)
        {
            nameText.text = tower.towerName;
        }

        if (tower.towerPrefab == null)
        {
            Debug.LogError($"Tower prefab is null for {tower.towerName}", this);
        }

        Debug.Log($"TowerButton {gameObject.name} initialized with tower: {tower.towerName}");
    }

    public void OnClick()
    {
        if (towerOption == null || towerOption.towerPrefab == null)
        {
            Debug.LogError($"Invalid tower option in {gameObject.name}", this);
            return;
        }

        if (TowerManager.Instance == null)
        {
            Debug.LogError("TowerManager instance is null", this);
            return;
        }

        Debug.Log($"Clicked tower button for: {towerOption.towerName}");
        TowerManager.Instance.SelectTower(towerOption);
    }
}