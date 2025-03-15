using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    public class Slot_UI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("�����ȡ")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] public Image slotHighlight;
        [SerializeField] private Button button;

        [Header("��������")]
        public SlotType slotType;

        public bool isSelected;

        public int slotIndex;

        //��Ʒ��Ϣ
        public ItemDetails itemDetails;
        public int itemAmount;

        public InventoryLocation Location
        {
            get
            {
                return slotType switch
                {
                    SlotType.Bag => InventoryLocation.Player,
                    SlotType.Box => InventoryLocation.Box,
                    _ => InventoryLocation.Player,
                };
            }
        }



        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;

            if (itemDetails == null)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// ���¸���UI����Ϣ
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">��������</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// ��Slot����Ϊ��
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;

                inventoryUI.UpdateSlotHighlight(-1);

                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;
            isSelected = !isSelected;

            inventoryUI.UpdateSlotHighlight(slotIndex);

            if (slotType == SlotType.Bag)
            {
                //֪ͨ��Ʒ��ѡ�е�״̬����Ϣ
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHighlight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            //Debug.Log(eventData.pointerCurrentRaycast);

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot_UI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot_UI>();
                int targeIndex = targetSlot.slotIndex;

                //��Player��������Χ�ڽ���
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targeIndex);
                }
                else if (slotType == SlotType.Shop && targetSlot.slotType == SlotType.Bag)    //��
                {
                    EventHandler.CallShowTradeUI(itemDetails, false);
                }
                else if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Shop)   //��
                {
                    EventHandler.CallShowTradeUI(itemDetails, true);
                }
                else if (slotType != SlotType.Shop && targetSlot.slotType != SlotType.Shop && slotType != targetSlot.slotType)
                {
                    //�米�����ݽ�����Ʒ
                    InventoryManager.Instance.SwapItem(Location, slotIndex, targetSlot.Location, targetSlot.slotIndex);
                }

                //������и�����ʾ
                inventoryUI.UpdateSlotHighlight(-1);
            }
            //else    //�������ڵ���
            //{
            //    if(itemDetails.canDropped)
            //    {
            //        //����Ӧ�����ͼ����
            //        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            //        EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
            //    }
            //}    
        }


    }
}

