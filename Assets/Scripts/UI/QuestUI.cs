using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("Quests")] 
    private Dictionary<string, QuestUIData> _questUIData = new Dictionary<string, QuestUIData>();
    
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
        GameEventsManager.Instance.QuestEvents.OnStartQuestUIEvent += InitializeQuestUI;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeUIEvent += UpdateStepUIText;
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent += RemoveQuestUI;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuestUIEvent -= InitializeQuestUI;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeUIEvent -= UpdateStepUIText;
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent -= RemoveQuestUI;
    }

    private void InitializeQuestUI(string id, string text, bool isSequential, int stepCount, QuestType type)
    {
        switch (type)
        {
            case QuestType.MainQuest:
                CreateQuestContainer(_mainQuestHolderPrefab, _mainQuestsParent, id, text, isSequential);
                PopulateQuestSteps(id, stepCount, _mainQuestStepPrefab);
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

        if (questData.isSequential)
        {
            questData.questStepsTMP[index].gameObject.SetActive(!isFinished);
        }
        else
        {
            questData.questStepsTMP[index].gameObject.SetActive(true);
        }

        questData.questStepsTMP[index].text = text;
        
    }
    
    private void RemoveQuestUI(string id)
    {
        Destroy(_questUIData[id].holder.gameObject);
        _questUIData.Remove(id);
    }

    private struct QuestUIData
    {
        public GameObject holder;
        public Transform stepHolder;
        public bool isSequential;
        public List<TextMeshProUGUI> questStepsTMP;

    }
    
}
