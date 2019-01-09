using System;

namespace UnityExtension
{
    /// <summary>
    /// 树节点
    /// 在内部, 一个节点的子节点被组织为双向循环链表
    /// </summary>
    public class TreeNode<T>
    {
        TreeNode<T> _next;
        TreeNode<T> _prev;
        TreeNode<T> _parent;
        TreeNode<T> _firstChild;
        int _directChildCount;


        #region Enumerable & Enumerator

        public struct ChildrenEnumerable
        {
            TreeNode<T> _node;

            internal ChildrenEnumerable(TreeNode<T> node)
            {
                _node = node;
            }

            public ChildrenEnumerator GetEnumerator()
            {
                return new ChildrenEnumerator(_node);
            }
        }


        public struct ChildrenEnumerator
        {
            TreeNode<T> _node;
            TreeNode<T> _current;
            int _state;

            internal ChildrenEnumerator(TreeNode<T> node)
            {
                _node = node;
                _current = null;
                _state = 0;
            }

            public T Current
            {
                get { return _current.value; }
            }

            public bool MoveNext()
            {
                switch (_state)
                {
                    // root
                    case 0:
                        _state = 1;
                        _current = _node;
                        return true;

                    // first child
                    case 1:
                        if (_current._firstChild != null)
                        {
                           _current = _current._firstChild;
                            return true;
                        }
                        if (_current != _node)
                        {
                            _state = 2;
                            return MoveNext();
                        }
                        return false;
                            
                    // next child
                    case 2:
                        if (_current.next != null)
                        {
                            _state = 1;
                            _current = _current._next;
                            return true;
                        }
                        if (_current._parent != _node)
                        {
                            _current = _current._parent;
                            return MoveNext();
                        }
                        return false;
                }
                return false;
            }

            public void Reset()
            {
                _current = null;
                _state = 0;
            }
        }


        public struct ParentsEnumerable
        {
            TreeNode<T> _node;

            internal ParentsEnumerable(TreeNode<T> node)
            {
                _node = node;
            }

            public ParentsEnumerator GetEnumerator()
            {
                return new ParentsEnumerator(_node);
            }
        }


        public struct ParentsEnumerator
        {
            TreeNode<T> _node;
            TreeNode<T> _current;

            internal ParentsEnumerator(TreeNode<T> node)
            {
                _node = node;
                _current = null;
            }

            public T Current
            {
                get { return _current.value; }
            }

            public bool MoveNext()
            {
                if (_current == null)
                {
                    _current = _node;
                    return true;
                }

                _current = _current._parent;
                return _current != null;
            }

            public void Reset()
            {
                _current = null;
            }
        }


        public struct DirectChildrenEnumerable
        {
            TreeNode<T> _node;

            internal DirectChildrenEnumerable(TreeNode<T> node)
            {
                _node = node;
            }

            public DirectChildrenEnumerator GetEnumerator()
            {
                return new DirectChildrenEnumerator(_node);
            }
        }


        public struct DirectChildrenEnumerator
        {
            TreeNode<T> _node;
            TreeNode<T> _current;

            internal DirectChildrenEnumerator(TreeNode<T> node)
            {
                _node = node;
                _current = null;
            }

            public T Current
            {
                get { return _current.value; }
            }

            public bool MoveNext()
            {
                if (_current == null) _current = _node._firstChild;
                else  _current = _current.next;

                return _current != null;
            }

            public void Reset()
            {
                _current = null;
            }
        }

        #endregion


        /// <summary>
        /// 节点包含的数据
        /// </summary>
        public T value;


        /// <summary>
        /// 构造一个包含默认数据的节点
        /// </summary>
        public TreeNode()
        {
            _next = this;
            _prev = this;
        }


        /// <summary>
        /// 构造一个初始化数据的节点
        /// </summary>
        public TreeNode(T value) : this()
        {
            this.value = value;
        }


        /// <summary>
        /// 同层级中后一个节点. 如果此节点是最后一个则返回 null
        /// </summary>
        public TreeNode<T> next
        {
            get
            {
                if (_parent == null || _next == _parent._firstChild)
                {
                    return null;
                }
                return _next;
            }
        }


        /// <summary>
        /// 同层级中后一个循环节点. 该返回值永远不为 null
        /// </summary>
        public TreeNode<T> circularNext
        {
            get { return _next; }
        }


        /// <summary>
        /// 同层级中前一个节点. 如果此节点是第一个则返回 null
        /// </summary>
        public TreeNode<T> previous
        {
            get
            {
                if (_parent == null || this == _parent._firstChild)
                {
                    return null;
                }
                return _prev;
            }
        }


        /// <summary>
        /// 同层级中前一个循环节点. 该返回值永远不为 null
        /// </summary>
        public TreeNode<T> circularPrevious
        {
            get { return _prev; }
        }


