using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("MainQuest")] 
    [SerializeField] private Transform _mainQuestParent;
    [SerializeField] private TextMeshProUGUI _mainQuestTitle;
    [SerializeField] private Transform _mainQuestStepParent;
    private GameObject _mainQuestStepTemplate;
    private string _mainQuestID;
    private List<TextMeshProUGUI> _mainStepsTMPs;

    [Header("SideQuest")] 
    [SerializeField] private TextMeshProUGUI _sideQuestTitle;
    [SerializeField] private Transform _sideQuestStepParent;
    private GameObject _sideQuestStepTemplate;
    
    

    private void Awake()
    {
        GameObject mainQuestStepTemplate = _mainQuestStepParent.GetChild(0).gameObject;
        _mainQuestStepTemplate = mainQuestStepTemplate;
        mainQuestStepTemplate.SetActive(false);
        
        GameObject sideQuestStepTemplate = _sideQuestStepParent.GetChild(0).gameObject;
        _sideQuestStepTemplate = sideQuestStepTemplate;
        sideQuestStepTemplate.SetActive(false);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuestUIEvent += InitializeQuestUI;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeUIEvent += UpdateStepUIText;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuestUIEvent -= InitializeQuestUI;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeUIEvent -= UpdateStepUIText;
    }

    private void InitializeQuestUI(string id, string text, int stepCount, QuestType type)
    {
        if (type == QuestType.MainQuest)
        {
            _mainQuestParent.gameObject.SetActive(true);
            _mainQuestID = id;
            _mainQuestTitle.text = text;
            _mainStepsTMPs = new List<TextMeshProUGUI>();
            
            //instantiate steps
            for (int i = 0; i < stepCount; i++)
            {
                _mainStepsTMPs.Add(CreateStepInstanceAndEnable(_mainQuestStepTemplate, _mainQuestStepParent));
            }
            
        }

        if (type == QuestType.SideQuest)
        {
            _sideQuestTitle.text = text;
            
            //instantiate steps
        }
    }

    private TextMeshProUGUI CreateStepInstanceAndEnable(GameObject o, Transform parent)
    {
        o = Instantiate(o, parent);
        o.SetActive(false);
        return o.GetComponent<TextMeshProUGUI>();
    }
    
    private void UpdateStepUIText(string id, int index, string text, bool isFinished, QuestType type)
    {
        if (type == QuestType.MainQuest)
        {
            if (isFinished)
            {
                foreach (var o in _mainStepsTMPs)
                {
                    if (o.gameObject.activeInHierarchy)
                    {
                        break;
                    }

                    _mainQuestParent.gameObject.SetActive(false);
                    break;
                }
            }
            
            _mainStepsTMPs[index].gameObject.SetActive(!isFinished);
            _mainStepsTMPs[index].text = text;

            
        }

        if (type == QuestType.SideQuest)
        {
            
        }
        
    }

    

}
