
public enum QuestState
{
    RequirementsNotMet,
    CanStart, // reqs met
    InProgress, // started
    CanFinish, // steps done
    Finished // claimed
}
