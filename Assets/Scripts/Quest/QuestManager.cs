using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI ��ҵ� ")]
    public GameObject questUI;
    public Text questTitletext;
    public Text questDescriptionText;
    public Text questProgressText;
    public Button completeButton;

    [Header("����Ʈ ���")]
    public QuestData[] availableQuests;

    private QuestData currentQuest;
    private int currentQuestIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (availableQuests.Length > 0)
        {
            StartQuest(availableQuests[0]);
        }
        if (completeButton != null)
        {
            completeButton.onClick.AddListener(CompleteCurrentQuest);
        }
    }

    void Update()
    {
        if (currentQuest != null && currentQuest.isActive)
        {
            CheckQuestProgress();
            UpdateQuestUI();
        }
    }

    void UpdateQuestUI()
    {
        if (currentQuest == null) return;

        if (questTitletext != null)
        {
            questTitletext.text = currentQuest.questTitle;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = currentQuest.description;
        }

        if (questProgressText != null)
        {
            questProgressText.text = currentQuest.GetPogressText();
        }
    }


    public void StartQuest(QuestData quest)
    {
        if(quest == null) return;

        currentQuest = quest;
        currentQuest.Initialize();
        currentQuest.isActive = true;

        Debug.Log("����Ʈ ���� : " + questTitletext);
        UpdateQuestUI();
        if (questUI != null)
        {
            questUI.SetActive(true);
        }
    }

    //��� ����Ʈ
    void CheckDeliveryProgress()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, currentQuest.deliveryPosition);

        if (distance <= currentQuest.deliveryRedius)    //������ �Ÿ��� ������������ �˻�
        {
            if (currentQuest.currentProgress == 0)
            {
                currentQuest.currentProgress = 1;       //�Ϸ�
            }
        }
        else
        {
            currentQuest.currentProgress = 0;           //����
        }
       
    }

    //���� ����Ʈ
    public void AddCollectProgress(string itemTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Collect && currentQuest.targetTag == itemTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("������ ���� : " + itemTag);
        }
    }

    //��ȣ�ۿ� ����Ʈ
    public void AddInteractProgress(string objectTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Interact && currentQuest.targetTag == objectTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("��ȣ�ۿ� �Ϸ� : " +  objectTag);   
        }
    }

    //���� ����Ʈ �Ϸ�
    public void CompleteCurrentQuest()
    {
        if(currentQuest == null || !currentQuest.isCompleted) return;

        Debug.Log("����Ʈ �Ϸ�!" + currentQuest.rewardMessage);
        if (completeButton != null)
        {
            completeButton.gameObject.SetActive(false);
        }

        currentQuestIndex++;
        if (currentQuestIndex < availableQuests.Length)
        {
            StartQuest(availableQuests[currentQuestIndex]);
        }
        else
        {
            currentQuest = null;
            if (questUI != null)
            {
                questUI.gameObject.SetActive(false);
            }
        }
    }

    void CheckQuestProgress()
    {
        if (currentQuest.questType == QuestType.Delivery)
        {
            CheckDeliveryProgress();
        }

        if (currentQuest.IsComplete() && !currentQuest.isCompleted)
        {
            currentQuest.isCompleted = true;

            if (completeButton != null)
            {
                completeButton.gameObject.SetActive(true);
            }
        }
    }
}
