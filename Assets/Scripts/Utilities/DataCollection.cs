using System.Globalization;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ItemDetails
{
    public int itemID;

    public string ItemName;

    public ItemType itemType;

    public Sprite itemIcon;

    public Sprite itemOnWorldSprite;

    public string itemDescription;

    public int itemUseRadius;

    public bool canPickedup;

    public bool canDropped;

    public bool canCarried;

    public int itemPrice;
    
    [Range(0, 1)]
    public float sellPercentage;
}

//作用：序列化
[System.Serializable]
//这里用结构体不是类的原因;
//省去删除操作
//自动初始化不会报空
public struct InventoryItem
{
    public int itemID;

    public int itemAmount;
}

[System.Serializable]
public class AnimatorType
{
    public PartType partType;

    public PartName partName;

    public AnimatorOverrideController overrideController;
}

[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;
    
    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

[System.Serializable]
public class SceneItem
{
    public int itemID;

    public SerializableVector3 position;
}

[System.Serializable]
public class SceneFurniture
{
    public int itemID;

    public SerializableVector3 position;

    public int boxIndex;
}

[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;

    public GridType gridType;

    public bool boolTypeValue;
}

[System.Serializable]
public class TileDetails
{
    public int gridX, gridY;

    public bool canDig;

    public bool canDropItem;

    public bool canPlaceFurniture;

    public bool isNPCObstacle;

    public int daysSinceDug = -1;

    public int daysSinceWatered = -1;

    public int seedItemID = -1;

    public int growthDays = -1;

    public int daysSinceLastHarvest = -1;

}

[System.Serializable]

public class NPCPosition
{
    public Transform npc;

    public string startScene;

    public Vector3 position;
}
//场景路径
[System.Serializable]
public class SceneRoute
{
    public string fromSceneName;

    public string gotoSceneName;

    public List<ScenePath> scenePathList;
}

[System.Serializable]
public class ScenePath
{
    public string sceneName;

    public Vector2Int fromGridCell;

    public Vector2Int gotoGridCell;
}

