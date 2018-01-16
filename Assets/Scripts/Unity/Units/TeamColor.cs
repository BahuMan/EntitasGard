using UnityEngine;

public class TeamColor : MonoBehaviour {

    public int teamNr = 0;

    public Renderer[] _renderer;

    public Color teamColor
    {
        set
        {
            foreach (var r in _renderer)
            {
                r.material.color = value;
            }
        }
        get
        {
            return _renderer[0].material.color;
        }
    }
}
