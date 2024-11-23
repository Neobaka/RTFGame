using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    public GameObject[] towerPrefabs;
    public LayerMask placementArea;
    public Color validPlacementColor = Color.green;
    public Color invalidPlacementColor = Color.red;

    private GameObject currentTowerPreview;
    private int selectedTowerIndex = -1;
    private Camera mainCamera;
    private Renderer previewRenderer;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (selectedTowerIndex >= 0)
        {
            UpdateTowerPreview();

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceTower();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    public void SelectTower(int index)
    {
        selectedTowerIndex = index;
        if (currentTowerPreview != null)
            Destroy(currentTowerPreview);

        currentTowerPreview = Instantiate(towerPrefabs[index]);
        previewRenderer = currentTowerPreview.GetComponentInChildren<Renderer>();
    }

    private void UpdateTowerPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementArea))
        {
            currentTowerPreview.transform.position = hit.point;
            bool canPlace = !Physics.CheckSphere(hit.point, 1f, LayerMask.GetMask("Tower"));
            previewRenderer.material.color = canPlace ? validPlacementColor : invalidPlacementColor;
        }
    }

    private void PlaceTower()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementArea))
        {
            if (!Physics.CheckSphere(hit.point, 1f, LayerMask.GetMask("Tower")))
            {
                int towerCost = towerPrefabs[selectedTowerIndex].GetComponent<Tower>().cost;
                if (GameManager.Instance.SpendGold(towerCost))
                {
                    Instantiate(towerPrefabs[selectedTowerIndex], hit.point, Quaternion.identity);
                }
            }
        }

        CancelPlacement();
    }

    private void CancelPlacement()
    {
        if (currentTowerPreview != null)
            Destroy(currentTowerPreview);
        selectedTowerIndex = -1;
    }
}