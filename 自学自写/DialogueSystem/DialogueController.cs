/* 代码功能：对话控制器
 作为实际功能组件的基类，用于控制对话的显示和跳过
 */




using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] protected DialogueControl dialogueControl;

    protected virtual void Awake()
    {
        if (dialogueControl is null)
        {
            dialogueControl = GetComponent<DialogueControl>();
            Debug.Log("没有找到所属的 DialogueControl,尝试获取组件");
        }
    }

    public virtual void StartDialogue()
    {
        if (dialogueControl != null)
        {
            dialogueControl.ShowDialogue();
        }
    }

    public virtual void SkipDialogue()
    {
        if (dialogueControl != null)
        {
            dialogueControl.SkipDialogue();
        }
    }
}