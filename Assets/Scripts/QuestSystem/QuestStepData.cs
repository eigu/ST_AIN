[System.Serializable]
public class QuestStepData
{
   public string state;
   public string stateTextDisplay;

   public QuestStepData(string state, string stateTextDisplay)
   {
      this.state = state;
      this.stateTextDisplay = stateTextDisplay;
   }

   public QuestStepData()
   {
      state = "";
      stateTextDisplay = "";
   }
}
