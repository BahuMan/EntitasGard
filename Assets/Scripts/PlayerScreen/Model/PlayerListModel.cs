using System.Collections;
using System.Collections.Generic;

namespace PlayerList.Model
{
    public class PlayerListModel: IEnumerable<PlayerModel>, IList<PlayerModel>
    {
        private List<PlayerModel> _list;
        public PlayerListModel()
        {
            _list = new List<PlayerModel>();
        }

        public PlayerModel this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public int Count { get { return _list.Count; } }

        public bool IsReadOnly { get { return false; } }

        public void Add(PlayerModel item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(PlayerModel item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(PlayerModel[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PlayerModel> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(PlayerModel item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, PlayerModel item)
        {
            _list.Insert(index, item);
        }

        public bool Remove(PlayerModel item)
        {
            return _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
