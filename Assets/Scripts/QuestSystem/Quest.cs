
using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;

    private int _currentQuestStepIndex;
    private QuestStepData[] _questStepData;

    public Quest(QuestInfoSO questInfo)
    {
        info = questInfo;
        state = QuestState.RequirementsNotMet;
        _currentQuestStepIndex = 0;
        _questStepData = new QuestStepData[info.questStepsPrefabs.Length];

        for (int i = 0; i < _questStepData.Length; i++)
        {
            _questStepData[i] = new QuestStepData();
        }
    }

    public Quest(QuestInfoSO info, QuestState state, int currentQuestStepIndex, QuestStepData[] questStepData)
    {
        this.info = info;
        this.state = state;
        _currentQuestStepIndex = currentQuestStepIndex;
        _questStepData = questStepData;

        if (_questStepData.Length != this.info.questStepsPrefabs.Length)
        {
            Debug.LogWarning("steps and states are different lengths. Something's wrong, i can feel it...");
        }
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (_currentQuestStepIndex < info.questStepsPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();

        if (questStepPrefab != null)
        {
            //can be pooled
            QuestStep qs = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            qs.InitializeQuestStep(info.ID, _currentQuestStepIndex, _questStepData[_currentQuestStepIndex].state);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepsPrefabs[_currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning($"Step index out of bounds. Quest ID: {info.ID}, Step Index: {_currentQuestStepIndex}");
        }

        return questStepPrefab;
    }

    public void StoreQuestStepData(QuestStepData questStepData, int stepIndex)
    {
        if (stepIndex < _questStepData.Length)
        {
            _questStepData[stepIndex].state = questStepData.state;
        }
        else
        {
            Debug.LogWarning($"Step index was out of range. Quest ID: {info.ID} Step Index: {stepIndex}");
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, _currentQuestStepIndex, _questStepData);
    }
}
