using UnityEngine;
using UnityEngine.UI;
using PlayerList.View;
using PlayerList.Model;
using UnityEngine.SceneManagement;

public class PlayerListData : MonoBehaviour
{
    public RectTransform _viewParent;
    public PlayerView _prototype;
    private PlayerViewFactory _factory;
    public PlayerListModel _model;
#pragma warning disable 0414
    private PlayerCtrl _ctrl;
#pragma warning restore 0414

    private void Start()
    {
        _prototype.gameObject.SetActive(false);
        GameObject.DontDestroyOnLoad(this.gameObject);
        _factory = new PlayerViewFactory(_viewParent, _prototype);
        _model = new PlayerListModel();
        _ctrl = new PlayerCtrl(_model, _factory);
    }

    //called by Start Button (see inspector)
    public void StartGame()
    {
        SceneManager.LoadScene("FirstScene");
    }
}
