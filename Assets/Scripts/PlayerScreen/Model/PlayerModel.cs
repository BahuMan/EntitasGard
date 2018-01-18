
namespace PlayerList.Model
{
    [System.Serializable]
    public class PlayerModel
    {
        public delegate void IntChangedHandler(int newColor);
        public delegate void StringChangedHandler(string newString);
        public delegate void TypeChangedHandler(PlayerTypeEnum newType);

        public event IntChangedHandler OnColorChanged;
        public event TypeChangedHandler OnTypeChanged;
        public event StringChangedHandler OnNameChanged;
        public event IntChangedHandler OnTeamChanged;

        public int Color { get { return _color; } set { UnityEngine.Debug.Log("color from " + _color + " to " + value); _color = value; if (OnColorChanged != null) OnColorChanged(value); } }
        public PlayerTypeEnum Type { get { return _type; } set { _type = value; if (OnTypeChanged != null) OnTypeChanged(value); } }
        public string Name { get { return _name; } set { _name = value; if (OnNameChanged != null) OnNameChanged(value); } }
        public int Team { get { return _team; } set { _team = value; if (OnTeamChanged != null) OnTeamChanged(value); } }

        //duplicates values, but not listeners
        public PlayerModel Duplicate()
        {
            return new PlayerModel() { Color = this.Color, Name = this.Name, Team = this.Team, Type = this.Type };
        }

        private int _color;
        private PlayerTypeEnum _type;
        private string _name;
        private int _team;
    }
}
