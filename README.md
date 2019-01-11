# UnityExtension
Unity 通用扩展，包括编辑器、运行时的即用扩展、API 扩展。
注意，源代码使用了最新的 C# 特性，作者只能保证在 Unity 2018.3 及以上版本可以正常编译（注意切换到 .NET 4.x），如果你想在旧版本中使用，请自行修复编译问题。

- 即用扩展
   - Unity 菜单：Assets->Create->UnityExtension，你可能会用到 Layers 脚本自动生成功能。
   - Unity 菜单：Component->UnityExtension，你可能会用到游戏对象池（Game Object Pool）和 FPS 显示功能。
   - Unity 菜单：Window->UnityExtension，目前有一个强大的测量工具，设计者可能会喜欢——比如解谜游戏设计者 :-)
   
- API 扩展
   - 编辑器：位于 UnityExtension.Editor 命名空间。你可能会喜欢用于 IMGUI 编程的 Scopes 类型。还有一些实用工具，推荐你做个快速浏览，或许可以为你的开发节约不少时间。
   - 运行时（除特别说明外，以下内容都位于 UnityExtension 命名空间）
      - Attributes：位于 RuntimeAPI/Attributes 目录，有一些实用的 Attribute 帮助你快速定制编辑器。
      - 基类型：位于 RuntimeAPI/BaseClasses 目录，你一定会喜欢 ScriptableAssetSingleton，可以帮你方便地做项目配置文件并在脚本中访问。
      - 扩展方法：位于 RuntimeAPI/Extensions 目录，在你的代码中只要简单的添加 using UnityExtension 即可使用这些扩展方法。
      - 状态机：位于 RuntimeAPI/StateMachines 目录，包含一般状态机和专为 UI 设计的栈状态机，这是我能想到的最好用的状态机设计了。
      - 工具箱：位于 RuntimeAPI/Utilities 目录，对应脚本中的 Utilities 类。
      - 其他补充类型：位于 RuntimeAPI/Supplements 目录，你可能会用到快速链表、树或者可创建实例的 Random 类型。
      
关于一些 API 的使用范例位于 UnityExtension/Test 目录，如果你不清楚该怎么用的时候可以参考下。

![Stack State Machine](https://github.com/yuyang9119/UnityExtension/blob/master/Documents/SSM.gif)
栈状态机示例

欢迎提交 Bug 报告，共同修复、改进这个通用扩展库。我是独立游戏开发者，如果你想和我联系，你可以在这些 QQ 群里找到我：

    - Unity 游戏编程：333763528
    - Unity 图形技术：371834666
    - 独立游戏开发：123020960
