using UnityEngine;
using PlayerList.View;

public class PlayerViewFactory
{
    private RectTransform _parent;
    private PlayerView _prefab;

    public PlayerViewFactory(RectTransform viewParent, PlayerView prefab)
    {
        _prefab = prefab;
        _parent = viewParent;
    }

    public IPlayerView CreateNewPlayer()
    {
        PlayerView pv = GameObject.Instantiate<PlayerView>(_prefab, _parent, false);
        pv.gameObject.SetActive(true);
        return pv;
    }

    public void Destroy(IPlayerView pv)
    {
        GameObject.Destroy(((PlayerView) pv).gameObject);
    }
}
