using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HandyControl.Tools;

namespace HandyControlDemo.Properties.Langs
{
    public class LangProvider : INotifyPropertyChanged
    {
        internal static LangProvider Instance => ResourceHelper.GetResource<LangProvider>("DemoLangs");

        private static string CultureInfoStr;

        public static CultureInfo Culture
        {
            get => Lang.Culture;
            set
            {
                if (value == null) return;
                if (Equals(CultureInfoStr, value.EnglishName)) return;
                Lang.Culture = value;
                CultureInfoStr = value.EnglishName;

                Instance.UpdateLangs();
            }
        }

        public static string GetLang(string key) => Lang.ResourceManager.GetString(key, Culture);

        public static void SetLang(DependencyObject dependencyObject, DependencyProperty dependencyProperty, string key) =>
            BindingOperations.SetBinding(dependencyObject, dependencyProperty, new Binding(key)
            {
                Source = Instance,
                Mode = BindingMode.OneWay
            });

		private void UpdateLangs()
        {
			OnPropertyChanged(nameof(About));
			OnPropertyChanged(nameof(AddItem));
			OnPropertyChanged(nameof(AnimationPath));
			OnPropertyChanged(nameof(AppClosingTip));
			OnPropertyChanged(nameof(Ask));
			OnPropertyChanged(nameof(Badge));
			OnPropertyChanged(nameof(BasicInfo));
			OnPropertyChanged(nameof(BasicLayout));
			OnPropertyChanged(nameof(Blink));
			OnPropertyChanged(nameof(Blog));
			OnPropertyChanged(nameof(Border));
			OnPropertyChanged(nameof(Brush));
			OnPropertyChanged(nameof(Button));
			OnPropertyChanged(nameof(ButtonCustom));
			OnPropertyChanged(nameof(ButtonGroup));
			OnPropertyChanged(nameof(Calendar));
			OnPropertyChanged(nameof(CalendarWithClock));
			OnPropertyChanged(nameof(Card));
			OnPropertyChanged(nameof(Carousel));
			OnPropertyChanged(nameof(ChangeLangAsk));
			OnPropertyChanged(nameof(ChatBubble));
			OnPropertyChanged(nameof(Chatroom));
			OnPropertyChanged(nameof(CheckBox));
			OnPropertyChanged(nameof(CirclePanel));
			OnPropertyChanged(nameof(Clear));
			OnPropertyChanged(nameof(Click2Count));
			OnPropertyChanged(nameof(Clock));
			OnPropertyChanged(nameof(ColorPicker));
			OnPropertyChanged(nameof(ColumnOffset));
			OnPropertyChanged(nameof(ColumnSpacing));
			OnPropertyChanged(nameof(ComboBox));
			OnPropertyChanged(nameof(ComingSoon));
			OnPropertyChanged(nameof(Comment));
			OnPropertyChanged(nameof(Common));
			OnPropertyChanged(nameof(CompareSlider));
			OnPropertyChanged(nameof(Complete));
			OnPropertyChanged(nameof(ContentDemoStr));
			OnPropertyChanged(nameof(Contributors));
			OnPropertyChanged(nameof(Controls));
			OnPropertyChanged(nameof(CoverFlow));
			OnPropertyChanged(nameof(CoverView));
			OnPropertyChanged(nameof(Danger));
			OnPropertyChanged(nameof(DataGrid));
			OnPropertyChanged(nameof(DatePicker));
			OnPropertyChanged(nameof(DateTimePicker));
			OnPropertyChanged(nameof(Default));
			OnPropertyChanged(nameof(Demo));
			OnPropertyChanged(nameof(Dialog));
			OnPropertyChanged(nameof(DialogDemo));
			OnPropertyChanged(nameof(Divider));
			OnPropertyChanged(nameof(Doc_cn));
			OnPropertyChanged(nameof(Doc_en));
			OnPropertyChanged(nameof(Documentation));
			OnPropertyChanged(nameof(DragHere));
			OnPropertyChanged(nameof(Drawer));
			OnPropertyChanged(nameof(Effects));
			OnPropertyChanged(nameof(Email));
			OnPropertyChanged(nameof(Error));
			OnPropertyChanged(nameof(Exit));
			OnPropertyChanged(nameof(Expander));
			OnPropertyChanged(nameof(Fatal));
			OnPropertyChanged(nameof(FlipClock));
			OnPropertyChanged(nameof(FloatingBlock));
			OnPropertyChanged(nameof(FlowDocument));
			OnPropertyChanged(nameof(FlowDocumentPageViewer));
			OnPropertyChanged(nameof(FlowDocumentReader));
			OnPropertyChanged(nameof(FlowDocumentScrollViewer));
			OnPropertyChanged(nameof(Frame));
			OnPropertyChanged(nameof(GifImage));
			OnPropertyChanged(nameof(GotoTop));
			OnPropertyChanged(nameof(Gravatar));
			OnPropertyChanged(nameof(Grid));
			OnPropertyChanged(nameof(GroupBox));
			OnPropertyChanged(nameof(Groups));
			OnPropertyChanged(nameof(Growl));
			OnPropertyChanged(nameof(GrowlAsk));
			OnPropertyChanged(nameof(GrowlDemo));
			OnPropertyChanged(nameof(GrowlError));
			OnPropertyChanged(nameof(GrowlFatal));
			OnPropertyChanged(nameof(GrowlInfo));
			OnPropertyChanged(nameof(GrowlSuccess));
			OnPropertyChanged(nameof(GrowlWarning));
			OnPropertyChanged(nameof(HatchBrushGenerator));
			OnPropertyChanged(nameof(HoneycombPanel));
			OnPropertyChanged(nameof(HybridLayout));
			OnPropertyChanged(nameof(Ignore));
			OnPropertyChanged(nameof(ImageBlock));
			OnPropertyChanged(nameof(ImageBrowser));
            OnPropertyChanged(nameof(ImageSelector));
            OnPropertyChanged(nameof(Index));
			OnPropertyChanged(nameof(Info));
			OnPropertyChanged(nameof(InteractiveDialog));
			OnPropertyChanged(nameof(IsNotPhone));
			OnPropertyChanged(nameof(Label));
			OnPropertyChanged(nameof(LangComment));
			OnPropertyChanged(nameof(ListBox));
			OnPropertyChanged(nameof(ListView));
			OnPropertyChanged(nameof(Loading));
			OnPropertyChanged(nameof(Magnifier));
			OnPropertyChanged(nameof(Menu));
			OnPropertyChanged(nameof(MessageBox));
			OnPropertyChanged(nameof(Morphing_Animation));
			OnPropertyChanged(nameof(Name));
			OnPropertyChanged(nameof(NewWindow));
			OnPropertyChanged(nameof(Next));
			OnPropertyChanged(nameof(Notification));
			OnPropertyChanged(nameof(NotifyIcon));
			OnPropertyChanged(nameof(NumericUpDown));
			OnPropertyChanged(nameof(Off));
			OnPropertyChanged(nameof(Ok));
			OnPropertyChanged(nameof(On));
			OnPropertyChanged(nameof(OpenBlurWindow));
			OnPropertyChanged(nameof(OpenCommonWindow));
			OnPropertyChanged(nameof(OpenCustomContentWindow));
			OnPropertyChanged(nameof(OpenCustomMessageWindow));
			OnPropertyChanged(nameof(OpenCustomNonClientAreaWindow));
			OnPropertyChanged(nameof(OpenGlowWindow));
			OnPropertyChanged(nameof(OpenImageBrowser));
			OnPropertyChanged(nameof(OpenMessageWindow));
			OnPropertyChanged(nameof(OpenMouseFollowWindow));
			OnPropertyChanged(nameof(OpenNativeCommonWindow));
			OnPropertyChanged(nameof(OpenNavigationWindow));
			OnPropertyChanged(nameof(OpenNoNonClientAreaDragableWindow));
			OnPropertyChanged(nameof(OpenPanel));
			OnPropertyChanged(nameof(OpenSprite));
			OnPropertyChanged(nameof(OutlineText));
			OnPropertyChanged(nameof(Pagination));
			OnPropertyChanged(nameof(PasswordBox));
			OnPropertyChanged(nameof(PinBox));
			OnPropertyChanged(nameof(PleaseInput));
			OnPropertyChanged(nameof(PleaseWait));
			OnPropertyChanged(nameof(PlsEnterContent));
			OnPropertyChanged(nameof(PlsEnterEmail));
			OnPropertyChanged(nameof(PlsEnterKey));
			OnPropertyChanged(nameof(Poptip));
			OnPropertyChanged(nameof(PoptipPositionStr));
			OnPropertyChanged(nameof(PracticalDemos));
			OnPropertyChanged(nameof(Prev));
			OnPropertyChanged(nameof(PreviewSlider));
			OnPropertyChanged(nameof(Primary));
			OnPropertyChanged(nameof(ProgressBar));
			OnPropertyChanged(nameof(ProgressButton));
			OnPropertyChanged(nameof(Project));
            OnPropertyChanged(nameof(PropertyGrid));
            OnPropertyChanged(nameof(PushToTalk));
			OnPropertyChanged(nameof(QQGroup));
			OnPropertyChanged(nameof(RadioButton));
			OnPropertyChanged(nameof(RangeSlider));
			OnPropertyChanged(nameof(Rate));
			OnPropertyChanged(nameof(Recommendation));
			OnPropertyChanged(nameof(Register));
			OnPropertyChanged(nameof(RelativePanel));
			OnPropertyChanged(nameof(Remark));
			OnPropertyChanged(nameof(RemoveItem));
			OnPropertyChanged(nameof(RepeatButton));
			OnPropertyChanged(nameof(Reply));
			OnPropertyChanged(nameof(Repository));
			OnPropertyChanged(nameof(ResponsiveLayout));
			OnPropertyChanged(nameof(RichTextBox));
			OnPropertyChanged(nameof(RightClickHere));
			OnPropertyChanged(nameof(RunningBlock));
			OnPropertyChanged(nameof(Screenshot));
			OnPropertyChanged(nameof(ScrollViewer));
			OnPropertyChanged(nameof(SearchBar));
			OnPropertyChanged(nameof(Second));
			OnPropertyChanged(nameof(Selected));
			OnPropertyChanged(nameof(SendNotification));
			OnPropertyChanged(nameof(Shield));
			OnPropertyChanged(nameof(ShowInCurrentWindow));
			OnPropertyChanged(nameof(ShowInMainWindow));
			OnPropertyChanged(nameof(ShowRowNumber));
			OnPropertyChanged(nameof(SideMenu));
			OnPropertyChanged(nameof(Slider));
			OnPropertyChanged(nameof(SplitButton));
			OnPropertyChanged(nameof(Sprite));
			OnPropertyChanged(nameof(StartScreenshot));
			OnPropertyChanged(nameof(StaysOpen));
			OnPropertyChanged(nameof(Step));
			OnPropertyChanged(nameof(StepBar));
			OnPropertyChanged(nameof(Styles));
			OnPropertyChanged(nameof(SubTitle));
			OnPropertyChanged(nameof(Success));
			OnPropertyChanged(nameof(TabControl));
			OnPropertyChanged(nameof(Tag));
			OnPropertyChanged(nameof(Text));
			OnPropertyChanged(nameof(TextBlock));
			OnPropertyChanged(nameof(TextBox));
			OnPropertyChanged(nameof(TextDialog));
			OnPropertyChanged(nameof(TextDialogInControl));
			OnPropertyChanged(nameof(TextDialogWithTimer));
			OnPropertyChanged(nameof(TimeBar));
			OnPropertyChanged(nameof(TimePicker));
			OnPropertyChanged(nameof(Tip));
			OnPropertyChanged(nameof(Title));
			OnPropertyChanged(nameof(TitleDemoStr1));
			OnPropertyChanged(nameof(TitleDemoStr2));
			OnPropertyChanged(nameof(TitleDemoStr3));
			OnPropertyChanged(nameof(ToggleButton));
			OnPropertyChanged(nameof(ToolBar));
			OnPropertyChanged(nameof(Tools));
			OnPropertyChanged(nameof(Transfer));
			OnPropertyChanged(nameof(TransitioningContentControl));
			OnPropertyChanged(nameof(TreeView));
			OnPropertyChanged(nameof(Try2CloseApp));
			OnPropertyChanged(nameof(Type));
			OnPropertyChanged(nameof(UploadFile));
			OnPropertyChanged(nameof(UploadStr));
			OnPropertyChanged(nameof(Visibility));
			OnPropertyChanged(nameof(Vsix));
			OnPropertyChanged(nameof(Warning));
			OnPropertyChanged(nameof(WaterfallPanel));
			OnPropertyChanged(nameof(Website));
			OnPropertyChanged(nameof(Window));
        }