        /// <summary>
        /// 第一个子节点. 如果没有子节点返回 null
        /// </summary>
        public TreeNode<T> firstChild
        {
            get { return _firstChild; }
        }


        /// <summary>
        /// 最后一个子节点. 如果没有子节点返回 null
        /// </summary>
        public TreeNode<T> lastChild
        {
            get
            {
                return _firstChild == null ? null : _firstChild._prev;
            }
        }


        /// <summary>
        /// 父节点. 如果没有父节点返回 null
        /// </summary>
        public TreeNode<T> parent
        {
            get { return _parent; }
        }


        /// <summary>
        /// 直接子节点总数
        /// </summary>
        public int directChildCount
        {
            get { return _directChildCount; }
        }


        /// <summary>
        /// 深度. 一个根节点的深度是 0
        /// 此属性运算复杂度为 O(d), d 为节点深度
        /// </summary>
        public int depth
        {
            get
            {
                int value = 0;
                var node = _parent;

                while (node != null)
                {
                    value++;
                    node = node._parent;
                }

                return value;
            }
        }


        /// <summary>
        /// 根节点
        /// 此属性运算复杂度为 O(d), d 为节点深度
        /// </summary>
        public TreeNode<T> root
        {
            get
            {
                var node = this;

                while (node._parent != null)
                {
                    node = node._parent;
                }

                return node;
            }
        }


        /// <summary>
        /// 是否为根节点
        /// </summary>
        public bool isRoot
        {
            get { return _parent == null; }
        }


        /// <summary>
        /// 是否为叶子节点
        /// </summary>
        public bool isLeaf
        {
            get { return _firstChild == null; }
        }


        /// <summary>
        /// 获取一个用以 foreach 所有子节点的 Enumerable 对象 (包括自身)
        /// </summary>
        public ChildrenEnumerable children
        {
            get { return new ChildrenEnumerable(this); }
        }


        /// <summary>
        /// 获取一个用以 foreach 所有父节点的 Enumerable 对象 (包括自身)
        /// </summary>
        public ParentsEnumerable parents
        {
            get { return new ParentsEnumerable(this); }
        }


        /// <summary>
        /// 获取一个用以 foreach 所有直接子节点的 Enumerable 对象
        /// </summary>
        public DirectChildrenEnumerable directChildren
        {
            get { return new DirectChildrenEnumerable(this); }
        }


        /// <summary>
        /// 作为第一个子节点附着到一个父节点下
        /// 注意: 如果 parent 存在于当前节点为根的子树中, 那么此操作的结果是未定义的
        /// </summary>
        public void AttachAsFirst(TreeNode<T> parent)
        {
            InternalValidateAttaching(parent);

            if (parent._firstChild == null)
            {
                InternalAttachChildless(parent);
            }
            else
            {
                InternalAttachBefore(parent, parent._firstChild);
                parent._firstChild = this;
            }
        }


        /// <summary>
        /// 作为最后一个子节点附着到一个父节点下
        /// 注意: 如果 parent 存在于当前节点为根的子树中, 那么此操作的结果是未定义的
        /// </summary>
        public void AttachAsLast(TreeNode<T> parent)
        {
            InternalValidateAttaching(parent);

            if (parent._firstChild == null)
            {
                InternalAttachChildless(parent);
            }
            else
            {
                InternalAttachBefore(parent, parent._firstChild);
            }
        }


        /// <summary>
        /// 附着到一个父节点下的某个子节点之前, 如果该子节点是第一个子节点, 那么父节点的 firstChild 也会改变
        /// 注意: 如果 parent 存在于当前节点为根的子树中, 那么此操作的结果是未定义的
        /// </summary>
        public void AttachBefore(TreeNode<T> parent, TreeNode<T> next)
        {
            InternalValidateAttaching(parent);
            parent.InternalValidateChild(next);
            InternalAttachBefore(parent, next);

            if (parent._firstChild == next)
            {
                parent._firstChild = this;
            }
        }


        /// <summary>
        /// 附着到一个父节点下的某个子节点之后
        /// 注意: 如果 parent 存在于当前节点为根的子树中, 那么此操作的结果是未定义的
        /// </summary>
        public void AttachAfter(TreeNode<T> parent, TreeNode<T> previous)
        {
            InternalValidateAttaching(parent);
            parent.InternalValidateChild(previous);
            InternalAttachBefore(parent, previous._next);
        }


        /// <summary>
        /// 从父节点脱离
        /// </summary>
        public void DetachParent()
        {
            if (_parent != null)
            {
                if (_parent._firstChild == this)
                {
                    _parent._firstChild = _next == this ? null : _next;
                }
                _parent._directChildCount--;

                _next._prev = _prev;
                _prev._next = _next;

                _parent = null;
                _next = this;
                _prev = this;
            }
        }


