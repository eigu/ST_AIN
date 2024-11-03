using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("Quests")] 
    [SerializeField] private float _questStayTimeAfterDone = 2;
    private Dictionary<string, QuestUIData> _questUIData = new Dictionary<string, QuestUIData>();
    
    [Header("Notification")]
    [SerializeField] private TextMeshProUGUI _mainQuestNotificationText;
    [SerializeField] private float _mainQuestNotificationStayTime;
    
    
    [Header("MainQuest")]
    [SerializeField] private GameObject _mainQuestHolderPrefab;
    [SerializeField] private GameObject _mainQuestStepPrefab;
    [SerializeField] private Transform _mainQuestsParent; //parent

    [Header("SideQuest")] 
    [SerializeField] private GameObject _sideQuestHolderPrefab;
    [SerializeField] private GameObject _sideQuestStepPrefab;
    [SerializeField] private Transform _sideQuestsParent; //parent
    
    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuestUIEvent += StartShowQuestUI;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeUIEvent += UpdateStepUIText;
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent += FinishQuest;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuestUIEvent -= StartShowQuestUI;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeUIEvent -= UpdateStepUIText;
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent -= FinishQuest;
    }

    private void StartShowQuestUI(string id, string text, bool isSequential, int stepCount, QuestType type)
    {
        switch (type)
        {
            case QuestType.MainQuest:
                CreateQuestContainer(_mainQuestHolderPrefab, _mainQuestsParent, id, text, isSequential);
                PopulateQuestSteps(id, stepCount, _mainQuestStepPrefab);
                StartCoroutine(ShowMainQuestNotification(text));
                break;
            case QuestType.SideQuest:
                CreateQuestContainer(_sideQuestHolderPrefab, _sideQuestsParent, id, text, isSequential);
                PopulateQuestSteps(id, stepCount, _sideQuestStepPrefab);
                break;
        }
    }

    private void CreateQuestContainer(GameObject questHolderPrefab, Transform questsParent, string id, string text, bool isSequential)
    {
        GameObject container = Instantiate(questHolderPrefab, questsParent);

        container.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
            
        _questUIData.Add(id, new QuestUIData {holder = container,stepHolder = container.transform.GetChild(1) ,isSequential = isSequential, questStepsTMP = new List<TextMeshProUGUI>()});

    }
    
    private void PopulateQuestSteps(string id, int stepCount, GameObject questStepPrefab)
    {
        for (int i = 0; i < stepCount; i++)
        {
            _questUIData[id].questStepsTMP.Add(CreateStepInstanceAndEnable(questStepPrefab, _questUIData[id].stepHolder));
        }
    }

    private TextMeshProUGUI CreateStepInstanceAndEnable(GameObject o, Transform parent)
    {
        o = Instantiate(o, parent);
        o.SetActive(false);
        return o.GetComponent<TextMeshProUGUI>();
    }
    
    private void UpdateStepUIText(string id, int index, string text, bool isFinished)
    {
        if (!_questUIData.ContainsKey(id)) return;
        
        var questData = _questUIData[id];

        if (!isFinished)
        {
            questData.questStepsTMP[index].gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(RemoveQuestStepUI(questData.questStepsTMP[index].transform, _questStayTimeAfterDone));
        }

        questData.questStepsTMP[index].text = text;
    }
    
    private void FinishQuest(string id)
    {
        StartCoroutine(RemoveQuestUI(_questUIData[id].holder.gameObject, _questStayTimeAfterDone));
    }

    private IEnumerator ShowMainQuestNotification(string text)
    {
        _mainQuestNotificationText.text = $"New Main Quest: {text}";
        _mainQuestNotificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(_mainQuestNotificationStayTime);
        
        _mainQuestNotificationText.gameObject.SetActive(false);
    }
    
    private IEnumerator RemoveQuestUI(GameObject o, float secs)
    {
        yield return new WaitForSeconds(secs);
        o.SetActive(false);
    }
    
    private IEnumerator RemoveQuestStepUI(Transform step, float secs)
    {
        step.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(secs);
        step.gameObject.SetActive(false);
    }

    private struct QuestUIData
    {
        public GameObject holder;
        public Transform stepHolder;
        public bool isSequential;
        public List<TextMeshProUGUI> questStepsTMP;

    }
    
}
