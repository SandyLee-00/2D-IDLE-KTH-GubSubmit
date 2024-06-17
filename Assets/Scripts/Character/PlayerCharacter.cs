using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float maxHealth;
    public float currentHealth;
    public float attackPower;
    public int currentExperience;
    public int currentLevel;
    public int currentGold;
    public int experienceToNextLevel;
}

public class PlayerCharacter : SingletonMonoBehaviour<PlayerCharacter>
{
    public PlayerData data;
    [field: Header("Anime��")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public PlayerController Input { get; private set; }
    public Character Character { get; private set; }
    [SerializeField] private PlayerStat stat; // �߰��� �κ�

    private int currentExperience { get => data.currentExperience; set => data.currentExperience = value; }
    private int currentLevel { get => data.currentLevel; set => data.currentLevel = value; }
    private int currentGold { get => data.currentGold; set => data.currentGold = value; }
    private int experienceToNextLevel { get => data.experienceToNextLevel; set => data.experienceToNextLevel = value; }

    private PlayerStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponentInChildren<Animator>();
        Character = GetComponent<Character>();
        Input = GetComponent<PlayerController>();
        AnimationData.Initialize();

        // ���� �ӽ� �ʱ�ȭ �� ù ���� ����
        stateMachine = new PlayerStateMachine(this);

        SetInitialValues();
    }

    void SaveData()
    {
        PlayerData playerData = new PlayerData();
        playerData.currentLevel = currentLevel;
        playerData.currentExperience = currentExperience;
        playerData.currentGold = currentGold;
        playerData.experienceToNextLevel = experienceToNextLevel;
        playerData.maxHealth = Character.MaxHealth;
        playerData.currentHealth = Character.CurrentHealth;
        playerData.attackPower = Character.AttackPower;

        string saveData = JsonUtility.ToJson(playerData);

        SaveAndLoadManager.Instance.SavePlayerData(saveData);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    void LoadData()
    {
        PlayerData data = SaveAndLoadManager.Instance.LoadPlayerData();

        if (data == null)
        {
            SetInitialValues();
            Debug.Log("���嵥���Ͱ� ��� �ʱⰪ���� ����");
            return;
        }

        
        currentLevel = data.currentLevel;
        currentExperience = data.currentExperience;
        currentGold = data.currentGold;
        experienceToNextLevel = data.experienceToNextLevel;
        Character.MaxHealth = data.maxHealth;
        Character.CurrentHealth = data.currentHealth;
        Character.AttackPower = data.attackPower;
    }


    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        LoadData();
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }
    void SetInitialValues()
    {
        // �ʱ�ȭ
        currentLevel = stat.level;
        currentExperience = 0;
        currentGold = stat.gold;
        experienceToNextLevel = stat.level * 1000;
    }
    public void TakeDamage(float damage)
    { 
        Character.CurrentHealth -= damage;
    }
    public PlayerStateMachine GetStateMachine()
    {
        return stateMachine;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        while (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel;
            LevelUp();
        }
    }

    public void Heal()
    {
        if(currentGold >= 1000)
        {
            Character.CurrentHealth = Character.MaxHealth;
            currentGold -= 1000;
        }
        else
        {
            ErrorMessageText.Instance.ShowErrorMessage("��尡 �����մϴ�.");
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        experienceToNextLevel = currentLevel * 100;
        Character.MaxHealth += currentLevel * 10; // ���������� �ִ� ü�� ����
        Character.AttackPower += currentLevel * 2; // ���������� ���ݷ� ����
        Character.CurrentHealth = Character.MaxHealth; // ������ �� ü�� ȸ��
    }

    public int GetCurrentExperience()
    {
        return currentExperience;
    }

    public int GetExperienceToNextLevel()
    {
        return experienceToNextLevel;
    }

    public int GetLevel()
    {
        return currentLevel;
    }

    public int GetGold()
    {
        return currentGold;
    }
}