        /// <summary>
        ///   查找类似 关于 的本地化字符串。
        /// </summary>
		public string About => Lang.About;

        /// <summary>
        ///   查找类似 添加项 的本地化字符串。
        /// </summary>
		public string AddItem => Lang.AddItem;

        /// <summary>
        ///   查找类似 动画路径 的本地化字符串。
        /// </summary>
		public string AnimationPath => Lang.AnimationPath;

        /// <summary>
        ///   查找类似 托盘图标已打开，将隐藏窗口而不是关闭程序 的本地化字符串。
        /// </summary>
		public string AppClosingTip => Lang.AppClosingTip;

        /// <summary>
        ///   查找类似 询问 的本地化字符串。
        /// </summary>
		public string Ask => Lang.Ask;

        /// <summary>
        ///   查找类似 标记 的本地化字符串。
        /// </summary>
		public string Badge => Lang.Badge;

        /// <summary>
        ///   查找类似 填写基本信息 的本地化字符串。
        /// </summary>
		public string BasicInfo => Lang.BasicInfo;

        /// <summary>
        ///   查找类似 基础布局 的本地化字符串。
        /// </summary>
		public string BasicLayout => Lang.BasicLayout;

        /// <summary>
        ///   查找类似 闪烁 的本地化字符串。
        /// </summary>
		public string Blink => Lang.Blink;

        /// <summary>
        ///   查找类似 博客 的本地化字符串。
        /// </summary>
		public string Blog => Lang.Blog;

        /// <summary>
        ///   查找类似 边框 的本地化字符串。
        /// </summary>
		public string Border => Lang.Border;

        /// <summary>
        ///   查找类似 画刷 的本地化字符串。
        /// </summary>
		public string Brush => Lang.Brush;

        /// <summary>
        ///   查找类似 按钮 的本地化字符串。
        /// </summary>
		public string Button => Lang.Button;

        /// <summary>
        ///   查找类似 自定义按钮 的本地化字符串。
        /// </summary>
		public string ButtonCustom => Lang.ButtonCustom;

        /// <summary>
        ///   查找类似 按钮组 的本地化字符串。
        /// </summary>
		public string ButtonGroup => Lang.ButtonGroup;

        /// <summary>
        ///   查找类似 日历 的本地化字符串。
        /// </summary>
		public string Calendar => Lang.Calendar;

        /// <summary>
        ///   查找类似 带时钟的日历 的本地化字符串。
        /// </summary>
		public string CalendarWithClock => Lang.CalendarWithClock;

        /// <summary>
        ///   查找类似 卡片 的本地化字符串。
        /// </summary>
		public string Card => Lang.Card;

        /// <summary>
        ///   查找类似 轮播 的本地化字符串。
        /// </summary>
		public string Carousel => Lang.Carousel;

        /// <summary>
        ///   查找类似 是否重启以更改语言？ 的本地化字符串。
        /// </summary>
		public string ChangeLangAsk => Lang.ChangeLangAsk;

        /// <summary>
        ///   查找类似 对话气泡 的本地化字符串。
        /// </summary>
		public string ChatBubble => Lang.ChatBubble;

        /// <summary>
        ///   查找类似 讨论室 的本地化字符串。
        /// </summary>
		public string Chatroom => Lang.Chatroom;

        /// <summary>
        ///   查找类似 复选框 的本地化字符串。
        /// </summary>
		public string CheckBox => Lang.CheckBox;

        /// <summary>
        ///   查找类似 圆形布局 的本地化字符串。
        /// </summary>
		public string CirclePanel => Lang.CirclePanel;

        /// <summary>
        ///   查找类似 清空 的本地化字符串。
        /// </summary>
		public string Clear => Lang.Clear;

        /// <summary>
        ///   查找类似 点击计数 的本地化字符串。
        /// </summary>
		public string Click2Count => Lang.Click2Count;

        /// <summary>
        ///   查找类似 时钟 的本地化字符串。
        /// </summary>
		public string Clock => Lang.Clock;

        /// <summary>
        ///   查找类似 颜色拾取器 的本地化字符串。
        /// </summary>
		public string ColorPicker => Lang.ColorPicker;

        /// <summary>
        ///   查找类似 分栏偏移 的本地化字符串。
        /// </summary>
		public string ColumnOffset => Lang.ColumnOffset;

        /// <summary>
        ///   查找类似 分栏间隔 的本地化字符串。
        /// </summary>
		public string ColumnSpacing => Lang.ColumnSpacing;

        /// <summary>
        ///   查找类似 组合框 的本地化字符串。
        /// </summary>
		public string ComboBox => Lang.ComboBox;

        /// <summary>
        ///   查找类似 敬请期待 的本地化字符串。
        /// </summary>
		public string ComingSoon => Lang.ComingSoon;

        /// <summary>
        ///   查找类似 评论 的本地化字符串。
        /// </summary>
		public string Comment => Lang.Comment;

        /// <summary>
        ///   查找类似 一般 的本地化字符串。
        /// </summary>
		public string Common => Lang.Common;

        /// <summary>
        ///   查找类似 对比滑块 的本地化字符串。
        /// </summary>
		public string CompareSlider => Lang.CompareSlider;

        /// <summary>
        ///   查找类似 完成 的本地化字符串。
        /// </summary>
		public string Complete => Lang.Complete;

        /// <summary>
        ///   查找类似 这是内容 的本地化字符串。
        /// </summary>
		public string ContentDemoStr => Lang.ContentDemoStr;

        /// <summary>
        ///   查找类似 贡献者 的本地化字符串。
        /// </summary>
		public string Contributors => Lang.Contributors;

        /// <summary>
        ///   查找类似 控件 的本地化字符串。
        /// </summary>
		public string Controls => Lang.Controls;

        /// <summary>
        ///   查找类似 封面流 的本地化字符串。
        /// </summary>
		public string CoverFlow => Lang.CoverFlow;

        /// <summary>
        ///   查找类似 封面视图 的本地化字符串。
        /// </summary>
		public string CoverView => Lang.CoverView;

        /// <summary>
        ///   查找类似 危险 的本地化字符串。
        /// </summary>
		public string Danger => Lang.Danger;

        /// <summary>
        ///   查找类似 数据表格 的本地化字符串。
        /// </summary>
		public string DataGrid => Lang.DataGrid;

        /// <summary>
        ///   查找类似 日期选择器 的本地化字符串。
        /// </summary>
		public string DatePicker => Lang.DatePicker;

        /// <summary>
        ///   查找类似 日期时间选择器 的本地化字符串。
        /// </summary>
		public string DateTimePicker => Lang.DateTimePicker;

