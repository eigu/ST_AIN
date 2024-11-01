[System.Serializable]
public class QuestStepData
{
   public string state;

   public QuestStepData(string state)
   {
      this.state = state;
   }

   public QuestStepData()
   {
      state = "";
   }
}
