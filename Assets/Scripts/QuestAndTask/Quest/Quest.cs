using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForCompletion
}

[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangedHandler(Quest quest, Subtask task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceledHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, SubtaskGroup currentTaskGroup, SubtaskGroup prevTaskGroup);
    #endregion

    [SerializeField]
    private Category category;
    [SerializeField]
    private Sprite icon;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField, TextArea]
    private string description;

    [Header("Subtask")]
    [SerializeField]
    private SubtaskGroup[] taskGroups;

    [Header("Reward")]
    [SerializeField]
    private Reward[] rewards;

    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete;
    [SerializeField]
    private bool isCancelable;

    [Header("Condition")]
    [SerializeField]
    private Condition[] acceptionConditions;
    [SerializeField]
    private Condition[] cancelConditions;

    private int currentTaskGroupIndex;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public QuestState State { get; private set; }
    public SubtaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<SubtaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<Reward> Rewards => rewards;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsComplatable => State == QuestState.WaitingForCompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => isCancelable && cancelConditions.All(x => x.IsPass(this));
    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));

    public event TaskSuccessChangedHandler OnTaskSuccessChanged;
    public event CompletedHandler OnCompleted;
    public event CanceledHandler OnCanceled;
    public event NewTaskGroupHandler OnNewTaskGroup;

    public void OnRegister()
    {
        // 결코 일어나서는 안될 버그를 빨리 알아채게 만드는 코드, 방어적 프로그레밍
        Debug.Assert(!IsRegistered, "This quest has already been registered."); 

        foreach (var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
                task.OnSuccessChanged += OnSuccessChanged;
        }

        State = QuestState.Running;
        CurrentTaskGroup.Start();
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegistered, "This quest has already been registered.");
        Debug.Assert(!IsCancel, "This quest has been canceled.");

        if (IsComplete)
            return;

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if (currentTaskGroupIndex + 1 == taskGroups.Length)
            {
                State = QuestState.WaitingForCompletion;
                if (useAutoComplete)
                    Complete();
            }
            else
            {
                var prevTasKGroup = taskGroups[currentTaskGroupIndex++];
                prevTasKGroup.End();
                CurrentTaskGroup.Start();
                OnNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTasKGroup);
            }
        }
        else
            State = QuestState.Running;
    }

    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskGroup in taskGroups)
            taskGroup.Complete();

        State = QuestState.Complete;

        foreach (var reward in rewards)
            reward.Give(this);

        OnCompleted?.Invoke(this);

        OnTaskSuccessChanged = null;
        OnCompleted = null;
        OnCanceled = null;
        OnNewTaskGroup = null;
    }

    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(IsCancelable, "This quest can't be canceled");

        State = QuestState.Cancel;
        OnCanceled?.Invoke(this);
    }

    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new SubtaskGroup(x)).ToArray();

        return clone;
    }

    private void OnSuccessChanged(Subtask task, int currentSuccess, int prevSuccess)
        => OnTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);

    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "This quest has already been registered");
        Debug.Assert(!IsCancel, "This quest has been canceled.");
        Debug.Assert(!IsComplete, "This quest has already been completed");
    }
}
