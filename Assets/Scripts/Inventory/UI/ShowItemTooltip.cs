using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    [RequireComponent(typeof(Slot_UI))]
    public class ShowItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Slot_UI slotUI;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Awake()
        {
            slotUI = GetComponent<Slot_UI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(slotUI.itemAmount != 0)
            {
                //Debug.Log("OnPointEnter函数已被调用");
                inventoryUI.itemTooltip.gameObject.SetActive(true);
                inventoryUI.itemTooltip.SetupTooltip(slotUI.itemDetails, slotUI.slotType);

                inventoryUI.itemTooltip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                inventoryUI.itemTooltip.transform.position = transform.position + Vector3.up * 60;

                if(slotUI.itemDetails.itemType == ItemType.Furniture)
                {
                    inventoryUI.itemTooltip.resourcePanel.SetActive(true);
                    inventoryUI.itemTooltip.SetupResourcePanel(slotUI.itemDetails.itemID);
                }
                else
                {
                    inventoryUI.itemTooltip.resourcePanel.SetActive(false);
                }
            }
            else
            {
                inventoryUI.itemTooltip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemTooltip.gameObject.SetActive(false);
        }


    }

}
