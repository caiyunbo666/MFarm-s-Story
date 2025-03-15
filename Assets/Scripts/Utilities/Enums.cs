public enum ItemType
{
    Seed,Commodity,Furniture,

    HoeTool,ChopTool,BreakTool,ReapTool,WaterTool,CollectTool,

    ReapableScenery,
    //���ӣ���Ʒ���Ҿ�
    //��ͷ����ͷ����ʯͷ����ݣ���ˮ���ո�
    //���Ա�����Ӳ�
}

public enum SlotType
{
    Bag, Box, Shop
}

public enum InventoryLocation
{
    Player,Box
}

public enum PartType
{
    None, Carry, Hoe, Break,Water,Chop,Collect,Reap
}

public enum PartName
{
    Body, Hair, Arm, Tool,
}

public enum Season
{
    ����, ����, ����, ����
}

public enum GridType
{
    //�ڿӣ��Ӷ��������üҾߣ��赲NPC
    Diggable,DropItem,PlaceFurniture,NPCObstacle
}

public enum ParticalEffectType
{
    None, LeavesFalling01, LeavesFalling02, Rock, ReapableScenery
}

public enum GameState
{
    Gameplay, Pause
}