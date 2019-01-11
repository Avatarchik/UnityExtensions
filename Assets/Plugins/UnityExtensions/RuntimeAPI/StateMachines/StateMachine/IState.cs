
namespace UnityExtensions
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 进入状态时触发
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 离开状态时触发
        /// </summary>
        void OnExit();

        /// <summary>
        /// 更新状态时触发
        /// </summary>
        void OnUpdate(float deltaTime);

    } // interface IState

} // namespace UnityExtensions
