using Entitas;
using UnityEngine;

namespace Systems.View
{
    public class ShowSelectionBox : IExecuteSystem
    {
        private IGroup<InputEntity> _boxCoordinates;
        private RectTransform _img;

        public ShowSelectionBox(Contexts contexts)
        {
            _boxCoordinates = contexts.input.GetGroup(InputMatcher.ScreenCoordinates);
            _img = GameObject.Find("SelectionBox").GetComponent<RectTransform>();
            _img.pivot = Vector2.zero;
            _img.anchorMin = Vector2.zero;
            _img.anchorMax = Vector3.zero;
        }

        void IExecuteSystem.Execute()
        {
            if (_boxCoordinates.count != 2)
            {
                _img.gameObject.SetActive(false);
                return;
            }

            InputEntity[] box = _boxCoordinates.GetEntities();

            Rect r = new Rect();
            r.x = Mathf.Min(box[0].screenCoordinates.x, box[1].screenCoordinates.x);
            r.y = Mathf.Min(box[0].screenCoordinates.y, box[1].screenCoordinates.y);
            r.width = Mathf.Abs(box[0].screenCoordinates.x - box[1].screenCoordinates.x);
            r.height= Mathf.Abs(box[0].screenCoordinates.y - box[1].screenCoordinates.y);

            if (r.width < Selection.ClickSelect.MAX_CLICK_DISTANCE
                || r.height < Selection.ClickSelect.MAX_CLICK_DISTANCE)
            {
                _img.gameObject.SetActive(false);
                return;
            }

            _img.position = new Vector3(r.x, r.y, 0);
            _img.sizeDelta = new Vector2(r.width, r.height);
            _img.gameObject.SetActive(true);
        }
    }
}
