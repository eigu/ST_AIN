[System.Serializable]
public class QuestData
{
    public QuestState state;
    public int questStepIndex;
    public QuestStepData[] questStepData;

    public QuestData(QuestState state, int questStepIndex, QuestStepData[] questStepData)
    {
        this.state = state;
        this.questStepIndex = questStepIndex;
        this.questStepData = questStepData;
    }
}
