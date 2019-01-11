
namespace UnityExtensions
{
    /// <summary>
    /// 键值对
    /// 因为 System.Collections.Generic.KeyValuePair 没有修改功能，因此添加了此实现
    /// </summary>
    public struct KeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;


        public KeyValuePair(TKey key)
        {
            this.key = key;
            value = default;
        }


        public KeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

    } // struct KeyValuePair

} // namespace UnityExtensions