using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Save
{
    public class GameSaveData
    {
        public string dataSceneName;
        /// <summary>
        /// 存储人物坐标，string人物名字
        /// </summary>
        public Dictionary<string, SerializableVector3> characterPosDict;

        public Dictionary<string, List<SceneItem>> sceneItemDict;

        public Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;

        //场景名字+坐标和对应的瓦片信息
        public Dictionary<string, TileDetails> tileDetailsDict;

        //场景是否第一次加载
        public Dictionary<string, bool> firstLoadDict;

        public Dictionary<string, List<InventoryItem>> inventoryDict;

        public Dictionary<string, int> timeDict;

        public int playerMoney;

        //NPC
        public string targetScene;

        public bool interactable;

        public int animationInstanceID;

    }
}

