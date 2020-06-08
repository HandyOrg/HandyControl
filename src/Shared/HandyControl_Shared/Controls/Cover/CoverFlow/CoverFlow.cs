using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    /// <summary>
    ///     封面流
    /// </summary>
    [TemplatePart(Name = ElementViewport3D, Type = typeof(Viewport3D))]
    [TemplatePart(Name = ElementCamera, Type = typeof(ProjectionCamera))]
    [TemplatePart(Name = ElementVisualParent, Type = typeof(ModelVisual3D))]
    public class CoverFlow : Control
    {
        private const string ElementViewport3D = "PART_Viewport3D";

        private const string ElementCamera = "PART_Camera";

        private const string ElementVisualParent = "PART_VisualParent";

        /// <summary>
        ///     最大显示数量的一半
        /// </summary>
        private const int MaxShowCountHalf = 3;

        /// <summary>
        ///     页码
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
            "PageIndex", typeof(int), typeof(CoverFlow),
            new PropertyMetadata(ValueBoxes.Int0Box, OnPageIndexChanged, CoercePageIndex));

        private static object CoercePageIndex(DependencyObject d, object baseValue)
        {
            var ctl = (CoverFlow)d;
            var v = (int)baseValue;

            if (v < 0)
            {
                return 0;
            }
            if (v >= ctl._contentDic.Count)
            {
                return ctl._contentDic.Count - 1;
            }
            return v;
        }

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (CoverFlow)d;
            ctl.UpdateIndex((int)e.NewValue);
        }

        /// <summary>
        ///     是否循环
        /// </summary>
        public static readonly DependencyProperty LoopProperty = DependencyProperty.Register(
            "Loop", typeof(bool), typeof(CoverFlow), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     存储所有的内容
        /// </summary>
        private readonly Dictionary<int, object> _contentDic = new Dictionary<int, object>();

        /// <summary>
        ///     当前在显示范围内的项
        /// </summary>
        private readonly Dictionary<int, CoverFlowItem> _itemShowDic = new Dictionary<int, CoverFlowItem>();

        /// <summary>
        ///     相机
        /// </summary>
        private ProjectionCamera _camera;

        private Point3DAnimation _point3DAnimation;

        /// <summary>
        ///     3d画布
        /// </summary>
        private Viewport3D _viewport3D;

        /// <summary>
        ///     项容器
        /// </summary>
        private ModelVisual3D _visualParent;

        /// <summary>
        ///     显示范围内第一个项的编号
        /// </summary>
        private int _firstShowIndex;

        /// <summary>
        ///     显示范围内最后一个项的编号
        /// </summary>
        private int _lastShowIndex;

        /// <summary>
        ///     跳转编号
        /// </summary>
        private int _jumpToIndex = -1;

        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            internal set => SetValue(PageIndexProperty, value);
        }

        /// <summary>
        ///     是否循环
        /// </summary>
        public bool Loop
        {
            get => (bool)GetValue(LoopProperty);
            set => SetValue(LoopProperty, value);
        }

        public override void OnApplyTemplate()
        {
            if (_viewport3D != null)
            {
                _viewport3D.Children.Clear();
                _itemShowDic.Clear();
                _viewport3D.MouseLeftButtonDown -= Viewport3D_MouseLeftButtonDown;
            }

            base.OnApplyTemplate();

            _viewport3D = GetTemplateChild(ElementViewport3D) as Viewport3D;
            if (_viewport3D != null)
            {
                _viewport3D.MouseLeftButtonDown += Viewport3D_MouseLeftButtonDown;
            }

            _camera = GetTemplateChild(ElementCamera) as ProjectionCamera;
            _visualParent = GetTemplateChild(ElementVisualParent) as ModelVisual3D;

            UpdateShowRange();
            if (_jumpToIndex > 0)
            {
                PageIndex = _jumpToIndex;
                _jumpToIndex = -1;
            }

            _point3DAnimation = new Point3DAnimation(new Point3D(CoverFlowItem.Interval * PageIndex, _camera.Position.Y, _camera.Position.Z), new Duration(TimeSpan.FromMilliseconds(200)));
            _camera.BeginAnimation(ProjectionCamera.PositionProperty, _point3DAnimation);
        }

        /// <summary>
        ///     批量添加资源
        /// </summary>
        /// <param name="contentList"></param>
        public void AddRange(IEnumerable<object> contentList)
        {
            foreach (var content in contentList)
            {
                _contentDic.Add(_contentDic.Count, content);
            }
        }

        /// <summary>
        ///     添加一项资源
        /// </summary>
        /// <param name="uriString"></param>
        public void Add(string uriString) => _contentDic.Add(_contentDic.Count, new Uri(uriString));

        /// <summary>
        ///     添加一项资源
        /// </summary>
        /// <param name="uri"></param>
        public void Add(Uri uri) => _contentDic.Add(_contentDic.Count, uri);

        /// <summary>
        ///     跳转
        /// </summary>
        public void JumpTo(int index) => _jumpToIndex = index;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0)
            {
                var index = PageIndex + 1;
                PageIndex = index >= _contentDic.Count ? Loop ? 0 : _contentDic.Count - 1 : index;
            }
            else
            {
                var index = PageIndex - 1;
                PageIndex = index < 0 ? Loop ? _contentDic.Count - 1 : 0 : index;
            }

            e.Handled = true;
        }

        /// <summary>
        ///     删除指定位置的项
        /// </summary>
        /// <param name="pos"></param>
        private void Remove(int pos)
        {
            if (!_itemShowDic.TryGetValue(pos, out var item)) return;
            _visualParent.Children.Remove(item);
            _itemShowDic.Remove(pos);
        }

        private void Viewport3D_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var result = (RayMeshGeometry3DHitTestResult)VisualTreeHelper.HitTest(_viewport3D, e.GetPosition(_viewport3D));
            if (result != null)
            {
                foreach (var item in _itemShowDic.Values)
                {
                    if (item.HitTest(result.MeshHit))
                    {
                        PageIndex = item.Index;
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     更新项的位置
        /// </summary>
        /// <param name="newIndex"></param>
        private void UpdateIndex(int newIndex)
        {
            UpdateShowRange();
            _itemShowDic.Do(item =>
            {
                if (item.Value.Index < _firstShowIndex || item.Value.Index > _lastShowIndex)
                {
                    Remove(item.Value.Index);
                }
                else
                {
                    item.Value.Move(PageIndex);
                }
            });

            _point3DAnimation = new Point3DAnimation(new Point3D(CoverFlowItem.Interval * newIndex, _camera.Position.Y,
                _camera.Position.Z), new Duration(TimeSpan.FromMilliseconds(200)));

            _camera.BeginAnimation(ProjectionCamera.PositionProperty, _point3DAnimation);
        }

        /// <summary>
        ///     更新显示范围
        /// </summary>
        private void UpdateShowRange()
        {
            var newFirstShowIndex = Math.Max(PageIndex - MaxShowCountHalf, 0);
            var newLastShowIndex = Math.Min(PageIndex + MaxShowCountHalf, _contentDic.Count - 1);

            for (var i = newFirstShowIndex; i <= newLastShowIndex; i++)
            {
                if (!_itemShowDic.ContainsKey(i))
                {
                    var cover = CreateCoverFlowItem(i, _contentDic[i]);
                    _itemShowDic[i] = cover;
                    _visualParent.Children.Add(cover);
                }
            }

            _firstShowIndex = newFirstShowIndex;
            _lastShowIndex = newLastShowIndex;
        }

        private CoverFlowItem CreateCoverFlowItem(int index, object content)
        {
            if (content is Uri uri)
            {
                try
                {
                    return new CoverFlowItem(index, PageIndex, new Image
                    {
                        Source = BitmapFrame.Create(uri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand)
                    });
                }
                catch
                {
                    return new CoverFlowItem(index, PageIndex, new ContentControl());
                }
            }

            return new CoverFlowItem(index, PageIndex, new ContentControl
            {
                Content = content
            });
        }
    }
}