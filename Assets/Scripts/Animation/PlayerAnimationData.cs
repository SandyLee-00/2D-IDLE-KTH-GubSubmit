using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[Serializable]
public class PlayerAnimationData
{
    [Header("����")]
    [SerializeField] private string noneParameterName = "@None";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string findParameterName = "Find";

    [Header("������ Ȱ��ȭ")]
    [SerializeField] private string autoAttackParameterName = "AutoAttack";

    // �ִϸ��̼� �Ķ���� �ؽ� �� ����
    public int NoneParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int FindParameterHash { get; private set; }
    public int AutoAttackParameterHash { get; private set; }
    public void Initialize()
    {
        // �Ķ���� �̸��� �ؽ� ������ ��ȯ
        NoneParameterHash = Animator.StringToHash(noneParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        FindParameterHash = Animator.StringToHash(findParameterName);
        AutoAttackParameterHash = Animator.StringToHash(autoAttackParameterName);
    }
}
