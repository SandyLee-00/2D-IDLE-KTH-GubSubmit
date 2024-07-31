using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Subtask/Action/SimpleCount", fileName = "SimpleCount")]
public class SimpleCount : TaskAction
{
    public override int Run(Subtask task, int currentSuccess, int successCount)
    {
        return currentSuccess + successCount;
    }
}
