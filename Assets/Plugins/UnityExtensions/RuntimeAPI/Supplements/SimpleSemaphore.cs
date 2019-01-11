using System.Threading;

namespace UnityExtensions
{
    /// <summary>
    /// 简单版本的 Semaphore，优点是有安全的 TryRelease 方法
    /// 目前已知在 Switch 平台 System.Semaphore.Release 无法正确抛出异常，因此添加了此简易版本
    /// https://stackoverflow.com/a/7334538
    /// </summary>
    public class SimpleSemaphore
    {
        int _count;
        int _maxCount;
        object _locker;


        public SimpleSemaphore(int initialCount, int maximumCount)
        {
            _count = initialCount;
            _maxCount = maximumCount;
            _locker = new object();
        }


        public void Wait()
        {
            lock (_locker)
            {
                while (_count == 0)
                {
                    Monitor.Wait(_locker);
                }
                _count--;
            }
        }


        public bool TryRelease()
        {
            lock (_locker)
            {
                if (_count < _maxCount)
                {
                    _count++;
                    Monitor.PulseAll(_locker);
                    return true;
                }
                return false;
            }
        }

    } // class SimpleSemaphore

} // UnityExtensions