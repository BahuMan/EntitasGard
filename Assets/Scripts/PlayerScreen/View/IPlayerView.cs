namespace PlayerList.View
{
    public delegate void DropdownHandler(int lineNr, int choiceNr);
    public delegate void InputFieldHandler(int lineNr, string content);
    public delegate void ButtonClickHandler(int lineNr);

    public interface IPlayerView
    {
        event DropdownHandler OnColorChosen;
        event DropdownHandler OnPlayertypeChosen;
        event DropdownHandler OnTeamChosen;

        event InputFieldHandler OnNameEdited;

        event ButtonClickHandler OnAddLineClicked;
        event ButtonClickHandler OnRemoveLineClicked;

        void SetLineNr(int nr);
        void SetColor(int c);
        void SetPlayerType(PlayerTypeEnum pt);
        void SetName(string name);
        void SetTeam(int team);

    }
}
