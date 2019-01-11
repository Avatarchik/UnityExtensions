using System;
using System.Collections.Generic;

namespace UnityExtensions
{
    /// <summary>
    /// 快速链表，与 LinkedList 相比，本实现使用连续的 struct node 提高性能
    /// 注意：必须使用带参数的构造函数
    /// </summary>
    public struct QuickLinkedList<T>
    {
        /// <summary>
        /// 链表节点
        /// </summary>
        public struct Node
        {
            public int previous;
            public int next;
            public T value;
        }


        Stack<int> _emptyIds;
        List<Node> _list;
        int _first;
        int _last;


        /// <summary>
        /// 链表第一个节点的 id
        /// </summary>
        public int first { get { return _first; } }


        /// <summary>
        /// 链表最后一个节点的 id
        /// </summary>
        public int last { get { return _last; } }


        /// <summary>
        /// 链表节点总数
        /// </summary>
        public int count { get { return _list.Count - _emptyIds.Count; } }


        /// <summary>
        /// 通过 id 访问节点的值
        /// </summary>
        public T this[int id]
        {
            get
            {
                var node = _list[id];
                if (node.previous == node.next && _first != id)
                {
                    throw new Exception("invalid id");
                }

                return node.value;
            }
            set
            {
                var node = _list[id];
                if (node.previous == node.next && _first != id)
                {
                    throw new Exception("invalid id");
                }

                node.value = value;
                _list[id] = node;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public QuickLinkedList(int capacity)
        {
            _emptyIds = new Stack<int>(4);
            _list = new List<Node>(capacity);
            _first = -1;
            _last = -1;
        }


        /// <summary>
        /// 
        /// </summary>
        public Node GetNode(int id)
        {
            return _list[id];
        }


        /// <summary>
        /// 
        /// </summary>
        public int GetPrevious(int id)
        {
            return _list[id].previous;
        }


        /// <summary>
        /// 
        /// </summary>
        public int GetNext(int id)
        {
            return _list[id].next;
        }


        int Add(ref Node node)
        {
            int index;

            if (_emptyIds.Count > 0)
            {
                index = _emptyIds.Pop();
                _list[index] = node;
            }
            else
            {
                _list.Add(node);
                index = _list.Count - 1;
            }

            return index;
        }


        void Remove(int id, ref Node node)
        {
            if (_first == id) _first = node.next;
            if (_last == id) _last = node.previous;

            if (node.previous != -1)
            {
                Node tmp = _list[node.previous];
                tmp.next = node.next;
                _list[node.previous] = tmp;
                node.previous = -1;
            }
            if (node.next != -1)
            {
                Node tmp = _list[node.next];
                tmp.previous = node.previous;
                _list[node.next] = tmp;
                node.next = -1;
            }

            node.value = default;
            _list[id] = node;
            _emptyIds.Push(id);
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddFirst(T value)
        {
            var node = new Node { previous = -1, next = _first, value = value };
            int newId = Add(ref node);

            if (_first == -1) _last = newId;
            else
            {
                var tmp = _list[_first];
                tmp.previous = newId;
                _list[_first] = tmp;
            }
            _first = newId;

            return newId;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddLast(T value)
        {
            var node = new Node { previous = _last, next = -1, value = value };
            int newId = Add(ref node);

            if (_first == -1) _first = newId;
            else
            {
                var tmp = _list[_last];
                tmp.next = newId;
                _list[_last] = tmp;
            }
            _last = newId;

            return _last;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddAfter(int id, T value)
        {
            var prevNode = _list[id];
            if (prevNode.previous == prevNode.next && _first != id)
            {
                throw new Exception("invalid id");
            }

            var node = new Node { previous = id, next = prevNode.next, value = value };
            int newId = Add(ref node);

            if (prevNode.next == -1)
            {
                _last = newId;
            }
            else
            {
                var tmp = _list[prevNode.next];
                tmp.previous = newId;
                _list[prevNode.next] = tmp;
            }
            prevNode.next = newId;
            _list[id] = prevNode;

            return newId;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns> id </returns>
        public int AddBefore(int id, T value)
        {
            var nextNode = _list[id];
            if (nextNode.previous == nextNode.next && _first != id)
            {
                throw new Exception("invalid id");
            }

            var node = new Node { previous = nextNode.previous, next = id, value = value };
            int newId = Add(ref node);

            if (nextNode.previous == -1)
            {
                _first = newId;
            }
            else
            {
                var tmp = _list[nextNode.previous];
                tmp.next = newId;
                _list[nextNode.previous] = tmp;
            }
            nextNode.previous = newId;
            _list[id] = nextNode;

            return newId;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _emptyIds.Clear();
            _list.Clear();
            _first = -1;
            _last = -1;
        }


        /// <summary>
        /// O(1)
        /// </summary>
        public void Remove(int id)
        {
            var node = _list[id];
            if (node.previous == node.next && _first != id)
            {
                throw new Exception("invalid id");
            }
            Remove(id, ref node);
        }


        /// <summary>
        /// O(1)
        /// </summary>
        public void RemoveFirst()
        {
            if (_first == -1)
            {
                throw new Exception("empty list");
            }
            var node = _list[_first];
            Remove(_first, ref node);
        }


        /// <summary>
        /// O(1)
        /// </summary>
        public void RemoveLast()
        {
            if (_last == -1)
            {
                throw new Exception("empty list");
            }
            var node = _list[_last];
            Remove(_last, ref node);
        }

    } // QuickLinkedList<T>

} // namespace UnityExtensions