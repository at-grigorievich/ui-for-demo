namespace ATG.Transition
{
    public enum TransitionType: ushort
    {
        None = 0,
        ExcitePlacement = 1,
        Relax = 2,
        SpawnCube = 3,
        FearCube = 4,
        StackCubes = 5,
        ChangeTextOutput = 6,
        PressButton = 7,
        HideCube = 8,
        FearPlacement = 9,
        RelaxCube = 10,
        ScaleButton = 11,
        ShowVerticalUI = 13,
        HideVerticalUI = 14,
        RollEmojiUI = 15,
        TutorialHandMove = 16
    }
}