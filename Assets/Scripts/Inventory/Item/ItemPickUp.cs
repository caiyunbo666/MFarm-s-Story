using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();

            if(item != null)
            {
                if(item.itemDetails.canPickedup)
                {
                    InventoryManager.Instance.AddItem(item, true);
                }
            }
            //else
            //{
            //    Debug.Log("Error,null item");
            //}
        }
    }
}

