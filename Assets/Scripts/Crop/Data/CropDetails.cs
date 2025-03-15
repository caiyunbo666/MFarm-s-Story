using UnityEngine;

[System.Serializable]
public class CropDetails
{
    public int seedItemID;
    [Header("��ͬ�׶���Ҫ������")]

    public int[] growthDays;
    public int TotalGrowthDays
    {
        get
        {
            int amount = 0;
            foreach (var days in growthDays)
            {
                amount += days;
            }
            return amount;
        }
    }

    [Header("��ͬ�׶������׶���ƷPrefabs")]
    public GameObject[] growthPrefabs;

    [Header("��ͬ�׶ε�ͼƬ")]
    public Sprite[] growthSprites;

    [Header("����ֲ�ļ���")]
    public Season[] seasons;

    [Space]
    [Header("�ո��")]
    public int[] harvestToolItemID;

    [Header("ÿ�ֹ���ʹ�ô���")]
    public int[] requireActionCount;

    [Header("ת������ƷID")]
    public int transferItemID;

    [Space]
    [Header("�ո��ʵ��Ϣ")]
    public int[] producedItemID;

    public int[] producedMinAmount;

    public int[] producedMaxAmount;

    public Vector2 spawnRadius;

    [Header("�ٴ�����ʱ��")]
    public int daysToRegrow;

    public int regrowTimes;

    [Header("Options")]
    public bool generateAtPlayerPosition;

    public bool hasAnimation;

    public bool hasParticalEffect;
    //TODO����Ч ��Ч ��

    public ParticalEffectType effectType;

    public Vector3 effectPos;

    /// <summary>
    /// ��鵱ǰ�����Ƿ����
    /// </summary>
    /// <param name="toolID">����ID</param>
    /// <returns></returns>
    public bool CheckToolAvailable(int toolID)
    {
        foreach(var tool in harvestToolItemID)
        {
            if(tool == toolID)
                return true; 
        }
        return false;
    }

    public int GetTotalRequireCount(int toolID)
    {
        for(int i = 0; i < harvestToolItemID.Length; i++)
        {
            if (harvestToolItemID[i] == toolID)
                return requireActionCount[i];
        }
        return -1;
    }
}
