using UnityEngine;
using System;
using UnityEngine.Events;
using System.Numerics;
public class EnemyCharacter : MonoBehaviour
{
    private Animator animator;
    public Character Character { get; private set; }

    [Header("������ǥ��")]
    [SerializeField] private GameObject damageTextPrefab;

    private StageManager stageManager; // �������� �Ŵ���
    private DropManager dropManager; // ��� �Ŵ���

    public static event Action OnEnemyDestroyed;
    public event Action OnDeath; // �� ��� �̺�Ʈ
    public UnityEvent onDead;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Character = GetComponent<Character>();
        stageManager = StageManager.Instance;
        dropManager = DropManager.Instance;
    }

    public void TakeDamage(BigInteger damage)
    {
        Character.CurrentHealth -= (float)damage;
        ShowDamageText(damage);
        if (Character.CurrentHealth <= 0)
        {
            OnDeath?.Invoke(); // ��� �̺�Ʈ Ʈ����
            OnEnemyDestroyed?.Invoke(); // �� �ı� �̺�Ʈ Ʈ����
            stageManager.IncrementStageProgress(); // �������� ���൵ ����
            dropManager.DropRewards(PlayerCharacter.Instance); // ���� ���� �� ���� ���
            onDead?.Invoke();
        }
    }
    private void ShowDamageText(BigInteger damage)
    {
        if (damageTextPrefab != null)
        {
            // ���� ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ
            UnityEngine.Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + UnityEngine.Vector3.up * 1.5f);
            // Canvas�� ã��, �� �ȿ� ������ �ؽ�Ʈ�� �����Ͽ� ǥ��
            Canvas canvas = FindObjectOfType<Canvas>();
            GameObject damageTextObject = Instantiate(damageTextPrefab, canvas.transform);
            damageTextObject.transform.position = screenPosition;

            DamageText damageText = damageTextObject.GetComponent<DamageText>();
            damageText.Initialize(damage);
        }
    }
}
