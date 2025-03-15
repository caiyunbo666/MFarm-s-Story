using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MFarm.Map;
using MFarm.CropPlant;
using MFarm.Inventory;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed, item;

    private Sprite currentSprite; //�洢��ǰ���ͼƬ
    private Image cursorImage;
    private RectTransform cursorCanvas;

    //����ͼ�����
    private Image buildImage;

    //�����
    private Camera mainCamera;
    private Grid currentGrid;

    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;

    private bool cursorEnable;

    private bool cursorPositionValid;

    private ItemDetails currentItem;

    private Transform playerTransform => FindObjectOfType<Player>().transform;


    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadEvent;
    }
    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadEvent;
    }

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        //�õ�����ͼ��
        buildImage = cursorCanvas.GetChild(1).GetComponent<Image>();
        buildImage.gameObject.SetActive(false);

        currentSprite = normal;
        SetCursorImage(normal);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;

        if (!InteractWithUI() && cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
            buildImage.gameObject.SetActive(false);
        }
    }

    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            //ִ�з���
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currentItem);
        }
    }


    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }

    private void OnAfterSceneLoadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }
    #region �������ͼƬ
    /// <summary>
    /// �������ͼƬ
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
        buildImage.color = new Color(1, 1, 1, 0.5f);
    }
    /// <summary>
    /// ������겻����
    /// </summary>
    private void SetCursorInValid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.4f);
        buildImage.color = new Color(1, 0, 0, 0.5f);
    }
    #endregion

    /// <summary>
    /// ��Ʒѡ���¼�����
    /// </summary>
    /// <param name="itemDetails"></param>
    /// <param name="isSelected"></param>
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {

        if (!isSelected)
        {
            currentItem = null;
            cursorEnable = false;
            currentSprite = normal;
            buildImage.gameObject.SetActive(false);
        }
        else
        {
            currentItem = itemDetails;

            //TODO:����������Ͷ�ӦͼƬ
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.ChopTool => tool,
                ItemType.HoeTool => tool,
                ItemType.WaterTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.Furniture => tool,
                ItemType.CollectTool => tool,
                _ => normal
            };
            cursorEnable = true;

            //��ʾ������ͼƬ
            if (itemDetails.itemType == ItemType.Furniture)
            {
                buildImage.gameObject.SetActive(true);
                buildImage.sprite = itemDetails.itemOnWorldSprite;
                buildImage.SetNativeSize();
            }
        }
    }

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        //Debug.Log("WorldPos:" + mouseWorldPos + "  GridPos:" + mouseGridPos);

        var playerGridPos = currentGrid.WorldToCell(playerTransform.position);

        //����ͼƬ�����ƶ�
        buildImage.rectTransform.position = Input.mousePosition;

        //�ж���ʹ�÷�Χ��
        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInValid();
            return;
        }

        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);

        if (currentTile != null)
        {
            CropDetails currentCrop = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
            Crop crop = GridMapManager.Instance.GetCropObject(mouseWorldPos);

            //TODO:����������Ʒ���͵��ж�
            switch (currentItem.itemType)
            {
                case ItemType.Seed:             //����
                    if (currentTile.daysSinceDug > -1 && currentTile.seedItemID == -1) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.Commodity:        //��Ʒ
                    if (currentTile.canDropItem && currentItem.canDropped) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.HoeTool:          //��ͷ
                    if (currentTile.canDig) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.WaterTool:        //��ˮ
                    if (currentTile.daysSinceDug > -1 && currentTile.daysSinceWatered == -1) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.BreakTool:        //ʮ�ָ�
                case ItemType.ChopTool:         //��ͷ
                    if (crop != null)
                    {
                        if (crop.CanHarvest && crop.cropDetails.CheckToolAvailable(currentItem.itemID)) SetCursorValid(); else SetCursorInValid();
                    }
                    else SetCursorInValid();
                    break;
                case ItemType.CollectTool:      //�ռ����ߣ������ӣ�
                    if (currentCrop != null)
                    {
                        if (currentCrop.CheckToolAvailable(currentItem.itemID))
                            if (currentTile.growthDays >= currentCrop.TotalGrowthDays) SetCursorValid(); else SetCursorInValid();
                    }
                    else
                        SetCursorInValid();
                    break;
                case ItemType.ReapTool:         //����
                    if (GridMapManager.Instance.HaveReapableItemsInRadius(mouseWorldPos, currentItem)) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.Furniture:
                    buildImage.gameObject.SetActive(true);
                    var bluePrintDetails = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(currentItem.itemID);

                    if (currentTile.canPlaceFurniture && InventoryManager.Instance.CheckStock(currentItem.itemID) && !HaveFurnitureInRaduis(bluePrintDetails))
                        SetCursorValid();
                    else SetCursorInValid();
                    break;
            }
        }
        else
        {
            SetCursorInValid();
        }
    }

    private bool HaveFurnitureInRaduis(BluePrintDetails bluePrintDetails)
    {
        var buildItem = bluePrintDetails.buildPrefab;
        Vector2 point = mouseWorldPos;
        var size = buildItem.GetComponent<BoxCollider2D>().size;

        var otherColl = Physics2D.OverlapBox(point, size, 0);
        if (otherColl != null)
            return otherColl.GetComponent<Furniture>();
        return false;
    }

    /// <summary>
    /// �Ƿ���UI����
    /// </summary>
    /// <returns></returns>
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
