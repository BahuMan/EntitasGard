using System.Collections.Generic;
using UnityEngine;

public class SelectedContainerBehaviour : MonoBehaviour {

    public RectTransform _unitPrefab;
    public RectTransform _turretPrefab;

    private List<RectTransform> _iconList;

    private void Start()
    {
        _iconList = new List<RectTransform>();
        _unitPrefab.gameObject.SetActive(false);
        _turretPrefab.gameObject.SetActive(false);
    }

    public void ClearSelection()
    {
        foreach (var icon in _iconList)
        {
            Destroy(icon.gameObject);
        }

        _iconList.Clear();
    }

    public void AddSelection(HexSelectable sel) 
    {
        RectTransform thumb;
        if (sel.name.Contains("Turret"))
        {
            thumb = Instantiate<RectTransform>(_turretPrefab);
        }
        else
        {
            thumb = Instantiate<RectTransform>(_unitPrefab);
        }
        thumb.SetParent(this.GetComponent<RectTransform>());
    }

}
