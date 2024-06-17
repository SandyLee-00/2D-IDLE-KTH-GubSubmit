using UnityEngine;
using System.Linq;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}


[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    #region Events
    public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuceess);
    #endregion

    [Header("Category")]
    [SerializeField] private Category category;

    [Header("Task")]
    [SerializeField] private string codeName;
    [SerializeField] private string description;

    [Header("Setting")]
    [SerializeField] private int needSuccessToCompleted;
    [SerializeField] InitialSuccessValue initialSuccessValue;
    [SerializeField] private bool canReceiverReportsDuringCompletion; 
    // Task�� �Ϸ�Ǿ ��� ����Ƚ��(ī����)�� �� �������� ���ϴ� �ɼ�, Ex 100���� ��ƾ� Ŭ���� �� �� �ִµ�, ������ 50���� ����
    // ���� �� �ɼ��� ���ٸ� 100���� ���� ���Ŀ��� ī������ ���߱� ������ ���� 50���� ���Ⱦ 100���� �����Ǿ� ����Ʈ�� Ŭ���� �� �� ����
    // �̸� �����ϱ� ���� ��

    // �� �׽�ũ�� Ÿ���� �������� �� �����Ƿ�, �迭�� ����
    [Header("Target")]
    [SerializeField] private TaskTarget[] targets;

    [Header("Action")]
    [SerializeField] private TaskAction action;

    private TaskState state;
    private int currectSuccess;

    public event StateChangedHandler onStateChanged;
    public event SuccessChangedHandler onSuccessChanged;

    public int CurrentSuccess
    {
        get => currectSuccess;

        set
        {
            int prevSuccess = currectSuccess;
            currectSuccess = Mathf.Clamp(value, 0, needSuccessToCompleted);
            if (currectSuccess != prevSuccess)
            {
                state = currectSuccess == needSuccessToCompleted ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, currectSuccess, prevSuccess);
            }
        }
    }

    public Category Category => category;
    public string CodeName => codeName;
    public string Description => description;
    public int NeedSuccessToCompleted => needSuccessToCompleted;

    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            onStateChanged?.Invoke(this, state, prevState);
        }
    }

    public bool IsComplete => State == TaskState.Complete;

    public Quest Owner { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }

    public void Start()
    {
        State = TaskState.Running;
        if (initialSuccessValue)
        {
            CurrentSuccess = initialSuccessValue.Getvalue(this);
        }
    }

    public void End()
    {
        onStateChanged = null;
        onSuccessChanged = null;
    }


    public void ReceiveReport(int successCount)
    {
        CurrentSuccess = action.Run(this, CurrentSuccess, successCount);
    }

    public void Complete()
    {
        CurrentSuccess = needSuccessToCompleted;
    }

    // TaskTarget�� ���� �� Task�� ���� Ƚ���� ������� ������� �ƴ����� �Ǵ�
    // Setting�س��� Target�� �߿� �ִٸ� True�� ���ٸ� Fasle�� ��ȯ
    public bool IsTarget(string category, object target)
        => Category == category &&
        targets.Any(x => x.IsEqual(target)) &&
        (!IsComplete || (IsComplete && canReceiverReportsDuringCompletion));
}