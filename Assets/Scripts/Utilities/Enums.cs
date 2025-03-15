public enum ItemType
{
    Seed,Commodity,Furniture,

    HoeTool,ChopTool,BreakTool,ReapTool,WaterTool,CollectTool,

    ReapableScenery,
    //种子，商品，家具
    //锄头，斧头，凿石头，割草，浇水，收割
    //可以被割的杂草
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
    春天, 夏天, 秋天, 冬天
}

public enum GridType
{
    //挖坑，扔东西，添置家具，阻挡NPC
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