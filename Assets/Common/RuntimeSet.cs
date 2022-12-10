using System.Collections.Generic;
using UnityEngine;

namespace SeedWork.RuntimeSet
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        protected const string _path = "RuntimeSet/";

        public T this[int index] 
        {
            get { return _items[index]; }
        }

        [SerializeField] List<T> _items = new List<T>();

        public int Count { get; private set; } = 0;

        public void Register(T item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
                Count++;
                //OnItemCountChanged?.Invoke(Items);
            }
        }

        public void Deregister(T item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                Count--;
                //OnItemCountChanged?.Invoke(Items);
            }
        }

        //public delegate void OnItemCountChangedHandler(List<T> items);
        //public event OnItemCountChangedHandler OnItemCountChanged;

//#if UNITY_EDITOR
//        [UnityEditor.MenuItem("Assets/Create/New Collection Type", false, 1)]
//        static void CreateNewMarkDownLanguageFile()
//        {
//            UnityEditor.ProjectWindowUtil.CreateAssetWithContent("readme.md", idkyet); 
//        }
//#endif
    }
}
