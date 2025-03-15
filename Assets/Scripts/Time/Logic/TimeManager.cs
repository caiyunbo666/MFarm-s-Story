using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Save;

public class TimeManager : Singleton<TimeManager>, ISaveable
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Season gameSeason = Season.春天;

    private int monthInSeason = 3;

    public bool gameClockPause;

    public float tikTime;

    public TimeSpan GameTime => new TimeSpan(gameHour, gameMinute, gameSecond);

    public string GUID => GetComponent<DataGUID>().guid;

    protected override void Awake()
    {
        base.Awake();
        NewGameTime();
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        EventHandler.EndGameEvent += OnEndGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        EventHandler.EndGameEvent -= OnEndGameEvent;
    }

    private void OnEndGameEvent()
    {
        gameClockPause = true;
    }

    private void OnUpdateGameStateEvent(GameState gameState)
    {
        gameClockPause = gameState == GameState.Pause;
    }

    private void OnStartNewGameEvent(int obj)
    {
        NewGameTime();
        gameClockPause = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        gameClockPause = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        gameClockPause = true;
    }
    private void Start()
    {
        //EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        //EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gameYear, gameSeason);
        ISaveable saveable = this;
        saveable.RegisterSaveable();
        gameClockPause = true;
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;

            if(tikTime > Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }

        ///作弊
        if(Input.GetKey(KeyCode.T))
        {
            for(int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gameDay++;
            EventHandler.CallGameDayEvent(gameDay, gameSeason);
            EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2024;
        gameSeason = Season.春天;
    }

    private void UpdateGameTime()
    {
        gameSecond++;
        if(gameSecond == Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if(gameMinute == Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if(gameHour == Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if(gameDay == Settings.dayHold)
                    {
                        gameMonth++;
                        gameDay = 1;

                        if(gameMonth > 12)
                        {
                            gameMonth = 1;
                        }

                        monthInSeason--;
                        if(monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if(seasonNumber == Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++; 
                            }

                            gameSeason = (Season)seasonNumber;

                            if(gameYear > 9999)
                            {
                                gameYear = 2024;
                            }
                        }
                        //用来刷新地图和农作物生长
                        EventHandler.CallGameDayEvent(gameDay, gameSeason);
                    }               
                }
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gameYear, gameSeason);
        }

        //Debug.Log("Second:" + gameSecond + "Minute:" + gameMinute);
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.timeDict = new Dictionary<string, int>();
        saveData.timeDict.Add("gameYear", gameYear);
        saveData.timeDict.Add("gameMonth", gameMonth);
        saveData.timeDict.Add("gameDay", gameDay);
        saveData.timeDict.Add("gameHour", gameHour);
        saveData.timeDict.Add("gameMinute", gameMinute);
        saveData.timeDict.Add("gameSecond", gameSecond);
        saveData.timeDict.Add("gameSeason", (int)gameSeason);

        return saveData;
    }

    public void RestoreData(GameSaveData saveData)
    {
        gameYear = saveData.timeDict["gameYear"];
        gameMonth = saveData.timeDict["gameMonth"];
        gameDay = saveData.timeDict["gameDay"];
        gameHour = saveData.timeDict["gameHour"];
        gameMinute = saveData.timeDict["gameMinute"];
        gameSecond = saveData.timeDict["gameSecond"];
        gameSeason = (Season)saveData.timeDict["gameSeason"];
    }
}
