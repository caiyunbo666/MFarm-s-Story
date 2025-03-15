using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Save;

public class Player : MonoBehaviour, ISaveable
{
    private Rigidbody2D rb;

    private float inputX;

    private float inputY;
    
    public float speed;
    
    private Vector2 movementInput;

    private Animator[] animators;

    private bool isMoving;

    private bool inputDisable;

    //动画使用工具
    private float mouseX;

    private float mouseY;

    private bool useTool;

    public string GUID => GetComponent<DataGUID>().guid;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();    
    }

    private void Start()
    {
        ISaveable saveable = this;
        saveable.RegisterSaveable();
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
        EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        EventHandler.EndGameEvent += OnEndGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickedEvent -= OnMouseClickedEvent;
        EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        EventHandler.EndGameEvent -= OnEndGameEvent;
    }

    private void OnEndGameEvent()
    {
        inputDisable = true;
    }

    void Update()
    {
        if (inputDisable == false)
            PlayerInput();
        else
            isMoving = false;
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        if (!inputDisable)
            Movement();
    }

    private void OnStartNewGameEvent(int obj)
    {
        inputDisable = false;
        transform.position = Settings.playerStarPos;
    }
    private void OnUpdateGameStateEvent(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Gameplay:
                inputDisable = false;
                break;
            case GameState.Pause:
                inputDisable = true; 
                break;
        }
    }

    private void OnMouseClickedEvent(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        //TODO:执行动画
        if(itemDetails.itemType != ItemType.Seed && itemDetails.itemType != ItemType.Commodity && itemDetails.itemType != ItemType.Furniture)
        {
            mouseX = mouseWorldPos.x - transform.position.x;
            mouseY = mouseWorldPos.y - (transform.position.y + 0.85f);

            if(Mathf.Abs(mouseX) > Mathf.Abs(mouseY)) 
                mouseY = 0;
            else
                mouseX = 0;

            StartCoroutine(UseToolRoutine(mouseWorldPos,itemDetails));
        }
        else
        {
            EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        }
    }

    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        useTool = true;
        inputDisable = true;
        yield return null;
        foreach(var anim in animators)
        {
            anim.SetTrigger("useTool");
            //人物的转动方向
            anim.SetFloat("InputX", mouseX);
            anim.SetFloat("InputY", mouseY);
        }
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        yield return new WaitForSeconds(0.25f);
        //等待动画结束
        useTool = false;
        inputDisable = false;
    }

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if (inputX != 0 && inputY != 0)
        {
            inputX = inputX * 0.6f;
            inputY = inputY * 0.6f;
        }

        //走路状态速度
        if(Input.GetKey(KeyCode.LeftShift))
        {
            inputX = inputX * 0.5f;
            inputY = inputY * 0.5f;
        }
        movementInput = new Vector2(inputX, inputY);

        isMoving = movementInput != Vector2.zero;
    }

    public void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);
    }

    private void SwitchAnimation()
    {
        foreach(var anim in animators)
        {
            anim.SetBool("isMoving", isMoving);
            anim.SetFloat("mouseX", mouseX);
            anim.SetFloat("mouseY", mouseY);
            if( isMoving )
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.characterPosDict = new Dictionary<string, SerializableVector3>();
        saveData.characterPosDict.Add(this.name, new SerializableVector3(transform.position));

        return saveData;
    }

    public void RestoreData(GameSaveData saveData)
    {
        var targetPosition = saveData.characterPosDict[this.name].ToVector3();

        transform.position = targetPosition;
    }
}