        /// <summary>
        ///   查找类似 默认 的本地化字符串。
        /// </summary>
		public string Default => Lang.Default;

        /// <summary>
        ///   查找类似 示例 的本地化字符串。
        /// </summary>
		public string Demo => Lang.Demo;

        /// <summary>
        ///   查找类似 对话框 的本地化字符串。
        /// </summary>
		public string Dialog => Lang.Dialog;

        /// <summary>
        ///   查找类似 对话框示例 的本地化字符串。
        /// </summary>
		public string DialogDemo => Lang.DialogDemo;

        /// <summary>
        ///   查找类似 分割线 的本地化字符串。
        /// </summary>
		public string Divider => Lang.Divider;

        /// <summary>
        ///   查找类似 中文文档 的本地化字符串。
        /// </summary>
		public string Doc_cn => Lang.Doc_cn;

        /// <summary>
        ///   查找类似 英文文档 的本地化字符串。
        /// </summary>
		public string Doc_en => Lang.Doc_en;

        /// <summary>
        ///   查找类似 文献资料 的本地化字符串。
        /// </summary>
		public string Documentation => Lang.Documentation;

        /// <summary>
        ///   查找类似 在这里拖拽 的本地化字符串。
        /// </summary>
		public string DragHere => Lang.DragHere;

        /// <summary>
        ///   查找类似 抽屉 的本地化字符串。
        /// </summary>
		public string Drawer => Lang.Drawer;

        /// <summary>
        ///   查找类似 效果 的本地化字符串。
        /// </summary>
		public string Effects => Lang.Effects;

        /// <summary>
        ///   查找类似 邮箱 的本地化字符串。
        /// </summary>
		public string Email => Lang.Email;

        /// <summary>
        ///   查找类似 错误 的本地化字符串。
        /// </summary>
		public string Error => Lang.Error;

        /// <summary>
        ///   查找类似 退出 的本地化字符串。
        /// </summary>
		public string Exit => Lang.Exit;

        /// <summary>
        ///   查找类似 展开框 的本地化字符串。
        /// </summary>
		public string Expander => Lang.Expander;

        /// <summary>
        ///   查找类似 严重 的本地化字符串。
        /// </summary>
		public string Fatal => Lang.Fatal;

        /// <summary>
        ///   查找类似 翻页时钟 的本地化字符串。
        /// </summary>
		public string FlipClock => Lang.FlipClock;

        /// <summary>
        ///   查找类似 漂浮块 的本地化字符串。
        /// </summary>
		public string FloatingBlock => Lang.FloatingBlock;

        /// <summary>
        ///   查找类似 流文档 的本地化字符串。
        /// </summary>
		public string FlowDocument => Lang.FlowDocument;

        /// <summary>
        ///   查找类似 流文档单页视图 的本地化字符串。
        /// </summary>
		public string FlowDocumentPageViewer => Lang.FlowDocumentPageViewer;

        /// <summary>
        ///   查找类似 流文档查看器 的本地化字符串。
        /// </summary>
		public string FlowDocumentReader => Lang.FlowDocumentReader;

        /// <summary>
        ///   查找类似 流文档滚动视图 的本地化字符串。
        /// </summary>
		public string FlowDocumentScrollViewer => Lang.FlowDocumentScrollViewer;

        /// <summary>
        ///   查找类似 导航框架 的本地化字符串。
        /// </summary>
		public string Frame => Lang.Frame;

        /// <summary>
        ///   查找类似 Gif图片 的本地化字符串。
        /// </summary>
		public string GifImage => Lang.GifImage;

        /// <summary>
        ///   查找类似 回到顶部 的本地化字符串。
        /// </summary>
		public string GotoTop => Lang.GotoTop;

        /// <summary>
        ///   查找类似 头像 的本地化字符串。
        /// </summary>
		public string Gravatar => Lang.Gravatar;

        /// <summary>
        ///   查找类似 栅格 的本地化字符串。
        /// </summary>
		public string Grid => Lang.Grid;

        /// <summary>
        ///   查找类似 分组框 的本地化字符串。
        /// </summary>
		public string GroupBox => Lang.GroupBox;

        /// <summary>
        ///   查找类似 组数 的本地化字符串。
        /// </summary>
		public string Groups => Lang.Groups;

        /// <summary>
        ///   查找类似 信息通知 的本地化字符串。
        /// </summary>
		public string Growl => Lang.Growl;

        /// <summary>
        ///   查找类似 检测到有新版本！是否更新？ 的本地化字符串。
        /// </summary>
		public string GrowlAsk => Lang.GrowlAsk;

        /// <summary>
        ///   查找类似 消息通知示例 的本地化字符串。
        /// </summary>
		public string GrowlDemo => Lang.GrowlDemo;

        /// <summary>
        ///   查找类似 连接失败，请检查网络！ 的本地化字符串。
        /// </summary>
		public string GrowlError => Lang.GrowlError;

        /// <summary>
        ///   查找类似 程序已崩溃~~~ 的本地化字符串。
        /// </summary>
		public string GrowlFatal => Lang.GrowlFatal;

        /// <summary>
        ///   查找类似 今天的天气不错~~~ 的本地化字符串。
        /// </summary>
		public string GrowlInfo => Lang.GrowlInfo;

        /// <summary>
        ///   查找类似 文件保存成功！ 的本地化字符串。
        /// </summary>
		public string GrowlSuccess => Lang.GrowlSuccess;

        /// <summary>
        ///   查找类似 磁盘空间快要满了！ 的本地化字符串。
        /// </summary>
		public string GrowlWarning => Lang.GrowlWarning;

        /// <summary>
        ///   查找类似 阴影画笔生成器 的本地化字符串。
        /// </summary>
		public string HatchBrushGenerator => Lang.HatchBrushGenerator;

        /// <summary>
        ///   查找类似 蜂窝布局 的本地化字符串。
        /// </summary>
		public string HoneycombPanel => Lang.HoneycombPanel;

        /// <summary>
        ///   查找类似 混合布局 的本地化字符串。
        /// </summary>
		public string HybridLayout => Lang.HybridLayout;

        /// <summary>
        ///   查找类似 忽略 的本地化字符串。
        /// </summary>
		public string Ignore => Lang.Ignore;

        /// <summary>
        ///   查找类似 图片块 的本地化字符串。
        /// </summary>
		public string ImageBlock => Lang.ImageBlock;

        /// <summary>
        ///   查找类似 图片浏览器 的本地化字符串。
        /// </summary>
		public string ImageBrowser => Lang.ImageBrowser;

        /// <summary>
        ///   查找类似 图片选择器 的本地化字符串。
        /// </summary>
        public string ImageSelector => Lang.ImageSelector;

        /// <summary>
        ///   查找类似 索引 的本地化字符串。
        /// </summary>
		public string Index => Lang.Index;

        /// <summary>
        ///   查找类似 信息 的本地化字符串。
        /// </summary>
		public string Info => Lang.Info;

        /// <summary>
        ///   查找类似 可交互对话框 的本地化字符串。
        /// </summary>
		public string InteractiveDialog => Lang.InteractiveDialog;

        /// <summary>
        ///   查找类似 不是手机号码 的本地化字符串。
        /// </summary>
		public string IsNotPhone => Lang.IsNotPhone;

        /// <summary>
        ///   查找类似 标签 的本地化字符串。
        /// </summary>
		public string Label => Lang.Label;

        /// <summary>
        ///   查找类似 查找类似 {0} 的本地化字符串。 的本地化字符串。
        /// </summary>
		public string LangComment => Lang.LangComment;

        /// <summary>
        ///   查找类似 列表框 的本地化字符串。
        /// </summary>
		public string ListBox => Lang.ListBox;

        /// <summary>
        ///   查找类似 列表视图 的本地化字符串。
        /// </summary>
		public string ListView => Lang.ListView;

        /// <summary>
        ///   查找类似 加载条 的本地化字符串。
        /// </summary>
		public string Loading => Lang.Loading;

        /// <summary>
        ///   查找类似 放大镜 的本地化字符串。
        /// </summary>
		public string Magnifier => Lang.Magnifier;

        /// <summary>
        ///   查找类似 菜单 的本地化字符串。
        /// </summary>
		public string Menu => Lang.Menu;

        /// <summary>
        ///   查找类似 消息框 的本地化字符串。
        /// </summary>
		public string MessageBox => Lang.MessageBox;

        /// <summary>
        ///   查找类似  的本地化字符串。
        /// </summary>
		public string Morphing_Animation => Lang.Morphing_Animation;

        /// <summary>
        ///   查找类似 名称 的本地化字符串。
        /// </summary>
		public string Name => Lang.Name;

        /// <summary>
        ///   查找类似 新建窗口 的本地化字符串。
        /// </summary>
		public string NewWindow => Lang.NewWindow;

        /// <summary>
        ///   查找类似 下一步 的本地化字符串。
        /// </summary>
		public string Next => Lang.Next;

        /// <summary>
        ///   查找类似 桌面通知 的本地化字符串。
        /// </summary>
		public string Notification => Lang.Notification;

        /// <summary>
        ///   查找类似 托盘图标 的本地化字符串。
        /// </summary>
		public string NotifyIcon => Lang.NotifyIcon;

        /// <summary>
        ///   查找类似 数值选择控件 的本地化字符串。
        /// </summary>
		public string NumericUpDown => Lang.NumericUpDown;

        /// <summary>
        ///   查找类似 关 的本地化字符串。
        /// </summary>
		public string Off => Lang.Off;

        /// <summary>
        ///   查找类似 确定 的本地化字符串。
        /// </summary>
		public string Ok => Lang.Ok;

