using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questTitleText;
    [SerializeField]
    private TaskDescriptor taskDescriptorPrefab;

    private Dictionary<Subtask, TaskDescriptor> taskDesriptorsByTask = new Dictionary<Subtask, TaskDescriptor>();

    private Quest targetQuest;

    private void OnDestroy()
    {
        if (targetQuest != null)
        {
            targetQuest.OnNewTaskGroup -= UpdateTaskDescriptos;
            targetQuest.OnCompleted -= DestroySelf;
        }

        foreach (var tuple in taskDesriptorsByTask)
        {
            var task = tuple.Key;
            task.OnSubtaskSuccessChanged -= UpdateText;
        }
    }

    public void Setup(Quest targetQuest, Color titleColor)
    {
        this.targetQuest = targetQuest;

        questTitleText.text = targetQuest.Category == null ?
            targetQuest.DisplayName :
            $"[{targetQuest.Category.DisplayName}] {targetQuest.DisplayName}";

        questTitleText.color = titleColor;

        targetQuest.OnNewTaskGroup += UpdateTaskDescriptos;
        targetQuest.OnCompleted += DestroySelf;

        var taskGroups = targetQuest.TaskGroups;
        UpdateTaskDescriptos(targetQuest, taskGroups[0]);

        if (taskGroups[0] != targetQuest.CurrentTaskGroup)
        {
            for (int i = 1; i < taskGroups.Count; i++)
            {
                var taskGroup = taskGroups[i];
                UpdateTaskDescriptos(targetQuest, taskGroup, taskGroups[i - 1]);

                if (taskGroup == targetQuest.CurrentTaskGroup)
                    break;
            }
        }
    }

    private void UpdateTaskDescriptos(Quest quest, SubtaskGroup currentTaskGroup, SubtaskGroup prevTaskGroup = null)
    {
        foreach (var task in currentTaskGroup.Tasks)
        {
            var taskDesriptor = Instantiate(taskDescriptorPrefab, transform);
            taskDesriptor.UpdateText(task);
            task.OnSubtaskSuccessChanged += UpdateText;

            taskDesriptorsByTask.Add(task, taskDesriptor);
        }

        if (prevTaskGroup != null)
        {
            foreach (var task in prevTaskGroup.Tasks)
            {
                var taskDesriptor = taskDesriptorsByTask[task];
                taskDesriptor.UpdateTextUsingStrikeThrough(task);
            }
        }
    }

    private void UpdateText(Subtask task, int currentSucess, int prevSuccess)
    {
        taskDesriptorsByTask[task].UpdateText(task);
    }

    private void DestroySelf(Quest quest)
    {
        Destroy(gameObject);
    }
}
