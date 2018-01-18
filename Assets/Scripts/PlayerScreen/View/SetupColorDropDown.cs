using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SetupColorDropDown : MonoBehaviour
{

    public Sprite preview;
    [SerializeField] Material[] colors;

    [ContextMenu("ReadColors")]
    public void ReadColors()
    {
        Object[] resColors = Resources.LoadAll("PlayerColors/");
        colors = new Material[resColors.Length];
        for (int c=0; c<resColors.Length; ++c)
        {
            colors[c] = (Material)resColors[c];
            GameObject go = new GameObject();
            Image img = go.AddComponent<Image>();
            img.sprite = preview;
        }

    }

    private void Start()
    {
        Dropdown drop = GetComponent<Dropdown>();
    }
}
