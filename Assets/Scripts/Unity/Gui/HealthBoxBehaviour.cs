using UnityEngine;
using UnityEngine.UI;

public class HealthBoxBehaviour : MonoBehaviour {


    private Bounds _bounds = new Bounds();
    private Bounds _screenBounds = new Bounds();

    private GameObject _target;
    private RectTransform _rect;
    private Slider _slider;
    private Image _sliderFill;
    private Image _frame;

    public Color TeamColor { get { return _sliderFill.color; } set { _sliderFill.color = value; } }
    public float MaxHealth { get { return _slider.maxValue; } set { _slider.maxValue = value; } }
    public float Health { get { return _slider.value; } set { _slider.value = value; } }
    public bool ShowFrame {  get { return _frame.enabled; } set { _frame.enabled = value; } }

    public void SetTarget(GameObject target)
    {
        _target = target;

        _bounds.center = Vector3.zero;
        _bounds.size = Vector3.zero;

        if (_target == null) return;

        transform.position = _target.transform.position;
        _bounds.center = transform.position;

        Renderer[] renderers = _target.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            _bounds.Encapsulate(r.bounds);
        }

        //convert to target-local coordinates
        _bounds.center -= transform.position;
    }

    private void Awake()
    {
        _frame = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
        _slider = GetComponentInChildren<Slider>();
        _sliderFill = _slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private Bounds CalcScreenBounds(Bounds b)
    {

        _screenBounds.center = Camera.main.WorldToScreenPoint(_target.transform.position);
        _screenBounds.size = Vector2.zero;

        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.min.x, b.min.y, b.min.z))));
        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.max.x, b.min.y, b.min.z))));
        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.max.x, b.max.y, b.min.z))));
        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.min.x, b.max.y, b.min.z))));
        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.min.x, b.min.y, b.max.z))));
        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.max.x, b.min.y, b.max.z))));
        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.max.x, b.max.y, b.max.z))));
        _screenBounds.Encapsulate(Camera.main.WorldToScreenPoint(_target.transform.TransformPoint(new Vector3(b.min.x, b.max.y, b.max.z))));

        return _screenBounds;
    }
    private void LateUpdate()
    {
        _screenBounds = CalcScreenBounds(_bounds);
        _rect.position = _screenBounds.min;
        _rect.sizeDelta = _screenBounds.size;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Became invisible");
        this.gameObject.SetActive(false);
    }

    private void OnBecameVisible()
    {
        Debug.Log("Became visible (peek a boo)");
        this.gameObject.SetActive(true);
    }
}
