using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtension.Editor;
#endif

namespace UnityExtension
{
    [AddComponentMenu("Unity Extension/Game Object Pool")]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public class GameObjectPool : ScriptableComponent
    {
        [System.Serializable]
        struct PoolSettings
        {
            public string name;
            public GameObject prefab;
            public int preallocateCount;
        }

        struct Pool
        {
            public GameObject prefab;
            public Stack<GameObject> stack;
        }

        struct DelayDespawnObject
        {
            public float time;
            public GameObject gameObject;
        }


        [SerializeField, Label("Don't Destroy on Load")]
        bool _dontDestroyOnLoad;

        [SerializeField, Label("Despawn All on Destroy")]
        bool _despawnAllOnDestroy;

        [SerializeField]
        PoolSettings[] _poolSettings;


        Dictionary<string, Pool> _pools;
        Dictionary<GameObject, Stack<GameObject>> _objectToStack;
        QuickLinkedList<DelayDespawnObject> _delayDespawnObjects;

        public static GameObjectPool lastAwaked { get; private set; }


        void AddObjects(Pool pool, int quantity)
        {
            while (quantity > 0)
            {
                var obj = Instantiate(pool.prefab);
                obj.transform.SetParent(transform, false);
                obj.SetActive(false);

                pool.stack.Push(obj);

                quantity--;
            }
        }


        void Awake()
        {
            lastAwaked = this;

            if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

            // Init pools
            _pools = new Dictionary<string, Pool>();

            for (int i = 0; i < _poolSettings.Length; i++)
            {
                ref var s = ref _poolSettings[i];

                var p = new Pool
                {
                    prefab = s.prefab,
                    stack = new Stack<GameObject>(Mathf.Max(s.preallocateCount, 4))
                };

                AddObjects(p, s.preallocateCount);

                _pools.Add(s.name, p);
            }

            _objectToStack = new Dictionary<GameObject, Stack<GameObject>>();

            _delayDespawnObjects = new QuickLinkedList<DelayDespawnObject>(16);
        }


        void OnDestroy()
        {
            if (_despawnAllOnDestroy)
            {
                DespawnAll();
            }
        }


        void Update()
        {
            while (_delayDespawnObjects.first >= 0)
            {
                var item = _delayDespawnObjects[_delayDespawnObjects.first];

                if (item.time > Time.time)
                {
                    return;
                }

                if (_objectToStack.ContainsKey(item.gameObject))
                {
                    Despawn(item.gameObject);
                }

                _delayDespawnObjects.RemoveFirst();
            }

            enabled = false;
        }


        public GameObject Spawn(string name)
        {
            var pool = _pools[name];
            if (pool.stack.Count == 0) AddObjects(pool, 1);

            var obj = pool.stack.Pop();
            _objectToStack.Add(obj, pool.stack);

            obj.transform.SetParent(null, false);
            obj.SetActive(true);

            return obj;
        }


        public void Despawn(GameObject target)
        {
            _objectToStack[target].Push(target);
            _objectToStack.Remove(target);

            target.SetActive(false);
            target.transform.SetParent(transform, false);
        }


        public void Despawn(GameObject target, float delay)
        {
            if (delay > 0f)
            {
                var newNodeValue = new DelayDespawnObject { time = delay + Time.time, gameObject = target };

                int id = _delayDespawnObjects.last;
                while (id >= 0)
                {
                    var node = _delayDespawnObjects.GetNode(id);

                    if (node.value.time <= newNodeValue.time)
                    {
                        _delayDespawnObjects.AddAfter(id, newNodeValue);
                        enabled = true;
                        return;
                    }

                    id = node.previous;
                }

                _delayDespawnObjects.AddFirst(newNodeValue);
                enabled = true;
            }
            else Despawn(target);
        }


        public void DespawnAll()
        {
            foreach (var p in _objectToStack)
            {
                p.Value.Push(p.Key);
                p.Key.SetActive(false);
                p.Key.transform.SetParent(transform, false);
            }
            _objectToStack.Clear();
            _delayDespawnObjects.Clear();
        }


#if UNITY_EDITOR

        HashSet<string> _hashSet = new HashSet<string>();


        bool HasRepeatedName(out string repeatedName)
        {
            repeatedName = null;
            bool result = false;

            if (_poolSettings != null)
            {
                for (int i = 0; i < _poolSettings.Length; i++)
                {
                    if (!_hashSet.Add(_poolSettings[i].name))
                    {
                        repeatedName = _poolSettings[i].name;
                        result = true;
                        break;
                    }
                }
                _hashSet.Clear();
            }

            return result;
        }


        [CustomEditor(typeof(GameObjectPool))]
        class GameObjectPoolEditor : BaseEditor<GameObjectPool>
        {
            public override void OnInspectorGUI()
            {
                using (new DisabledScope(Application.isPlaying))
                {
                    using (var scope = new ChangeCheckScope(target))
                    {
                        base.OnInspectorGUI();

                        if (target._poolSettings != null && scope.changed)
                        {
                            for (int i = 0; i < target._poolSettings.Length; i++)
                            {
                                ref var s = ref target._poolSettings[i];
                                if (string.IsNullOrEmpty(s.name) && s.prefab)
                                {
                                    s.name = s.prefab.name;
                                }
                            }
                        }
                    }
                }

                if (target.HasRepeatedName(out var repeatedName))
                {
                    EditorGUILayout.HelpBox("Has repeated name: " + (string.IsNullOrEmpty(repeatedName) ? "\"\"" : repeatedName), MessageType.Error);
                }
            }
        }

#endif // UNITY_EDITOR

    } // class GameObjectPool

} // namespace UnityExtension