        /// <summary>
        ///   查找类似 开 的本地化字符串。
        /// </summary>
		public string On => Lang.On;

        /// <summary>
        ///   查找类似 点击打开背景模糊窗口 的本地化字符串。
        /// </summary>
		public string OpenBlurWindow => Lang.OpenBlurWindow;

        /// <summary>
        ///   查找类似 点击打开常规窗口 的本地化字符串。
        /// </summary>
		public string OpenCommonWindow => Lang.OpenCommonWindow;

        /// <summary>
        ///   查找类似 点击打开自定义内容窗口 的本地化字符串。
        /// </summary>
		public string OpenCustomContentWindow => Lang.OpenCustomContentWindow;

        /// <summary>
        ///   查找类似 点击打开自定义消息窗口 的本地化字符串。
        /// </summary>
		public string OpenCustomMessageWindow => Lang.OpenCustomMessageWindow;

        /// <summary>
        ///   查找类似 点击打开自定义非客户端区域窗口 的本地化字符串。
        /// </summary>
		public string OpenCustomNonClientAreaWindow => Lang.OpenCustomNonClientAreaWindow;

        /// <summary>
        ///   查找类似 点击打开辉光窗口 的本地化字符串。
        /// </summary>
		public string OpenGlowWindow => Lang.OpenGlowWindow;

        /// <summary>
        ///   查找类似 点击打开图片浏览器 的本地化字符串。
        /// </summary>
		public string OpenImageBrowser => Lang.OpenImageBrowser;

        /// <summary>
        ///   查找类似 点击打开消息窗口 的本地化字符串。
        /// </summary>
		public string OpenMessageWindow => Lang.OpenMessageWindow;

        /// <summary>
        ///   查找类似 点击打开鼠标跟随窗口 的本地化字符串。
        /// </summary>
		public string OpenMouseFollowWindow => Lang.OpenMouseFollowWindow;

        /// <summary>
        ///   查找类似 点击打开原生常规窗口 的本地化字符串。
        /// </summary>
		public string OpenNativeCommonWindow => Lang.OpenNativeCommonWindow;

        /// <summary>
        ///   查找类似 点击打开导航窗口 的本地化字符串。
        /// </summary>
		public string OpenNavigationWindow => Lang.OpenNavigationWindow;

        /// <summary>
        ///   查找类似 打开无非客户端区域可拖拽窗口 的本地化字符串。
        /// </summary>
		public string OpenNoNonClientAreaDragableWindow => Lang.OpenNoNonClientAreaDragableWindow;

        /// <summary>
        ///   查找类似 打开面板 的本地化字符串。
        /// </summary>
		public string OpenPanel => Lang.OpenPanel;

        /// <summary>
        ///   查找类似 打开精灵 的本地化字符串。
        /// </summary>
		public string OpenSprite => Lang.OpenSprite;

        /// <summary>
        ///   查找类似 轮廓文本 的本地化字符串。
        /// </summary>
		public string OutlineText => Lang.OutlineText;

        /// <summary>
        ///   查找类似 页码条 的本地化字符串。
        /// </summary>
		public string Pagination => Lang.Pagination;

        /// <summary>
        ///   查找类似 密码框 的本地化字符串。
        /// </summary>
		public string PasswordBox => Lang.PasswordBox;

        /// <summary>
        ///   查找类似 PIN码框 的本地化字符串。
        /// </summary>
		public string PinBox => Lang.PinBox;

        /// <summary>
        ///   查找类似 请输入... 的本地化字符串。
        /// </summary>
		public string PleaseInput => Lang.PleaseInput;

        /// <summary>
        ///   查找类似 请稍后... 的本地化字符串。
        /// </summary>
		public string PleaseWait => Lang.PleaseWait;

        /// <summary>
        ///   查找类似 请输入内容 的本地化字符串。
        /// </summary>
		public string PlsEnterContent => Lang.PlsEnterContent;

        /// <summary>
        ///   查找类似 请输入邮箱 的本地化字符串。
        /// </summary>
		public string PlsEnterEmail => Lang.PlsEnterEmail;

        /// <summary>
        ///   查找类似 请输入关键字 的本地化字符串。
        /// </summary>
		public string PlsEnterKey => Lang.PlsEnterKey;

        /// <summary>
        ///   查找类似 气泡提示 的本地化字符串。
        /// </summary>
		public string Poptip => Lang.Poptip;

        /// <summary>
        ///   查找类似 上左;上边;上右;右上;右边;右下;下右;下边;下左;左下;左边;左上 的本地化字符串。
        /// </summary>
		public string PoptipPositionStr => Lang.PoptipPositionStr;

        /// <summary>
        ///   查找类似 实用例子 的本地化字符串。
        /// </summary>
		public string PracticalDemos => Lang.PracticalDemos;

        /// <summary>
        ///   查找类似 上一步 的本地化字符串。
        /// </summary>
		public string Prev => Lang.Prev;

        /// <summary>
        ///   查找类似 预览滑块 的本地化字符串。
        /// </summary>
		public string PreviewSlider => Lang.PreviewSlider;

        /// <summary>
        ///   查找类似 主要 的本地化字符串。
        /// </summary>
		public string Primary => Lang.Primary;

        /// <summary>
        ///   查找类似 进度条 的本地化字符串。
        /// </summary>
		public string ProgressBar => Lang.ProgressBar;

        /// <summary>
        ///   查找类似 进度按钮 的本地化字符串。
        /// </summary>
		public string ProgressButton => Lang.ProgressButton;

        /// <summary>
        ///   查找类似 项目 的本地化字符串。
        /// </summary>
		public string Project => Lang.Project;

        /// <summary>
        ///   查找类似 属性编辑器 的本地化字符串。
        /// </summary>
        public string PropertyGrid => Lang.PropertyGrid;

        /// <summary>
        ///   查找类似 按住说话 的本地化字符串。
        /// </summary>
		public string PushToTalk => Lang.PushToTalk;

        /// <summary>
        ///   查找类似 QQ群 的本地化字符串。
        /// </summary>
		public string QQGroup => Lang.QQGroup;

        /// <summary>
        ///   查找类似 单选按钮 的本地化字符串。
        /// </summary>
		public string RadioButton => Lang.RadioButton;

        /// <summary>
        ///   查找类似 范围滑块 的本地化字符串。
        /// </summary>
		public string RangeSlider => Lang.RangeSlider;

        /// <summary>
        ///   查找类似 评分 的本地化字符串。
        /// </summary>
		public string Rate => Lang.Rate;

        /// <summary>
        ///   查找类似 群友推荐 的本地化字符串。
        /// </summary>
		public string Recommendation => Lang.Recommendation;

        /// <summary>
        ///   查找类似 注册 的本地化字符串。
        /// </summary>
		public string Register => Lang.Register;

        /// <summary>
        ///   查找类似 相对布局 的本地化字符串。
        /// </summary>
		public string RelativePanel => Lang.RelativePanel;

        /// <summary>
        ///   查找类似 备注 的本地化字符串。
        /// </summary>
		public string Remark => Lang.Remark;

        /// <summary>
        ///   查找类似 删除项 的本地化字符串。
        /// </summary>
		public string RemoveItem => Lang.RemoveItem;

        /// <summary>
        ///   查找类似 重复按钮 的本地化字符串。
        /// </summary>
		public string RepeatButton => Lang.RepeatButton;

        /// <summary>
        ///   查找类似 回复 的本地化字符串。
        /// </summary>
		public string Reply => Lang.Reply;

        /// <summary>
        ///   查找类似 代码仓库 的本地化字符串。
        /// </summary>
		public string Repository => Lang.Repository;

        /// <summary>
        ///   查找类似 响应式布局 的本地化字符串。
        /// </summary>
		public string ResponsiveLayout => Lang.ResponsiveLayout;

        /// <summary>
        ///   查找类似 富文本框 的本地化字符串。
        /// </summary>
		public string RichTextBox => Lang.RichTextBox;

        /// <summary>
        ///   查找类似 在这里右击 的本地化字符串。
        /// </summary>
		public string RightClickHere => Lang.RightClickHere;

        /// <summary>
        ///   查找类似 滚动块 的本地化字符串。
        /// </summary>
		public string RunningBlock => Lang.RunningBlock;

        /// <summary>
        ///   查找类似 截图 的本地化字符串。
        /// </summary>
		public string Screenshot => Lang.Screenshot;

        /// <summary>
        ///   查找类似 滚动视图 的本地化字符串。
        /// </summary>
		public string ScrollViewer => Lang.ScrollViewer;

        /// <summary>
        ///   查找类似 搜索栏 的本地化字符串。
        /// </summary>
		public string SearchBar => Lang.SearchBar;

        /// <summary>
        ///   查找类似 秒 的本地化字符串。
        /// </summary>
		public string Second => Lang.Second;

        /// <summary>
        ///   查找类似 选中 的本地化字符串。
        /// </summary>
		public string Selected => Lang.Selected;

        /// <summary>
        ///   查找类似 发送通知 的本地化字符串。
        /// </summary>
		public string SendNotification => Lang.SendNotification;

        /// <summary>
        ///   查找类似 徽章 的本地化字符串。
        /// </summary>
		public string Shield => Lang.Shield;

        /// <summary>
        ///   查找类似 在当前窗口显示 的本地化字符串。
        /// </summary>
		public string ShowInCurrentWindow => Lang.ShowInCurrentWindow;

        /// <summary>
        ///   查找类似 在主窗口显示 的本地化字符串。
        /// </summary>
		public string ShowInMainWindow => Lang.ShowInMainWindow;

        /// <summary>
        ///   查找类似 显示行号 的本地化字符串。
        /// </summary>
		public string ShowRowNumber => Lang.ShowRowNumber;

