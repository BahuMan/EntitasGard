using Entitas;
using UnityEngine;

namespace Systems.View
{
    public class ShowSelectionBox : IExecuteSystem
    {
        private IGroup<InputEntity> _selectStart;
        private RectTransform _img;

        public ShowSelectionBox(Contexts contexts)
        {
            _selectStart = contexts.input.GetGroup(InputMatcher.SelectionBox);
            _img = GameObject.Find("SelectionBox").GetComponent<RectTransform>();
            _img.pivot = Vector2.zero;
            _img.anchorMin = Vector2.zero;
            _img.anchorMax = Vector3.zero;
        }

        void IExecuteSystem.Execute()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                _img.gameObject.SetActive(true);
                foreach (var box in _selectStart)
                {

                    Rect r = new Rect();
                    r.x = box.selectionBox.x; //Mathf.Min(box.selectionBox.x, UnityEngine.Input.mousePosition.x);
                    r.y = box.selectionBox.y; //Mathf.Min(box.selectionBox.y, UnityEngine.Input.mousePosition.y);
                    r.width = box.selectionBox.width;   //Mathf.Abs(box.selectionBox.x - UnityEngine.Input.mousePosition.x);
                    r.height = box.selectionBox.height; //Mathf.Abs(box.selectionBox.y - UnityEngine.Input.mousePosition.y);

                    if (r.width < 0)
                    {
                        r.width = Mathf.Abs(r.width);
                    } else
                    {
                        r.x -= r.width;
                    }
                    if (r.height < 0)
                    {
                        r.height = Mathf.Abs(r.height);
                    }
                    else {
                        r.y -= r.height;
                    }
                    _img.position = new Vector3( r.x, r.y, 0);
                    _img.sizeDelta = new Vector2(r.width, r.height);
                }
            }
            else
            {
                _img.gameObject.SetActive(false);
            }
        }
    }
}
