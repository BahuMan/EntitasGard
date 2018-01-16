using UnityEngine;
using UnityEngine.UI;

public class CommandPanelBehaviour : MonoBehaviour
{

    public delegate void UICommandHandler();

    public Button _attackButton;
    public event UICommandHandler AttackCommand;
    public Button _navigateButton;
    public event UICommandHandler NavigateCommand;

    public RectTransform _buildMenu;
    public Button _buildButton;
    public RectTransform _buildSubMenu;
        public Button _buildBarracksButton;
        public event UICommandHandler BuildBarracksCommand;
        public Button _buildTowerButton;
        public event UICommandHandler BuildTowerCommand;

    public RectTransform _newUnitMenu;
    public Button _newUnitButton;
    public RectTransform _newUnitSubMenu;
        public Button _newVehicleButton;
        public event UICommandHandler newVehicleCommand;


    public void Start()
    {
        _attackButton.onClick.AddListener(() => { if (AttackCommand != null) AttackCommand(); });
        _navigateButton.onClick.AddListener(() => { if (NavigateCommand != null) NavigateCommand(); });

        _buildButton.onClick.AddListener(() => { _buildSubMenu.gameObject.SetActive(!_buildSubMenu.gameObject.activeSelf); });
        _buildBarracksButton.onClick.AddListener(() => { if (BuildBarracksCommand != null) BuildBarracksCommand(); });
        _buildTowerButton.onClick.AddListener(() => { if (BuildTowerCommand != null) BuildTowerCommand(); });

        _newUnitButton.onClick.AddListener(() => { _newUnitSubMenu.gameObject.SetActive(_newUnitSubMenu.gameObject.activeSelf); });
        _newVehicleButton.onClick.AddListener(() => { if (newVehicleCommand != null) newVehicleCommand(); });
        ShowNothing();
    }

    public void ShowNothing()
    {
        _attackButton.gameObject.SetActive(false);
        _navigateButton.gameObject.SetActive(false);

        _buildMenu.gameObject.SetActive(false);
        _buildButton.gameObject.SetActive(false);
        _buildSubMenu.gameObject.SetActive(false);
            _buildBarracksButton.gameObject.SetActive(false);
            _buildTowerButton.gameObject.SetActive(false);

        _newUnitMenu.gameObject.SetActive(false);
        _newUnitButton.gameObject.SetActive(false);
        _newUnitSubMenu.gameObject.SetActive(false);
            _newVehicleButton.gameObject.SetActive(false);

    }

public void ShowAttack()
    {
        _attackButton.gameObject.SetActive(true);
    }

    public void ShowNavigate()
    {
        _navigateButton.gameObject.SetActive(true);
    }

    public void ShowBuild()
    {
        _buildMenu.gameObject.SetActive(true);
        _buildButton.gameObject.SetActive(true);
    }

    public void ShowBuildBarracks()
    {
        _buildBarracksButton.gameObject.SetActive(true);
        ShowBuild();
    }

    public void ShowBuildTower()
    {
        _buildTowerButton.gameObject.SetActive(true);
        ShowBuild();
    }

    public void ShowNewUnit()
    {
        _newUnitMenu.gameObject.SetActive(true);
        _newUnitButton.gameObject.SetActive(true);
    }

    public void ShowNewVehicle()
    {
        _newVehicleButton.gameObject.SetActive(true);
        ShowNewUnit();
    }

}
