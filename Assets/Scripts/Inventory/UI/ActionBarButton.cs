using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MFarm.Inventory
{
    [RequireComponent(typeof(Slot_UI))]
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;

        private Slot_UI slotUI;

        private bool canUse;

        private void Awake()
        {
            slotUI = GetComponent<Slot_UI>();
        }

        private void OnEnable()
        {
            EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
        }

        private void OnDisable()
        {
            EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;
        }

        private void OnUpdateGameStateEvent(GameState gameState)
        {
            canUse = gameState == GameState.Gameplay;
        }

        void Update()
        {
            if(Input.GetKeyDown(key) && canUse)
            {
                if(slotUI.itemDetails != null)
                {
                    slotUI.isSelected = !slotUI.isSelected;
                    if (slotUI.isSelected)
                        slotUI.inventoryUI.UpdateSlotHighlight(slotUI.slotIndex);
                    else
                        slotUI.inventoryUI.UpdateSlotHighlight(-1);

                    EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
                }
            }
        }
    }
}

