using UnityEngine;
using UnityEngine.UI;

public class CommandPanelBehaviour : MonoBehaviour
{

    public delegate void UICommandHandler();
    public event UICommandHandler AttackCommand;
    public event UICommandHandler NavigateCommand;

    public Button _attackButton;
    public Button _navigateButton;

    public void Start()
    {
        ShowNothing();
    }

    public void ShowNothing()
    {
        _attackButton.onClick.AddListener(() => { if (AttackCommand != null) AttackCommand();});
        _attackButton.gameObject.SetActive(false);

        _navigateButton.onClick.AddListener(() => { if (NavigateCommand != null) NavigateCommand(); });
        _navigateButton.gameObject.SetActive(false);
    }

    public void ShowAttack()
    {
        _attackButton.gameObject.SetActive(true);
    }

    public void ShowNavigate()
    {
        _navigateButton.gameObject.SetActive(true);
    }
}
