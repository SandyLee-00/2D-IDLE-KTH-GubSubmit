using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskAction : ScriptableObject
{
    public abstract int Run(Subtask task, int currentSuccess, int successCount);
}
