using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Common.Modules
{
    public sealed class ComponentPoolNonAlloc : MonoBehaviour
    {
        readonly Queue<Component> _particleSystems = new Queue<Component>();
        [SerializeField] byte _instantiationCount = 8;
        [SerializeField] Component _component = null;

        void Awake()
        {
#if UNITY_EDITOR
            Assert.IsTrue(PrefabUtility.IsPartOfPrefabAsset(_component.gameObject), 
                $"The Component assigned has to be fulfill requirement: '{nameof(PrefabUtility.IsPartOfPrefabAsset)}'." +
                $"\nBut you still have to drag and drop the actual Component attached to it onto it.");
#endif

            StartCoroutine(InstantiateComponents());
            IEnumerator InstantiateComponents()
            {
                for (byte i = 0; i < _instantiationCount; i++)
                {
                    Component component = Instantiate(_component);
                    Return(component);
                }
                yield break;
            }
        }

        public T Get<T>() where T : Component
        {
            Component component = _particleSystems.Dequeue();
            component.transform.parent = null;
            component.gameObject.SetActive(true);
            return (T)component;
        }

        public void Return(Component component)
        {
            component.transform.SetParent(transform);
            component.gameObject.SetActive(false);
            _particleSystems.Enqueue(component);
        }
    }
}
