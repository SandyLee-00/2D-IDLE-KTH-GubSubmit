using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Subtask/Action/NegativeCount", fileName = "NegativeCount")]
public class NegativeCount : TaskAction
{
    public override int Run(Subtask task, int currentSuccess, int successCount)
    {
        return successCount < 0 ? currentSuccess - successCount : currentSuccess;
    }
}