        /// <summary>
        /// 分离所有直接子节点
        /// </summary>
        public void DetachChildren()
        {
            TreeNode<T> child;

            while (_directChildCount > 0)
            {
                child = _firstChild;
                _firstChild = child._next;

                child._parent = null;
                child._next = child;
                child._prev = child;

                _directChildCount--;
            }

            _firstChild = null;
        }


        /// <summary>
        /// 是否存在于某个节点为根的子树中. parent 为 null 时会返回 false
        /// </summary>
        public bool IsChildOf(TreeNode<T> parent)
        {
            var node = this;

            do
            {
                if (node == parent) return true;
                node = node._parent;
            }
            while (node != null);

            return false;
        }


        /// <summary>
        /// 遍历子树 (包含自己).
        /// 用于在遍历过程中修改节点的 value, 否则推荐使用 foreach children
        /// 注意: 在遍历过程中修改树的结构是未定义的行为
        /// </summary>
        public void TraverseChildren(Action<TreeNode<T>> action)
        {
            action(this);

            if (_firstChild != null)
            {
                var node = _firstChild;
                do
                {
                    node.TraverseChildren(action);
                    node = node._next;
                }
                while (node != _firstChild);
            }
        }


        /// <summary>
        /// 遍历所有父级节点 (包含自己)
        /// 用于在遍历过程中修改节点的 value, 否则推荐使用 foreach parents
        /// 注意: 在遍历过程中修改树的结构是未定义的行为
        /// </summary>
        public void TraverseParents(Action<TreeNode<T>> action)
        {
            var node = this;

            do
            {
                action(node);
                node = node._parent;
            }
            while (node != null);
        }


        /// <summary>
        /// 遍历所有直接子节点
        /// 用于在遍历过程中修改节点的 value, 否则推荐使用 foreach directChildren
        /// 注意: 在遍历过程中修改树的结构是未定义的行为
        /// </summary>
        public void TraverseDirectChildren(Action<TreeNode<T>> action)
        {
            var node = _firstChild;
            while (node != null)
            {
                action(node);
                node = node.next;
            }
        }


        /// <summary>
        /// 在子树中查找 (包含自己). 查找失败返回 null
        /// 注意: 在匹配方法中修改树的结构是未定义的行为
        /// </summary>
        public TreeNode<T> FindChild(Predicate<T> match)
        {
            if (match(value)) return this;

            if (_firstChild != null)
            {
                var node = _firstChild;
                TreeNode<T> result;

                do
                {
                    result = node.FindChild(match);
                    if (result != null) return result;
                    node = node._next;
                }
                while (node != _firstChild);
            }

            return null;
        }


        /// <summary>
        /// 在所有父级节点中查找 (包含自己). 查找失败返回 null
        /// 注意: 在匹配方法中修改树的结构是未定义的行为
        /// </summary>
        public TreeNode<T> FindParent(Predicate<T> match)
        {
            var node = this;

            do
            {
                if (match(node.value)) return node;
                node = node._parent;
            }
            while (node != null);

            return null;
        }


        /// <summary>
        /// 在所有直接子节点中查找. 查找失败返回 null
        /// 注意: 在匹配方法中修改树的结构是未定义的行为
        /// </summary>
        public TreeNode<T> FindDirectChildren(Predicate<T> match)
        {
            var node = _firstChild;
            while (node != null)
            {
                if (match(node.value)) return node;
                node = node.next;
            }
            return null;
        }


        #region Internal

        // 验证 Attach 操作是否合法
        void InternalValidateAttaching(TreeNode<T> parent)
        {
            if (_parent != null)
            {
                throw new InvalidOperationException("node is attached");
            }
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            //if (parent.IsChildOf(this))
            //{
            //    throw new InvalidOperationException("new parent is child of node");
            //}
        }


        // 验证一个节点是否为 parent 的子节点
        void InternalValidateChild(TreeNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (node._parent != this)
            {
                throw new InvalidOperationException("node is not child of parent");
            }
        }


        // 附着到一个没有子节点的节点下
        void InternalAttachChildless(TreeNode<T> parent)
        {
            _parent = parent;
            parent._directChildCount++;
            parent._firstChild = this;
        }


        // 附着到一个父节点下的某个子节点之前
        void InternalAttachBefore(TreeNode<T> parent, TreeNode<T> next)
        {
            _parent = parent;
            _next = next;
            _prev = next._prev;

            parent._directChildCount++;

            next._prev._next = this;
            next._prev = this;
        }

        #endregion

    } // class TreeNode<T>

} // namespace UnityExtension