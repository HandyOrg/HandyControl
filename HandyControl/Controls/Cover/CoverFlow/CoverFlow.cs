using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HandyControl.Data;

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
        private const int MaxShowCountHalf = 7;

        /// <summary>
        ///     页码
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
            "PageIndex", typeof(int), typeof(CoverFlow),
            new PropertyMetadata(ValueBoxes.Int0Box, OnPageIndexChanged, CoercePageIndex));

        private static object CoercePageIndex(DependencyObject d, object baseValue)
        {
            var ctl = (CoverFlow) d;
            var v = (int) baseValue;

            if (v < 0)
            {
                return 0;
            }
            if (v >= ctl._uriList.Count)
            {
                return ctl._uriList.Count - 1;
            }
            return v;
        }

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (CoverFlow) d;
            ctl.UpdateIndex((int)e.NewValue, (int)e.OldValue);
        }

        /// <summary>
        ///     是否循环
        /// </summary>
        public static readonly DependencyProperty LoopProperty = DependencyProperty.Register(
            "Loop", typeof(bool), typeof(CoverFlow), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     存储所有的资源标识符
        /// </summary>
        private readonly Dictionary<int, Uri> _uriList = new Dictionary<int, Uri>();

        /// <summary>
        ///     当前在显示范围内的项
        /// </summary>
        private readonly Dictionary<int, CoverFlowItem> _itemShowList = new Dictionary<int, CoverFlowItem>();

        /// <summary>
        ///     相机
        /// </summary>
        private ProjectionCamera _camera;

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
        ///     在获取新的模板后是否需要更新项
        /// </summary>
        private bool _updateItems;

        private bool _isLoaded;

        /// <summary>
        ///     跳转编号
        /// </summary>
        private int _jumpToIndex = -1;

        public CoverFlow()
        {
            Loaded += (s, e) =>
            {
                if (_isLoaded) return;
                _isLoaded = true;
                if (_updateItems)
                {
                    UpdateShowRange();
                    _updateItems = false;
                }
                if (_jumpToIndex > 0)
                {
                    PageIndex = _jumpToIndex;
                }
            };
        }

        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex
        {
            get => (int) GetValue(PageIndexProperty);
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
                _itemShowList.Clear();
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

            if (_isLoaded)
            {
                UpdateShowRange();
                _camera.Position = new Point3D(CoverFlowItem.Interval * PageIndex, _camera.Position.Y,
                    _camera.Position.Z);
            }
        }

        /// <summary>
        ///     批量添加资源
        /// </summary>
        /// <param name="uriList"></param>
        public void AddRange(List<Uri> uriList)
        {
            foreach (var uri in uriList)
            {
                _uriList.Add(_uriList.Count, uri);
            }
            _updateItems = true;
        }

        /// <summary>
        ///     添加一项资源
        /// </summary>
        /// <param name="uriString"></param>
        public void Add(string uriString)
        {
            _uriList.Add(_uriList.Count, new Uri(uriString));
            _updateItems = true;
        }

        /// <summary>
        ///     添加一项资源
        /// </summary>
        /// <param name="uri"></param>
        public void Add(Uri uri)
        {
            _uriList.Add(_uriList.Count, uri);
            _updateItems = true;
        }

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
                PageIndex = index >= _uriList.Count ? (Loop ? 0 : _uriList.Count - 1) : index;
            }
            else
            {
                var index = PageIndex - 1;
                PageIndex = index < 0 ? (Loop ? _uriList.Count - 1 : 0) : index;
            }
        }

        /// <summary>
        ///     删除指定位置的项
        /// </summary>
        /// <param name="pos"></param>
        private void Remove(int pos)
        {
            var item = _itemShowList[pos];
            _visualParent.Children.Remove(item);
            _itemShowList.Remove(pos);
        }

        /// <summary>
        ///     移动项到指定的位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="animated"></param>
        private void Move(int index, bool animated) => _itemShowList[index].Move(PageIndex, animated);

        private void Viewport3D_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var result = (RayMeshGeometry3DHitTestResult) VisualTreeHelper.HitTest(_viewport3D, e.GetPosition(_viewport3D));
            if (result != null)
            {
                foreach (var item in _itemShowList.Values)
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
        /// <param name="oldIndex"></param>
        private void UpdateIndex(int newIndex, int oldIndex)
        {
            var animate = Math.Abs(newIndex - oldIndex) < MaxShowCountHalf;
            UpdateShowRange();
            if (newIndex > oldIndex)
            {
                if (oldIndex < _firstShowIndex)
                {
                    oldIndex = _firstShowIndex;
                }
                for (var i = oldIndex; i <= newIndex; i++)
                {
                    Move(i, animate);
                }
            }
            else
            {
                if (oldIndex > _lastShowIndex)
                {
                    oldIndex = _lastShowIndex;
                }
                for (var i = oldIndex; i >= newIndex; i--)
                {
                    Move(i, animate);
                }
            }
            _camera.Position = new Point3D(CoverFlowItem.Interval * newIndex, _camera.Position.Y,
                _camera.Position.Z);
        }

        /// <summary>
        ///     更新显示范围
        /// </summary>
        private void UpdateShowRange()
        {
            var newFirstShowIndex = Math.Max(PageIndex - MaxShowCountHalf, 0);
            var newLastShowIndex = Math.Min(PageIndex + MaxShowCountHalf, _uriList.Count - 1);

            if (_firstShowIndex < newFirstShowIndex)
            {
                for (var i = _firstShowIndex; i < newFirstShowIndex; i++)
                {
                    Remove(i);
                }
            }
            else if (newLastShowIndex < _lastShowIndex)
            {
                for (var i = newLastShowIndex; i < _lastShowIndex; i++)
                {
                    Remove(i);
                }
            }

            for (var i = newFirstShowIndex; i <= newLastShowIndex; i++)
            {
                if (!_itemShowList.ContainsKey(i))
                {
                    var cover = new CoverFlowItem(i, PageIndex, BitmapFrame.Create(_uriList[i]));
                    _itemShowList[i] = cover;
                    _visualParent.Children.Add(cover);
                }
            }

            _firstShowIndex = newFirstShowIndex;
            _lastShowIndex = newLastShowIndex;
        }
    }
}