        /// <summary>
        ///   查找类似 侧边菜单 的本地化字符串。
        /// </summary>
		public string SideMenu => Lang.SideMenu;

        /// <summary>
        ///   查找类似 滑块 的本地化字符串。
        /// </summary>
		public string Slider => Lang.Slider;

        /// <summary>
        ///   查找类似 分割按钮 的本地化字符串。
        /// </summary>
		public string SplitButton => Lang.SplitButton;

        /// <summary>
        ///   查找类似 精灵 的本地化字符串。
        /// </summary>
		public string Sprite => Lang.Sprite;

        /// <summary>
        ///   查找类似 开始截图 的本地化字符串。
        /// </summary>
		public string StartScreenshot => Lang.StartScreenshot;

        /// <summary>
        ///   查找类似 保持打开 的本地化字符串。
        /// </summary>
		public string StaysOpen => Lang.StaysOpen;

        /// <summary>
        ///   查找类似 步骤 的本地化字符串。
        /// </summary>
		public string Step => Lang.Step;

        /// <summary>
        ///   查找类似 步骤条 的本地化字符串。
        /// </summary>
		public string StepBar => Lang.StepBar;

        /// <summary>
        ///   查找类似 样式模板 的本地化字符串。
        /// </summary>
		public string Styles => Lang.Styles;

        /// <summary>
        ///   查找类似 子标题 的本地化字符串。
        /// </summary>
		public string SubTitle => Lang.SubTitle;

        /// <summary>
        ///   查找类似 成功 的本地化字符串。
        /// </summary>
		public string Success => Lang.Success;

        /// <summary>
        ///   查找类似 选项卡控件 的本地化字符串。
        /// </summary>
		public string TabControl => Lang.TabControl;

        /// <summary>
        ///   查找类似 标签 的本地化字符串。
        /// </summary>
		public string Tag => Lang.Tag;

        /// <summary>
        ///   查找类似 正文 的本地化字符串。
        /// </summary>
		public string Text => Lang.Text;

        /// <summary>
        ///   查找类似 文本块 的本地化字符串。
        /// </summary>
		public string TextBlock => Lang.TextBlock;

        /// <summary>
        ///   查找类似 文本框 的本地化字符串。
        /// </summary>
		public string TextBox => Lang.TextBox;

        /// <summary>
        ///   查找类似 文本对话框 的本地化字符串。
        /// </summary>
		public string TextDialog => Lang.TextDialog;

        /// <summary>
        ///   查找类似 文本对话框（控件中） 的本地化字符串。
        /// </summary>
		public string TextDialogInControl => Lang.TextDialogInControl;

        /// <summary>
        ///   查找类似 文本对话框，带计时器 的本地化字符串。
        /// </summary>
		public string TextDialogWithTimer => Lang.TextDialogWithTimer;

        /// <summary>
        ///   查找类似 时间条 的本地化字符串。
        /// </summary>
		public string TimeBar => Lang.TimeBar;

        /// <summary>
        ///   查找类似 时间选择器 的本地化字符串。
        /// </summary>
		public string TimePicker => Lang.TimePicker;

        /// <summary>
        ///   查找类似 提示 的本地化字符串。
        /// </summary>
		public string Tip => Lang.Tip;

        /// <summary>
        ///   查找类似 标题 的本地化字符串。
        /// </summary>
		public string Title => Lang.Title;

        /// <summary>
        ///   查找类似 这是标题 的本地化字符串。
        /// </summary>
		public string TitleDemoStr1 => Lang.TitleDemoStr1;

        /// <summary>
        ///   查找类似 此项必填 的本地化字符串。
        /// </summary>
		public string TitleDemoStr2 => Lang.TitleDemoStr2;

        /// <summary>
        ///   查找类似 标题在左侧 的本地化字符串。
        /// </summary>
		public string TitleDemoStr3 => Lang.TitleDemoStr3;

        /// <summary>
        ///   查找类似 切换按钮 的本地化字符串。
        /// </summary>
		public string ToggleButton => Lang.ToggleButton;

        /// <summary>
        ///   查找类似 工具条 的本地化字符串。
        /// </summary>
		public string ToolBar => Lang.ToolBar;

        /// <summary>
        ///   查找类似 工具 的本地化字符串。
        /// </summary>
		public string Tools => Lang.Tools;

        /// <summary>
        ///   查找类似 穿梭框 的本地化字符串。
        /// </summary>
		public string Transfer => Lang.Transfer;

        /// <summary>
        ///   查找类似 内容过渡控件 的本地化字符串。
        /// </summary>
		public string TransitioningContentControl => Lang.TransitioningContentControl;

        /// <summary>
        ///   查找类似 树视图 的本地化字符串。
        /// </summary>
		public string TreeView => Lang.TreeView;

        /// <summary>
        ///   查找类似 试试关闭程序吧？ 的本地化字符串。
        /// </summary>
		public string Try2CloseApp => Lang.Try2CloseApp;

        /// <summary>
        ///   查找类似 类型 的本地化字符串。
        /// </summary>
		public string Type => Lang.Type;

        /// <summary>
        ///   查找类似 上传文件 的本地化字符串。
        /// </summary>
		public string UploadFile => Lang.UploadFile;

        /// <summary>
        ///   查找类似 上传;上传中 的本地化字符串。
        /// </summary>
		public string UploadStr => Lang.UploadStr;

        /// <summary>
        ///   查找类似 可见性 的本地化字符串。
        /// </summary>
		public string Visibility => Lang.Visibility;

        /// <summary>
        ///   查找类似 VS 插件 的本地化字符串。
        /// </summary>
		public string Vsix => Lang.Vsix;

        /// <summary>
        ///   查找类似 警告 的本地化字符串。
        /// </summary>
		public string Warning => Lang.Warning;

        /// <summary>
        ///   查找类似 瀑布流 的本地化字符串。
        /// </summary>
		public string WaterfallPanel => Lang.WaterfallPanel;

        /// <summary>
        ///   查找类似 网站 的本地化字符串。
        /// </summary>
		public string Website => Lang.Website;

