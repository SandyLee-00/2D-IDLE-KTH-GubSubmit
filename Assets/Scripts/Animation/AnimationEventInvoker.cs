using System.Numerics;
using UnityEngine;

public class AnimationEventInvoker : MonoBehaviour
{
    private EnemyCharacter enemyCharacter;

    private void Start()
    {
        enemyCharacter = GetComponentInParent<EnemyCharacter>();
    }

    public void InvokeAttackEnemy()
    {
        // ���� �ʵ忡 �����ϴ� ���� ã��
        var enemy = FindObjectOfType<EnemyCharacter>();
        if (enemy != null)
        {
            // �θ� PlayerCharacter�� ���ݷ��� �����ͼ� ������ �������� ��
            BigInteger attackPower = PlayerCharacter.Instance.Character.AttackPower;
            enemy.TakeDamage(attackPower);
            AudioManager.Instance.PlayerAttackSound();
        }
    }

    public void InvokeStopAttack()
    {
        var stateMachine = PlayerCharacter.Instance.GetStateMachine();
        if (stateMachine.currentState is PlayerIdleState idleState)
        {
            idleState.SetAttackFalse();
        }
    }

    public void InvokeEnemyAttackisPlayer()
    {
        var player = FindObjectOfType<PlayerCharacter>();
        if ( player != null && enemyCharacter != null)
        {
            BigInteger enemyAttackPower = enemyCharacter.Character.AttackPower;
            player.TakeDamage(enemyAttackPower);
        }
    }
}
