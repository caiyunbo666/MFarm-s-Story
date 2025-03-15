using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Settings
{
    public const float itemfadeDuration = 0.35f;

    public const float targetAlpha = 0.45f;

    //时间相关
    public const float secondThreshold = 0.1f;

    public const int secondHold = 59;

    public const int minuteHold = 59;

    public const int hourHold = 23;

    public const int dayHold = 30;

    public const int seasonHold = 3;

    //Transition
    public const float fadeDuration = 1.5f;

    public const int reapAmount = 2;

    //NPC 网格移动
    public const float gridCellSize = 1;

    public const float gridCellDiagonalSize = 1.41f;

    public const float pixelSize = 0.05f;   //20*20 占 1 unit

    public const float animationBreakTime = 5f;  //动画间隔时间

    public const int maxGridSize = 9999;

    public static Vector3 playerStarPos = new Vector3(-4.9f, 2.0f, 0);

    public const int playerStartMoney = 100;
}
