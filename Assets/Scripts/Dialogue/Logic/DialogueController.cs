using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MFarm.Dialogue
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogueController : MonoBehaviour
    {
        private NPCMovement npc => GetComponent<NPCMovement>();

        public UnityEvent OnFinishEvent;

        public List<DialoguePiece> dialogueList = new List<DialoguePiece>();

        private Stack<DialoguePiece> dialogueStack;

        private bool canTalk;

        private bool isTalking;

        private GameObject uiSign;

        private void Awake()
        {
            uiSign = transform.GetChild(1).gameObject;
            FillDialogueStack();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                canTalk = !npc.isMoving && npc.interactable;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            canTalk = false;
        }

        private void Update()
        {
            uiSign.SetActive(canTalk);

            if (canTalk && Input.GetKeyDown(KeyCode.Space) && !isTalking)
            {
                StartCoroutine(DialogueRoutine());
            }
        }


        /// <summary>
        /// 构建对话堆栈
        /// </summary>
        private void FillDialogueStack()
        {
            dialogueStack = new Stack<DialoguePiece>();
            for (int i = dialogueList.Count - 1; i > -1; i--)
            {
                dialogueList[i].isDone = false;
                dialogueStack.Push(dialogueList[i]);
            }
        }

        private IEnumerator DialogueRoutine()
        {
            isTalking = true;
            if (dialogueStack.TryPop(out DialoguePiece result))
            {
                //传到UI显示对话
                EventHandler.CallShowDialogueEvent(result);
                EventHandler.CallUpdateGameStateEvent(GameState.Pause);
                yield return new WaitUntil(() => result.isDone);
                isTalking = false;
            }
            else
            {
                EventHandler.CallUpdateGameStateEvent(GameState.Gameplay);
                EventHandler.CallShowDialogueEvent(null);
                FillDialogueStack();
                isTalking = false;

                if (OnFinishEvent != null)
                {
                    OnFinishEvent.Invoke();
                    canTalk = false;
                }
            }
        }
    }
}

