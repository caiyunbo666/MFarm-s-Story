using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Save
{
    public class GameSaveData
    {
        public string dataSceneName;
        /// <summary>
        /// �洢�������꣬string��������
        /// </summary>
        public Dictionary<string, SerializableVector3> characterPosDict;

        public Dictionary<string, List<SceneItem>> sceneItemDict;

        public Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;

        //��������+����Ͷ�Ӧ����Ƭ��Ϣ
        public Dictionary<string, TileDetails> tileDetailsDict;

        //�����Ƿ��һ�μ���
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

