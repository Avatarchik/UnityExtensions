
namespace UnityExtension
{
    /// <summary>
    /// 状态栈的行为
    /// </summary>
    public enum StackAction
    {
        Push,
        Pop,
    }


    /// <summary>
    /// 栈状态接口
    /// </summary>
    public interface IStackState
    {
        /// <summary>
        /// 进入状态时触发
        /// </summary>
        void OnEnter(StackAction stackAction);

        /// <summary>
        /// 离开状态时触发
        /// </summary>
        void OnExit(StackAction stackAction);

        /// <summary>
        /// 更新状态时触发
        /// </summary>
        void OnUpdate(float deltaTime);

    } // interface IStackState

} // namespace UnityExtension
