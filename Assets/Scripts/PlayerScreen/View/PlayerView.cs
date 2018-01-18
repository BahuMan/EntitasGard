using UnityEngine;
using UnityEngine.UI;

namespace PlayerList.View
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        public event DropdownHandler OnColorChosen;
        public event DropdownHandler OnPlayertypeChosen;
        public event DropdownHandler OnTeamChosen;

        public event InputFieldHandler OnNameEdited;

        public event ButtonClickHandler OnAddLineClicked;
        public event ButtonClickHandler OnRemoveLineClicked;

        public int _lineNr;
#pragma warning disable 0649
        [SerializeField] private Text _colorText;
        [SerializeField] private Dropdown _colorDropdown;
        [SerializeField] private Dropdown _playerTypeDropdown;
        [SerializeField] private InputField _nameInputField;
        [SerializeField] private Dropdown _teamDropdown;
        [SerializeField] private Button _addLine;
        [SerializeField] private Button _removeLine;
#pragma warning restore 0649


        private void OnEnable()
        {
            _colorDropdown.onValueChanged.AddListener(Color_changed);
            _playerTypeDropdown.onValueChanged.AddListener(Playertype_changed);
            _nameInputField.onEndEdit.AddListener(NameInputfield_EndEdit);
            _teamDropdown.onValueChanged.AddListener(Team_changed);
            _addLine.onClick.AddListener(AddLine_Clicked);
            _removeLine.onClick.AddListener(RemoveLine_Clicked);
        }

        private void OnDisable()
        {
            _colorDropdown.onValueChanged.RemoveListener(Color_changed);
            _playerTypeDropdown.onValueChanged.RemoveListener(Playertype_changed);
            _nameInputField.onEndEdit.RemoveListener(NameInputfield_EndEdit);
            _teamDropdown.onValueChanged.RemoveListener(Team_changed);
            _addLine.onClick.RemoveListener(AddLine_Clicked);
            _removeLine.onClick.RemoveListener(RemoveLine_Clicked);
        }

        private void RemoveLine_Clicked()
        {
            if (OnRemoveLineClicked != null) OnRemoveLineClicked(_lineNr);
        }

        private void AddLine_Clicked()
        {
            if (OnAddLineClicked != null) OnAddLineClicked(_lineNr);
        }

        private void NameInputfield_EndEdit(string newName)
        {
            if (OnNameEdited != null) OnNameEdited(_lineNr, newName);
        }

        private void Team_changed(int newVal)
        {
            if (OnTeamChosen != null) OnTeamChosen(_lineNr, newVal);
        }

        private void Playertype_changed(int newVal)
        {
            if (OnPlayertypeChosen != null) OnPlayertypeChosen(_lineNr, newVal);
        }

        private void Color_changed(int newVal)
        {
            if (OnColorChosen != null) OnColorChosen(_lineNr, newVal);
        }

        public void SetLineNr(int nr)
        {
            transform.SetSiblingIndex(nr);
            _lineNr = nr;
        }

        public void SetColor(int c)
        {
            _colorText.text = "" + c;
            _colorDropdown.value = c;
        }

        public void SetPlayerType(PlayerTypeEnum pt)
        {
            _playerTypeDropdown.value = (int)pt;
        }

        public void SetName(string name)
        {
            _nameInputField.text = name;
        }

        public void SetTeam(int team)
        {
            _teamDropdown.value = team;
        }

    }
}
