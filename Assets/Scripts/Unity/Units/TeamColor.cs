using UnityEngine;

public class TeamColor : MonoBehaviour {

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