        /// <summary>
        ///   查找类似 窗口 的本地化字符串。
        /// </summary>
		public string Window => Lang.Window;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class LangKeys
    {
        /// <summary>
        ///   查找类似 关于 的本地化字符串。
        /// </summary>
		public static string About = nameof(About);

        /// <summary>
        ///   查找类似 添加项 的本地化字符串。
        /// </summary>
		public static string AddItem = nameof(AddItem);

        /// <summary>
        ///   查找类似 动画路径 的本地化字符串。
        /// </summary>
		public static string AnimationPath = nameof(AnimationPath);

        /// <summary>
        ///   查找类似 托盘图标已打开，将隐藏窗口而不是关闭程序 的本地化字符串。
        /// </summary>
		public static string AppClosingTip = nameof(AppClosingTip);

        /// <summary>
        ///   查找类似 询问 的本地化字符串。
        /// </summary>
		public static string Ask = nameof(Ask);

        /// <summary>
        ///   查找类似 标记 的本地化字符串。
        /// </summary>
		public static string Badge = nameof(Badge);

        /// <summary>
        ///   查找类似 填写基本信息 的本地化字符串。
        /// </summary>
		public static string BasicInfo = nameof(BasicInfo);

        /// <summary>
        ///   查找类似 基础布局 的本地化字符串。
        /// </summary>
		public static string BasicLayout = nameof(BasicLayout);

        /// <summary>
        ///   查找类似 闪烁 的本地化字符串。
        /// </summary>
		public static string Blink = nameof(Blink);

        /// <summary>
        ///   查找类似 博客 的本地化字符串。
        /// </summary>
		public static string Blog = nameof(Blog);

        /// <summary>
        ///   查找类似 边框 的本地化字符串。
        /// </summary>
		public static string Border = nameof(Border);

        /// <summary>
        ///   查找类似 画刷 的本地化字符串。
        /// </summary>
		public static string Brush = nameof(Brush);

        /// <summary>
        ///   查找类似 按钮 的本地化字符串。
        /// </summary>
		public static string Button = nameof(Button);

        /// <summary>
        ///   查找类似 自定义按钮 的本地化字符串。
        /// </summary>
		public static string ButtonCustom = nameof(ButtonCustom);

        /// <summary>
        ///   查找类似 按钮组 的本地化字符串。
        /// </summary>
		public static string ButtonGroup = nameof(ButtonGroup);

        /// <summary>
        ///   查找类似 日历 的本地化字符串。
        /// </summary>
		public static string Calendar = nameof(Calendar);

        /// <summary>
        ///   查找类似 带时钟的日历 的本地化字符串。
        /// </summary>
		public static string CalendarWithClock = nameof(CalendarWithClock);

        /// <summary>
        ///   查找类似 卡片 的本地化字符串。
        /// </summary>
		public static string Card = nameof(Card);

        /// <summary>
        ///   查找类似 轮播 的本地化字符串。
        /// </summary>
		public static string Carousel = nameof(Carousel);

        /// <summary>
        ///   查找类似 是否重启以更改语言？ 的本地化字符串。
        /// </summary>
		public static string ChangeLangAsk = nameof(ChangeLangAsk);

        /// <summary>
        ///   查找类似 对话气泡 的本地化字符串。
        /// </summary>
		public static string ChatBubble = nameof(ChatBubble);

        /// <summary>
        ///   查找类似 讨论室 的本地化字符串。
        /// </summary>
		public static string Chatroom = nameof(Chatroom);

        /// <summary>
        ///   查找类似 复选框 的本地化字符串。
        /// </summary>
		public static string CheckBox = nameof(CheckBox);

        /// <summary>
        ///   查找类似 圆形布局 的本地化字符串。
        /// </summary>
		public static string CirclePanel = nameof(CirclePanel);

        /// <summary>
        ///   查找类似 清空 的本地化字符串。
        /// </summary>
		public static string Clear = nameof(Clear);

        /// <summary>
        ///   查找类似 点击计数 的本地化字符串。
        /// </summary>
		public static string Click2Count = nameof(Click2Count);

        /// <summary>
        ///   查找类似 时钟 的本地化字符串。
        /// </summary>
		public static string Clock = nameof(Clock);

        /// <summary>
        ///   查找类似 颜色拾取器 的本地化字符串。
        /// </summary>
		public static string ColorPicker = nameof(ColorPicker);

        /// <summary>
        ///   查找类似 分栏偏移 的本地化字符串。
        /// </summary>
		public static string ColumnOffset = nameof(ColumnOffset);

        /// <summary>
        ///   查找类似 分栏间隔 的本地化字符串。
        /// </summary>
		public static string ColumnSpacing = nameof(ColumnSpacing);

        /// <summary>
        ///   查找类似 组合框 的本地化字符串。
        /// </summary>
		public static string ComboBox = nameof(ComboBox);

        /// <summary>
        ///   查找类似 敬请期待 的本地化字符串。
        /// </summary>
		public static string ComingSoon = nameof(ComingSoon);

        /// <summary>
        ///   查找类似 评论 的本地化字符串。
        /// </summary>
		public static string Comment = nameof(Comment);

        /// <summary>
        ///   查找类似 一般 的本地化字符串。
        /// </summary>
		public static string Common = nameof(Common);

        /// <summary>
        ///   查找类似 对比滑块 的本地化字符串。
        /// </summary>
		public static string CompareSlider = nameof(CompareSlider);

        /// <summary>
        ///   查找类似 完成 的本地化字符串。
        /// </summary>
		public static string Complete = nameof(Complete);

        /// <summary>
        ///   查找类似 这是内容 的本地化字符串。
        /// </summary>
		public static string ContentDemoStr = nameof(ContentDemoStr);

        /// <summary>
        ///   查找类似 贡献者 的本地化字符串。
        /// </summary>
		public static string Contributors = nameof(Contributors);

        /// <summary>
        ///   查找类似 控件 的本地化字符串。
        /// </summary>
		public static string Controls = nameof(Controls);

        /// <summary>
        ///   查找类似 封面流 的本地化字符串。
        /// </summary>
		public static string CoverFlow = nameof(CoverFlow);

        /// <summary>
        ///   查找类似 封面视图 的本地化字符串。
        /// </summary>
		public static string CoverView = nameof(CoverView);

        /// <summary>
        ///   查找类似 危险 的本地化字符串。
        /// </summary>
		public static string Danger = nameof(Danger);

        /// <summary>
        ///   查找类似 数据表格 的本地化字符串。
        /// </summary>
		public static string DataGrid = nameof(DataGrid);

        /// <summary>
        ///   查找类似 日期选择器 的本地化字符串。
        /// </summary>
		public static string DatePicker = nameof(DatePicker);

        /// <summary>
        ///   查找类似 日期时间选择器 的本地化字符串。
        /// </summary>
		public static string DateTimePicker = nameof(DateTimePicker);

        /// <summary>
        ///   查找类似 默认 的本地化字符串。
        /// </summary>
		public static string Default = nameof(Default);

        /// <summary>
        ///   查找类似 示例 的本地化字符串。
        /// </summary>
		public static string Demo = nameof(Demo);

        /// <summary>
        ///   查找类似 对话框 的本地化字符串。
        /// </summary>
		public static string Dialog = nameof(Dialog);

        /// <summary>
        ///   查找类似 对话框示例 的本地化字符串。
        /// </summary>
		public static string DialogDemo = nameof(DialogDemo);

        /// <summary>
        ///   查找类似 分割线 的本地化字符串。
        /// </summary>
		public static string Divider = nameof(Divider);

        /// <summary>
        ///   查找类似 中文文档 的本地化字符串。
        /// </summary>
		public static string Doc_cn = nameof(Doc_cn);

        /// <summary>
        ///   查找类似 英文文档 的本地化字符串。
        /// </summary>
		public static string Doc_en = nameof(Doc_en);

        /// <summary>
        ///   查找类似 文献资料 的本地化字符串。
        /// </summary>
		public static string Documentation = nameof(Documentation);

        /// <summary>
        ///   查找类似 在这里拖拽 的本地化字符串。
        /// </summary>
		public static string DragHere = nameof(DragHere);

        /// <summary>
        ///   查找类似 抽屉 的本地化字符串。
        /// </summary>
		public static string Drawer = nameof(Drawer);

        /// <summary>
        ///   查找类似 效果 的本地化字符串。
        /// </summary>
		public static string Effects = nameof(Effects);

        /// <summary>
        ///   查找类似 邮箱 的本地化字符串。
        /// </summary>
		public static string Email = nameof(Email);

        /// <summary>
        ///   查找类似 错误 的本地化字符串。
        /// </summary>
		public static string Error = nameof(Error);

        /// <summary>
        ///   查找类似 退出 的本地化字符串。
        /// </summary>
		public static string Exit = nameof(Exit);

        /// <summary>
        ///   查找类似 展开框 的本地化字符串。
        /// </summary>
		public static string Expander = nameof(Expander);

        /// <summary>
        ///   查找类似 严重 的本地化字符串。
        /// </summary>
		public static string Fatal = nameof(Fatal);

        /// <summary>
        ///   查找类似 翻页时钟 的本地化字符串。
        /// </summary>
		public static string FlipClock = nameof(FlipClock);

        /// <summary>
        ///   查找类似 漂浮块 的本地化字符串。
        /// </summary>
		public static string FloatingBlock = nameof(FloatingBlock);

        /// <summary>
        ///   查找类似 流文档 的本地化字符串。
        /// </summary>
		public static string FlowDocument = nameof(FlowDocument);

        /// <summary>
        ///   查找类似 流文档单页视图 的本地化字符串。
        /// </summary>
		public static string FlowDocumentPageViewer = nameof(FlowDocumentPageViewer);

        /// <summary>
        ///   查找类似 流文档查看器 的本地化字符串。
        /// </summary>
		public static string FlowDocumentReader = nameof(FlowDocumentReader);

        /// <summary>
        ///   查找类似 流文档滚动视图 的本地化字符串。
        /// </summary>
		public static string FlowDocumentScrollViewer = nameof(FlowDocumentScrollViewer);

        /// <summary>
        ///   查找类似 导航框架 的本地化字符串。
        /// </summary>
		public static string Frame = nameof(Frame);

        /// <summary>
        ///   查找类似 Gif图片 的本地化字符串。
        /// </summary>
		public static string GifImage = nameof(GifImage);

        /// <summary>
        ///   查找类似 回到顶部 的本地化字符串。
        /// </summary>
		public static string GotoTop = nameof(GotoTop);

        /// <summary>
        ///   查找类似 头像 的本地化字符串。
        /// </summary>
		public static string Gravatar = nameof(Gravatar);

        /// <summary>
        ///   查找类似 栅格 的本地化字符串。
        /// </summary>
		public static string Grid = nameof(Grid);

        /// <summary>
        ///   查找类似 分组框 的本地化字符串。
        /// </summary>
		public static string GroupBox = nameof(GroupBox);

        /// <summary>
        ///   查找类似 组数 的本地化字符串。
        /// </summary>
		public static string Groups = nameof(Groups);

        /// <summary>
        ///   查找类似 信息通知 的本地化字符串。
        /// </summary>
		public static string Growl = nameof(Growl);

        /// <summary>
        ///   查找类似 检测到有新版本！是否更新？ 的本地化字符串。
        /// </summary>
		public static string GrowlAsk = nameof(GrowlAsk);

        /// <summary>
        ///   查找类似 消息通知示例 的本地化字符串。
        /// </summary>
		public static string GrowlDemo = nameof(GrowlDemo);

        /// <summary>
        ///   查找类似 连接失败，请检查网络！ 的本地化字符串。
        /// </summary>
		public static string GrowlError = nameof(GrowlError);

        /// <summary>
        ///   查找类似 程序已崩溃~~~ 的本地化字符串。
        /// </summary>
		public static string GrowlFatal = nameof(GrowlFatal);

        /// <summary>
        ///   查找类似 今天的天气不错~~~ 的本地化字符串。
        /// </summary>
		public static string GrowlInfo = nameof(GrowlInfo);

        /// <summary>
        ///   查找类似 文件保存成功！ 的本地化字符串。
        /// </summary>
		public static string GrowlSuccess = nameof(GrowlSuccess);

        /// <summary>
        ///   查找类似 磁盘空间快要满了！ 的本地化字符串。
        /// </summary>
		public static string GrowlWarning = nameof(GrowlWarning);

        /// <summary>
        ///   查找类似 阴影画笔生成器 的本地化字符串。
        /// </summary>
		public static string HatchBrushGenerator = nameof(HatchBrushGenerator);

        /// <summary>
        ///   查找类似 蜂窝布局 的本地化字符串。
        /// </summary>
		public static string HoneycombPanel = nameof(HoneycombPanel);

        /// <summary>
        ///   查找类似 混合布局 的本地化字符串。
        /// </summary>
		public static string HybridLayout = nameof(HybridLayout);

        /// <summary>
        ///   查找类似 忽略 的本地化字符串。
        /// </summary>
		public static string Ignore = nameof(Ignore);

        /// <summary>
        ///   查找类似 图片块 的本地化字符串。
        /// </summary>
		public static string ImageBlock = nameof(ImageBlock);

        /// <summary>
        ///   查找类似 图片浏览器 的本地化字符串。
        /// </summary>
		public static string ImageBrowser = nameof(ImageBrowser);

        /// <summary>
        ///   查找类似 图片选择器 的本地化字符串。
        /// </summary>
        public static string ImageSelector = nameof(ImageSelector);

        /// <summary>
        ///   查找类似 索引 的本地化字符串。
        /// </summary>
		public static string Index = nameof(Index);

        /// <summary>
        ///   查找类似 信息 的本地化字符串。
        /// </summary>
		public static string Info = nameof(Info);

        /// <summary>
        ///   查找类似 可交互对话框 的本地化字符串。
        /// </summary>
		public static string InteractiveDialog = nameof(InteractiveDialog);

        /// <summary>
        ///   查找类似 不是手机号码 的本地化字符串。
        /// </summary>
		public static string IsNotPhone = nameof(IsNotPhone);

        /// <summary>
        ///   查找类似 标签 的本地化字符串。
        /// </summary>
		public static string Label = nameof(Label);

        /// <summary>
        ///   查找类似 查找类似 {0} 的本地化字符串。 的本地化字符串。
        /// </summary>
		public static string LangComment = nameof(LangComment);

        /// <summary>
        ///   查找类似 列表框 的本地化字符串。
        /// </summary>
		public static string ListBox = nameof(ListBox);

        /// <summary>
        ///   查找类似 列表视图 的本地化字符串。
        /// </summary>
		public static string ListView = nameof(ListView);

        /// <summary>
        ///   查找类似 加载条 的本地化字符串。
        /// </summary>
		public static string Loading = nameof(Loading);

        /// <summary>
        ///   查找类似 放大镜 的本地化字符串。
        /// </summary>
		public static string Magnifier = nameof(Magnifier);

        /// <summary>
        ///   查找类似 菜单 的本地化字符串。
        /// </summary>
		public static string Menu = nameof(Menu);

        /// <summary>
        ///   查找类似 消息框 的本地化字符串。
        /// </summary>
		public static string MessageBox = nameof(MessageBox);

        /// <summary>
        ///   查找类似  的本地化字符串。
        /// </summary>
		public static string Morphing_Animation = nameof(Morphing_Animation);

        /// <summary>
        ///   查找类似 名称 的本地化字符串。
        /// </summary>
		public static string Name = nameof(Name);

        /// <summary>
        ///   查找类似 新建窗口 的本地化字符串。
        /// </summary>
		public static string NewWindow = nameof(NewWindow);

        /// <summary>
        ///   查找类似 下一步 的本地化字符串。
        /// </summary>
		public static string Next = nameof(Next);

        /// <summary>
        ///   查找类似 桌面通知 的本地化字符串。
        /// </summary>
		public static string Notification = nameof(Notification);

        /// <summary>
        ///   查找类似 托盘图标 的本地化字符串。
        /// </summary>
		public static string NotifyIcon = nameof(NotifyIcon);

        /// <summary>
        ///   查找类似 数值选择控件 的本地化字符串。
        /// </summary>
		public static string NumericUpDown = nameof(NumericUpDown);

        /// <summary>
        ///   查找类似 关 的本地化字符串。
        /// </summary>
		public static string Off = nameof(Off);

        /// <summary>
        ///   查找类似 确定 的本地化字符串。
        /// </summary>
		public static string Ok = nameof(Ok);

        /// <summary>
        ///   查找类似 开 的本地化字符串。
        /// </summary>
		public static string On = nameof(On);

        /// <summary>
        ///   查找类似 点击打开背景模糊窗口 的本地化字符串。
        /// </summary>
		public static string OpenBlurWindow = nameof(OpenBlurWindow);

        /// <summary>
        ///   查找类似 点击打开常规窗口 的本地化字符串。
        /// </summary>
		public static string OpenCommonWindow = nameof(OpenCommonWindow);

        /// <summary>
        ///   查找类似 点击打开自定义内容窗口 的本地化字符串。
        /// </summary>
		public static string OpenCustomContentWindow = nameof(OpenCustomContentWindow);

        /// <summary>
        ///   查找类似 点击打开自定义消息窗口 的本地化字符串。
        /// </summary>
		public static string OpenCustomMessageWindow = nameof(OpenCustomMessageWindow);

        /// <summary>
        ///   查找类似 点击打开自定义非客户端区域窗口 的本地化字符串。
        /// </summary>
		public static string OpenCustomNonClientAreaWindow = nameof(OpenCustomNonClientAreaWindow);

        /// <summary>
        ///   查找类似 点击打开辉光窗口 的本地化字符串。
        /// </summary>
		public static string OpenGlowWindow = nameof(OpenGlowWindow);

        /// <summary>
        ///   查找类似 点击打开图片浏览器 的本地化字符串。
        /// </summary>
		public static string OpenImageBrowser = nameof(OpenImageBrowser);

        /// <summary>
        ///   查找类似 点击打开消息窗口 的本地化字符串。
        /// </summary>
		public static string OpenMessageWindow = nameof(OpenMessageWindow);

        /// <summary>
        ///   查找类似 点击打开鼠标跟随窗口 的本地化字符串。
        /// </summary>
		public static string OpenMouseFollowWindow = nameof(OpenMouseFollowWindow);

        /// <summary>
        ///   查找类似 点击打开原生常规窗口 的本地化字符串。
        /// </summary>
		public static string OpenNativeCommonWindow = nameof(OpenNativeCommonWindow);

        /// <summary>
        ///   查找类似 点击打开导航窗口 的本地化字符串。
        /// </summary>
		public static string OpenNavigationWindow = nameof(OpenNavigationWindow);

        /// <summary>
        ///   查找类似 打开无非客户端区域可拖拽窗口 的本地化字符串。
        /// </summary>
		public static string OpenNoNonClientAreaDragableWindow = nameof(OpenNoNonClientAreaDragableWindow);

        /// <summary>
        ///   查找类似 打开面板 的本地化字符串。
        /// </summary>
		public static string OpenPanel = nameof(OpenPanel);

        /// <summary>
        ///   查找类似 打开精灵 的本地化字符串。
        /// </summary>
		public static string OpenSprite = nameof(OpenSprite);

        /// <summary>
        ///   查找类似 轮廓文本 的本地化字符串。
        /// </summary>
		public static string OutlineText = nameof(OutlineText);

        /// <summary>
        ///   查找类似 页码条 的本地化字符串。
        /// </summary>
		public static string Pagination = nameof(Pagination);

        /// <summary>
        ///   查找类似 密码框 的本地化字符串。
        /// </summary>
		public static string PasswordBox = nameof(PasswordBox);

        /// <summary>
        ///   查找类似 PIN码框 的本地化字符串。
        /// </summary>
		public static string PinBox = nameof(PinBox);

        /// <summary>
        ///   查找类似 请输入... 的本地化字符串。
        /// </summary>
		public static string PleaseInput = nameof(PleaseInput);

        /// <summary>
        ///   查找类似 请稍后... 的本地化字符串。
        /// </summary>
		public static string PleaseWait = nameof(PleaseWait);

        /// <summary>
        ///   查找类似 请输入内容 的本地化字符串。
        /// </summary>
		public static string PlsEnterContent = nameof(PlsEnterContent);

        /// <summary>
        ///   查找类似 请输入邮箱 的本地化字符串。
        /// </summary>
		public static string PlsEnterEmail = nameof(PlsEnterEmail);

        /// <summary>
        ///   查找类似 请输入关键字 的本地化字符串。
        /// </summary>
		public static string PlsEnterKey = nameof(PlsEnterKey);

        /// <summary>
        ///   查找类似 气泡提示 的本地化字符串。
        /// </summary>
		public static string Poptip = nameof(Poptip);

        /// <summary>
        ///   查找类似 上左;上边;上右;右上;右边;右下;下右;下边;下左;左下;左边;左上 的本地化字符串。
        /// </summary>
		public static string PoptipPositionStr = nameof(PoptipPositionStr);

        /// <summary>
        ///   查找类似 实用例子 的本地化字符串。
        /// </summary>
		public static string PracticalDemos = nameof(PracticalDemos);

        /// <summary>
        ///   查找类似 上一步 的本地化字符串。
        /// </summary>
		public static string Prev = nameof(Prev);

        /// <summary>
        ///   查找类似 预览滑块 的本地化字符串。
        /// </summary>
		public static string PreviewSlider = nameof(PreviewSlider);

        /// <summary>
        ///   查找类似 主要 的本地化字符串。
        /// </summary>
		public static string Primary = nameof(Primary);

        /// <summary>
        ///   查找类似 进度条 的本地化字符串。
        /// </summary>
		public static string ProgressBar = nameof(ProgressBar);

        /// <summary>
        ///   查找类似 进度按钮 的本地化字符串。
        /// </summary>
		public static string ProgressButton = nameof(ProgressButton);

        /// <summary>
        ///   查找类似 项目 的本地化字符串。
        /// </summary>
		public static string Project = nameof(Project);

        /// <summary>
        ///   查找类似 属性编辑器 的本地化字符串。
        /// </summary>
        public static string PropertyGrid = nameof(PropertyGrid);

        /// <summary>
        ///   查找类似 按住说话 的本地化字符串。
        /// </summary>
		public static string PushToTalk = nameof(PushToTalk);

        /// <summary>
        ///   查找类似 QQ群 的本地化字符串。
        /// </summary>
		public static string QQGroup = nameof(QQGroup);

        /// <summary>
        ///   查找类似 单选按钮 的本地化字符串。
        /// </summary>
		public static string RadioButton = nameof(RadioButton);

        /// <summary>
        ///   查找类似 范围滑块 的本地化字符串。
        /// </summary>
		public static string RangeSlider = nameof(RangeSlider);

        /// <summary>
        ///   查找类似 评分 的本地化字符串。
        /// </summary>
		public static string Rate = nameof(Rate);

        /// <summary>
        ///   查找类似 群友推荐 的本地化字符串。
        /// </summary>
		public static string Recommendation = nameof(Recommendation);

        /// <summary>
        ///   查找类似 注册 的本地化字符串。
        /// </summary>
		public static string Register = nameof(Register);

        /// <summary>
        ///   查找类似 相对布局 的本地化字符串。
        /// </summary>
		public static string RelativePanel = nameof(RelativePanel);

        /// <summary>
        ///   查找类似 备注 的本地化字符串。
        /// </summary>
		public static string Remark = nameof(Remark);

        /// <summary>
        ///   查找类似 删除项 的本地化字符串。
        /// </summary>
		public static string RemoveItem = nameof(RemoveItem);

        /// <summary>
        ///   查找类似 重复按钮 的本地化字符串。
        /// </summary>
		public static string RepeatButton = nameof(RepeatButton);

        /// <summary>
        ///   查找类似 回复 的本地化字符串。
        /// </summary>
		public static string Reply = nameof(Reply);

        /// <summary>
        ///   查找类似 代码仓库 的本地化字符串。
        /// </summary>
		public static string Repository = nameof(Repository);

        /// <summary>
        ///   查找类似 响应式布局 的本地化字符串。
        /// </summary>
		public static string ResponsiveLayout = nameof(ResponsiveLayout);

        /// <summary>
        ///   查找类似 富文本框 的本地化字符串。
        /// </summary>
		public static string RichTextBox = nameof(RichTextBox);

        /// <summary>
        ///   查找类似 在这里右击 的本地化字符串。
        /// </summary>
		public static string RightClickHere = nameof(RightClickHere);

        /// <summary>
        ///   查找类似 滚动块 的本地化字符串。
        /// </summary>
		public static string RunningBlock = nameof(RunningBlock);

        /// <summary>
        ///   查找类似 截图 的本地化字符串。
        /// </summary>
		public static string Screenshot = nameof(Screenshot);

        /// <summary>
        ///   查找类似 滚动视图 的本地化字符串。
        /// </summary>
		public static string ScrollViewer = nameof(ScrollViewer);

        /// <summary>
        ///   查找类似 搜索栏 的本地化字符串。
        /// </summary>
		public static string SearchBar = nameof(SearchBar);

        /// <summary>
        ///   查找类似 秒 的本地化字符串。
        /// </summary>
		public static string Second = nameof(Second);

        /// <summary>
        ///   查找类似 选中 的本地化字符串。
        /// </summary>
		public static string Selected = nameof(Selected);

        /// <summary>
        ///   查找类似 发送通知 的本地化字符串。
        /// </summary>
		public static string SendNotification = nameof(SendNotification);

        /// <summary>
        ///   查找类似 徽章 的本地化字符串。
        /// </summary>
		public static string Shield = nameof(Shield);

        /// <summary>
        ///   查找类似 在当前窗口显示 的本地化字符串。
        /// </summary>
		public static string ShowInCurrentWindow = nameof(ShowInCurrentWindow);

        /// <summary>
        ///   查找类似 在主窗口显示 的本地化字符串。
        /// </summary>
		public static string ShowInMainWindow = nameof(ShowInMainWindow);

        /// <summary>
        ///   查找类似 显示行号 的本地化字符串。
        /// </summary>
		public static string ShowRowNumber = nameof(ShowRowNumber);

        /// <summary>
        ///   查找类似 侧边菜单 的本地化字符串。
        /// </summary>
		public static string SideMenu = nameof(SideMenu);

        /// <summary>
        ///   查找类似 滑块 的本地化字符串。
        /// </summary>
		public static string Slider = nameof(Slider);

        /// <summary>
        ///   查找类似 分割按钮 的本地化字符串。
        /// </summary>
		public static string SplitButton = nameof(SplitButton);

        /// <summary>
        ///   查找类似 精灵 的本地化字符串。
        /// </summary>
		public static string Sprite = nameof(Sprite);

        /// <summary>
        ///   查找类似 开始截图 的本地化字符串。
        /// </summary>
		public static string StartScreenshot = nameof(StartScreenshot);

        /// <summary>
        ///   查找类似 保持打开 的本地化字符串。
        /// </summary>
		public static string StaysOpen = nameof(StaysOpen);

        /// <summary>
        ///   查找类似 步骤 的本地化字符串。
        /// </summary>
		public static string Step = nameof(Step);

        /// <summary>
        ///   查找类似 步骤条 的本地化字符串。
        /// </summary>
		public static string StepBar = nameof(StepBar);

        /// <summary>
        ///   查找类似 样式模板 的本地化字符串。
        /// </summary>
		public static string Styles = nameof(Styles);

        /// <summary>
        ///   查找类似 子标题 的本地化字符串。
        /// </summary>
		public static string SubTitle = nameof(SubTitle);

        /// <summary>
        ///   查找类似 成功 的本地化字符串。
        /// </summary>
		public static string Success = nameof(Success);

        /// <summary>
        ///   查找类似 选项卡控件 的本地化字符串。
        /// </summary>
		public static string TabControl = nameof(TabControl);

        /// <summary>
        ///   查找类似 标签 的本地化字符串。
        /// </summary>
		public static string Tag = nameof(Tag);

        /// <summary>
        ///   查找类似 正文 的本地化字符串。
        /// </summary>
		public static string Text = nameof(Text);

        /// <summary>
        ///   查找类似 文本块 的本地化字符串。
        /// </summary>
		public static string TextBlock = nameof(TextBlock);

        /// <summary>
        ///   查找类似 文本框 的本地化字符串。
        /// </summary>
		public static string TextBox = nameof(TextBox);

        /// <summary>
        ///   查找类似 文本对话框 的本地化字符串。
        /// </summary>
		public static string TextDialog = nameof(TextDialog);

        /// <summary>
        ///   查找类似 文本对话框（控件中） 的本地化字符串。
        /// </summary>
		public static string TextDialogInControl = nameof(TextDialogInControl);

        /// <summary>
        ///   查找类似 文本对话框，带计时器 的本地化字符串。
        /// </summary>
		public static string TextDialogWithTimer = nameof(TextDialogWithTimer);

        /// <summary>
        ///   查找类似 时间条 的本地化字符串。
        /// </summary>
		public static string TimeBar = nameof(TimeBar);

        /// <summary>
        ///   查找类似 时间选择器 的本地化字符串。
        /// </summary>
		public static string TimePicker = nameof(TimePicker);

        /// <summary>
        ///   查找类似 提示 的本地化字符串。
        /// </summary>
		public static string Tip = nameof(Tip);

        /// <summary>
        ///   查找类似 标题 的本地化字符串。
        /// </summary>
		public static string Title = nameof(Title);

        /// <summary>
        ///   查找类似 这是标题 的本地化字符串。
        /// </summary>
		public static string TitleDemoStr1 = nameof(TitleDemoStr1);

        /// <summary>
        ///   查找类似 此项必填 的本地化字符串。
        /// </summary>
		public static string TitleDemoStr2 = nameof(TitleDemoStr2);

        /// <summary>
        ///   查找类似 标题在左侧 的本地化字符串。
        /// </summary>
		public static string TitleDemoStr3 = nameof(TitleDemoStr3);

        /// <summary>
        ///   查找类似 切换按钮 的本地化字符串。
        /// </summary>
		public static string ToggleButton = nameof(ToggleButton);

        /// <summary>
        ///   查找类似 工具条 的本地化字符串。
        /// </summary>
		public static string ToolBar = nameof(ToolBar);

        /// <summary>
        ///   查找类似 工具 的本地化字符串。
        /// </summary>
		public static string Tools = nameof(Tools);

        /// <summary>
        ///   查找类似 穿梭框 的本地化字符串。
        /// </summary>
		public static string Transfer = nameof(Transfer);

        /// <summary>
        ///   查找类似 内容过渡控件 的本地化字符串。
        /// </summary>
		public static string TransitioningContentControl = nameof(TransitioningContentControl);

        /// <summary>
        ///   查找类似 树视图 的本地化字符串。
        /// </summary>
		public static string TreeView = nameof(TreeView);

        /// <summary>
        ///   查找类似 试试关闭程序吧？ 的本地化字符串。
        /// </summary>
		public static string Try2CloseApp = nameof(Try2CloseApp);

        /// <summary>
        ///   查找类似 类型 的本地化字符串。
        /// </summary>
		public static string Type = nameof(Type);

        /// <summary>
        ///   查找类似 上传文件 的本地化字符串。
        /// </summary>
		public static string UploadFile = nameof(UploadFile);

        /// <summary>
        ///   查找类似 上传;上传中 的本地化字符串。
        /// </summary>
		public static string UploadStr = nameof(UploadStr);

        /// <summary>
        ///   查找类似 可见性 的本地化字符串。
        /// </summary>
		public static string Visibility = nameof(Visibility);

        /// <summary>
        ///   查找类似 VS 插件 的本地化字符串。
        /// </summary>
		public static string Vsix = nameof(Vsix);

        /// <summary>
        ///   查找类似 警告 的本地化字符串。
        /// </summary>
		public static string Warning = nameof(Warning);

        /// <summary>
        ///   查找类似 瀑布流 的本地化字符串。
        /// </summary>
		public static string WaterfallPanel = nameof(WaterfallPanel);

        /// <summary>
        ///   查找类似 网站 的本地化字符串。
        /// </summary>
		public static string Website = nameof(Website);

        /// <summary>
        ///   查找类似 窗口 的本地化字符串。
        /// </summary>
		public static string Window = nameof(Window);

    }
}