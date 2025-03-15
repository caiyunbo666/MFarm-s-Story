using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Save;

namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>, ISaveable
    {
        [Header("��Ʒ����")]
        public ItemDataList_SO ItemDataList_SO;
        [Header("������ͼ")]
        public BluePrintDataList_SO bluePrintData;
        [Header("��������")]
        public InventoryBag_SO playerBagTemp;
        public InventoryBag_SO playerBag;
        
        private InventoryBag_SO currentBoxBag;
        [Header("����")]
        public int playerMoney;

        private Dictionary<string, List<InventoryItem>> boxDataDict = new Dictionary<string, List<InventoryItem>>();

        public int BoxDataAmount => boxDataDict.Count;

        public string GUID => GetComponent<DataGUID>().guid;

        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
            EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
            EventHandler.BaseBagCloseEvent += OnBaseBagCloseEvent;
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
            EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
            EventHandler.BaseBagCloseEvent -= OnBaseBagCloseEvent;
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }
        private void OnBaseBagCloseEvent(SlotType slotType, InventoryBag_SO bag_SO)
        {
            currentBoxBag = bag_SO;
        }

        private void OnBuildFurnitureEvent(int ID, Vector3 mousePos)
        {
            RemoveItem(ID, 1);
            BluePrintDetails bluePrint = bluePrintData.GetBluePrintDetails(ID);
            foreach (var item in bluePrint.resourceItem)
            {
                RemoveItem(item.itemID, item.itemAmount);
            }
        }

        private void Start()
        {
            //EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }

        private void OnStartNewGameEvent(int obj)
        {
            playerBag = Instantiate(playerBagTemp);
            playerMoney = Settings.playerStartMoney;
            boxDataDict.Clear();
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)
        {
            RemoveItem(ID, 1);
        }

        private void OnHarvestAtPlayerPosition(int ID)
        {
            //�Ƿ��Ѿ��и�����
            var index = GetItemIndexInBag(ID);

            AddItemAtIndex(ID, index, 1);

            //����UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// ͨ��ID������Ʒ��Ϣ
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return ItemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// �����Ʒ��Player������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestroy"></param>
        public void AddItem(Item item, bool toDestroy)
        {
            //�Ƿ����и�����
            var index = GetItemIndexInBag(item.itemID);
            //�����Ƿ��п�λ
            AddItemAtIndex(item.itemID, index, 1);

            Debug.Log(GetItemDetails(item.itemID).itemID);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }

            //����UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
        /// <summary>
        /// ��鱳���Ƿ��п�λ
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// ͨ����ƷID�ҵ�����������Ʒλ��
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <returns>-1��û�����������򷵻����</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// ��ָ���������λ���������Ʒ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <param name="index">���</param>
        /// <param name="amount">����</param>
        private void AddItemAtIndex(int ID, int index, int amount)
        {
            if(index == -1 && CheckBagCapacity()) //�������޸���Ʒ �ұ����п�λ
            {
                var item = new InventoryItem { itemID =  ID, itemAmount = amount };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else //�������и���Ʒ
            {
                int currentAmount = playerBag.itemList[index].itemAmount + amount;
                var item = new InventoryItem { itemID= ID, itemAmount = currentAmount};

                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// Player ������Χ�ڽ�����Ʒ
        /// </summary>
        /// <param name="fromIndex">��ʼ���</param>
        /// <param name="targetIndex">Ŀ���������</param>
        public void SwapItem(int fromIndex, int targetIndex)
        {
            InventoryItem currentItem = playerBag.itemList[fromIndex];
            InventoryItem targetItem = playerBag.itemList[targetIndex];

            if(targetItem.itemID != 0)
            {
                playerBag.itemList[fromIndex] = targetItem;
                playerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                playerBag.itemList[targetIndex] = currentItem;
                playerBag.itemList[fromIndex] = new InventoryItem();
                
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// �米����������
        /// </summary>
        /// <param name="locationFrom"></param>
        /// <param name="fromIndex"></param>
        /// <param name="locationTarget"></param>
        /// <param name="targetIndex"></param>
        public void SwapItem(InventoryLocation locationFrom, int fromIndex, InventoryLocation locationTarget, int targetIndex)
        {
            var currentList = GetItemList(locationFrom);
            var targetList = GetItemList(locationTarget);

            InventoryItem currentItem = currentList[fromIndex];

            if (targetIndex < targetList.Count)
            {
                InventoryItem targetItem = targetList[targetIndex];

                if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID)//�в���ͬ��������Ʒ
                {
                    currentList[fromIndex] = targetItem;
                    targetList[targetIndex] = currentItem;
                }
                else if (currentItem.itemID == targetItem.itemID)//����ͬ��������Ʒ
                {
                    targetItem.itemAmount += currentItem.itemAmount;
                    targetList[targetIndex] = targetItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                else//Ŀ��ո���
                {
                    targetList[targetIndex] = currentItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                EventHandler.CallUpdateInventoryUI(locationFrom, currentList);
                EventHandler.CallUpdateInventoryUI(locationTarget, targetList);
            }
        }

        /// <summary>
        /// ����λ�÷��ر��������б�
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private List<InventoryItem> GetItemList(InventoryLocation location)
        {
            return location switch
            {
                InventoryLocation.Player => playerBag.itemList,
                InventoryLocation.Box => currentBoxBag.itemList,
                _ => null,
            };
        }

        /// <summary>
        /// �Ƴ�ָ�������ı�����Ʒ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <param name="removeAmount">����</param>
        private void RemoveItem(int ID, int removeAmount)
        {
            var index = GetItemIndexInBag(ID);

            if (playerBag.itemList[index].itemAmount > removeAmount)
            {
                var amount = playerBag.itemList[index].itemAmount - removeAmount;
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                playerBag.itemList[index] = item;
            }
            else if(playerBag.itemList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                playerBag.itemList[index] = item;
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        public void TradeItem(ItemDetails itemDetails, int amount, bool isSellTrade)
        {
            int cost = itemDetails.itemPrice * amount;
            //�����Ʒ����λ��
            int index = GetItemIndexInBag(itemDetails.itemID);

            if(isSellTrade)  //��
            {
                if (playerBag.itemList[index].itemAmount >= amount)
                {
                    RemoveItem(itemDetails.itemID, amount);
                    //�����ܼ�
                    cost = (int)(cost * itemDetails.sellPercentage);
                    playerMoney += cost;
                }
            }
            else if(playerMoney - cost >= 0)   //��
            {
                if (CheckBagCapacity())
                {
                    AddItemAtIndex(itemDetails.itemID, index, amount);
                }
                playerMoney -= cost;
            }
            //ˢ��UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// ��齨����Դ��Ʒ���
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckStock(int ID)
        {
            var bluePrintDetails = bluePrintData.GetBluePrintDetails(ID);

            foreach (var resourceItem in bluePrintDetails.resourceItem)
            {
                var itemStock = playerBag.GetInventoryItem(resourceItem.itemID);
                if (itemStock.itemAmount >= resourceItem.itemAmount)
                {
                    continue;
                }
                else return false;
            }
            return true;
        }

        public List<InventoryItem> GetBoxDataList(string key)
        {
            if (boxDataDict.ContainsKey(key))
                return boxDataDict[key];
            return null;
        }

        /// <summary>
        /// �������������ֵ�
        /// </summary>
        /// <param name="box"></param>
        public void AddBoxDataDict(Box box)
        {
            var key = box.name + box.index;
            if (!boxDataDict.ContainsKey(key))
                boxDataDict.Add(key, box.boxBagData.itemList);
        }

        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.playerMoney = this.playerMoney;

            saveData.inventoryDict = new Dictionary<string, List<InventoryItem>>();
            saveData.inventoryDict.Add(playerBag.name, playerBag.itemList);

            foreach (var item in boxDataDict)
            {
                saveData.inventoryDict.Add(item.Key, item.Value);
            }
            return saveData;
        }

        public void RestoreData(GameSaveData saveData)
        {
            this.playerMoney = saveData.playerMoney;
            playerBag = Instantiate(playerBagTemp);
            //string key = playerBag.name.Replace("(Clone)", "");
            playerBag.itemList = saveData.inventoryDict[playerBag.name];

            foreach (var item in saveData.inventoryDict)
            {
                if (boxDataDict.ContainsKey(item.Key))
                {
                    boxDataDict[item.Key] = item.Value;
                }
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}

