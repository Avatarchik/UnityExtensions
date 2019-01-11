using System;

namespace UnityExtensions
{
    /// <summary>
    /// LinkedComponent
    /// 所有同类型组件对象被组织为链表，从链表中添加和移除的时间复杂度为 O(1)
    /// </summary>
    public class LinkedComponent<T> : ScriptableComponent where T : LinkedComponent<T>
    {
        T _this;

        public static T listFirst { get; private set; }
        public static T listLast { get; private set; }

        public T listPrevious { get; private set; }
        public T listNext { get; private set; }


        public static bool isListEmpty
        {
            get { return !listFirst; }
        }


        public LinkedComponent()
        {
            _this = this as T;
        }


        protected void AttachAsListLast()
        {
#if DEBUG
            if (listPrevious || listNext || listFirst == _this) throw new Exception("Node is already at list");
#endif

            listPrevious = listLast;
            if (listLast) listLast.listNext = _this;
            else listFirst = _this;
            listLast = _this;
        }


        protected void AttachAsListFirst()
        {
#if DEBUG
            if (listPrevious || listNext || listFirst == _this) throw new Exception("Node is already at list");
#endif

            listNext = listFirst;
            if (listFirst) listFirst.listPrevious = _this;
            else listLast = _this;
            listFirst = _this;
        }


        protected void AttachBefore(T target)
        {
#if DEBUG
            if (listPrevious || listNext || listFirst == _this) throw new Exception("Node is already at list");
            if (!target.listPrevious && !target.listNext && listFirst != target) throw new Exception("Target is not at list");
#endif

            listNext = target;
            listPrevious = target.listPrevious;
            target.listPrevious = _this;
            if (listFirst == target) listFirst = _this;
        }


        protected void AttachAfter(T target)
        {
#if DEBUG
            if (listPrevious || listNext || listFirst == _this) throw new Exception("Node is already at list");
            if (!target.listPrevious && !target.listNext && listFirst != target) throw new Exception("Target is not at list");
#endif

            listPrevious = target;
            listNext = target.listNext;
            target.listNext = _this;
            if (listLast == target) listLast = _this;
        }


        protected void DetachFromList()
        {
#if DEBUG
            if (!listPrevious && !listNext && listFirst != _this) throw new Exception("Node is already at list");
#endif

            if (listFirst == _this) listFirst = listNext;
            if (listLast == _this) listLast = listPrevious;
            if (listPrevious) listPrevious.listNext = listNext;
            if (listNext) listNext.listPrevious = listPrevious;
            listPrevious = null;
            listNext = null;
        }

    } // class LinkedComponent<T>

} // namespace UnityExtensions