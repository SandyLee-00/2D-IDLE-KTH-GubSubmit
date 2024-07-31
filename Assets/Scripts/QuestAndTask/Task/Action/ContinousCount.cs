using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Subtask/Action/ContineousCount", fileName = "ContinousCount")]
public class ContinousCount : TaskAction
{
    public override int Run(Subtask task, int currentSuccess, int successCount)
    {
        return successCount > 0 ? currentSuccess + successCount : 0;
    }

}
