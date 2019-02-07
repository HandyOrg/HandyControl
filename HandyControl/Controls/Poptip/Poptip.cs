using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace HandyControl.Controls
{
    public enum EnumPlacement
    {
        /// <summary>
        /// 左上
        /// </summary>
        LeftTop,
        /// <summary>
        /// 左中
        /// </summary>
        LeftCenter,
        /// <summary>
        /// 左下
        /// </summary>
        LeftBottom,
        /// <summary>
        /// 右上
        /// </summary>
        RightTop,
        /// <summary>
        /// 右中
        /// </summary>
        RightCenter,
        /// <summary>
        /// 右下
        /// </summary>
        RightBottom,
        /// <summary>
        /// 上左
        /// </summary>
        TopLeft,
        /// <summary>
        /// 上中
        /// </summary>
        TopCenter,
        /// <summary>
        /// 上右
        /// </summary>
        TopRight,
        /// <summary>
        /// 下左
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 下中
        /// </summary>
        BottomCenter,
        /// <summary>
        /// 下右
        /// </summary>
        BottomRight,
    }
    /// <summary>
    /// 气泡提示控件
    /// </summary>
    public class Poptip : Popup
    {
        #region private fields

        private bool mIsLoaded = false;
        private AngleBorder angleBorder;

        #endregion

        #region DependencyProperty

        #region PlacementEx

        public EnumPlacement PlacementEx
        {
            get { return (EnumPlacement)GetValue(PlacementExProperty); }
            set { SetValue(PlacementExProperty, value); }
        }

        public static readonly DependencyProperty PlacementExProperty =
            DependencyProperty.Register("PlacementEx", typeof(EnumPlacement), typeof(Poptip)
                , new PropertyMetadata(EnumPlacement.TopLeft, PlacementExChangedCallback));

        private static void PlacementExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Poptip poptip = d as Poptip;
            if (poptip != null)
            {
                EnumPlacement placement = (EnumPlacement)e.NewValue;
                switch (placement)
                {
                    case EnumPlacement.LeftTop:
                        break;
                    case EnumPlacement.LeftBottom:
                        break;
                    case EnumPlacement.LeftCenter:
                        break;
                    case EnumPlacement.RightTop:
                        break;
                    case EnumPlacement.RightBottom:
                        break;
                    case EnumPlacement.RightCenter:
                        break;
                    case EnumPlacement.TopLeft:
                        break;
                    case EnumPlacement.TopCenter:
                        poptip.Placement = PlacementMode.Top;
                        break;
                    case EnumPlacement.TopRight:
                        break;
                    case EnumPlacement.BottomLeft:
                        break;
                    case EnumPlacement.BottomCenter:
                        break;
                    case EnumPlacement.BottomRight:
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Background

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(Poptip), new PropertyMetadata(Brushes.White));

        #endregion

        #region BorderThickness

        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Poptip), new PropertyMetadata(new Thickness(1)));

        #endregion

        #region BorderBrush

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Poptip), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(204, 206, 219))));

        #endregion

        #region CornerRadius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Poptip), new PropertyMetadata(new CornerRadius(5)));

        #endregion

        #endregion

        #region Override

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.AllowsTransparency = true;
            //this.StaysOpen = false;

            UIElement element = this.Child;
            this.Child = null;

            Grid root = new Grid()
            {
                Margin = new Thickness(10),
            };

            angleBorder = new AngleBorder()
            {
                Background = this.Background,
                CornerRadius = this.CornerRadius,
                BorderThickness = this.BorderThickness,
                BorderBrush = this.BorderBrush,
            };
            switch (this.PlacementEx)
            {
                case EnumPlacement.LeftTop:
                    angleBorder.Placement = EnumPlacement.RightTop;
                    break;
                case EnumPlacement.LeftBottom:
                    angleBorder.Placement = EnumPlacement.RightBottom;
                    break;
                case EnumPlacement.LeftCenter:
                    angleBorder.Placement = EnumPlacement.RightCenter;
                    break;
                case EnumPlacement.RightTop:
                    angleBorder.Placement = EnumPlacement.LeftTop;
                    break;
                case EnumPlacement.RightBottom:
                    angleBorder.Placement = EnumPlacement.LeftBottom;
                    break;
                case EnumPlacement.RightCenter:
                    angleBorder.Placement = EnumPlacement.LeftCenter;
                    break;
                case EnumPlacement.TopLeft:
                    angleBorder.Placement = EnumPlacement.BottomLeft;
                    break;
                case EnumPlacement.TopCenter:
                    angleBorder.Placement = EnumPlacement.BottomCenter;
                    break;
                case EnumPlacement.TopRight:
                    angleBorder.Placement = EnumPlacement.BottomRight;
                    break;
                case EnumPlacement.BottomLeft:
                    angleBorder.Placement = EnumPlacement.TopLeft;
                    break;
                case EnumPlacement.BottomCenter:
                    angleBorder.Placement = EnumPlacement.TopCenter;
                    break;
                case EnumPlacement.BottomRight:
                    angleBorder.Placement = EnumPlacement.TopRight;
                    break;
                default:
                    break;
            }
            
            //在原有控件基础上，最外层套一个AngleBorder
            angleBorder.Child = element;

            root.Children.Add(angleBorder);

            this.Child = root;
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            if (this.mIsLoaded)
            {
                return;
            }

            FrameworkElement targetElement = this.PlacementTarget as FrameworkElement;
            FrameworkElement child = this.Child as FrameworkElement;

            if (targetElement == null || child == null) return;

            switch (this.PlacementEx)
            {
                case EnumPlacement.LeftTop:
                    this.Placement = PlacementMode.Left;
                    break;
                case EnumPlacement.LeftBottom:
                    this.Placement = PlacementMode.Left;
                    this.VerticalOffset = targetElement.ActualHeight - child.ActualHeight;
                    break;
                case EnumPlacement.LeftCenter:
                    this.Placement = PlacementMode.Left;
                    this.VerticalOffset = this.GetOffset(targetElement.ActualHeight, child.ActualHeight);
                    break;
                case EnumPlacement.RightTop:
                    this.Placement = PlacementMode.Right;
                    break;
                case EnumPlacement.RightBottom:
                    this.Placement = PlacementMode.Right;
                    this.VerticalOffset = targetElement.ActualHeight - child.ActualHeight;
                    break;
                case EnumPlacement.RightCenter:
                    this.Placement = PlacementMode.Right;
                    this.VerticalOffset = this.GetOffset(targetElement.ActualHeight, child.ActualHeight);
                    break;
                case EnumPlacement.TopLeft:
                    this.Placement = PlacementMode.Top;
                    break;
                case EnumPlacement.TopCenter:
                    this.Placement = PlacementMode.Top;
                    this.HorizontalOffset = this.GetOffset(targetElement.ActualWidth, child.ActualWidth);
                    break;
                case EnumPlacement.TopRight:
                    this.Placement = PlacementMode.Top;
                    this.HorizontalOffset = targetElement.ActualWidth - child.ActualWidth;
                    break;
                case EnumPlacement.BottomLeft:
                    this.Placement = PlacementMode.Bottom;
                    break;
                case EnumPlacement.BottomCenter:
                    this.Placement = PlacementMode.Bottom;
                    this.HorizontalOffset = this.GetOffset(targetElement.ActualWidth, child.ActualWidth);
                    break;
                case EnumPlacement.BottomRight:
                    this.Placement = PlacementMode.Bottom;
                    this.HorizontalOffset = targetElement.ActualWidth - child.ActualWidth;
                    break;
            }
            this.mIsLoaded = true;
        }

        #endregion

        #region private function

        private double GetOffset(double targetSize, double poptipSize)
        {
            if (double.IsNaN(targetSize) || double.IsNaN(poptipSize))
            {
                return 0;
            }
            return (targetSize / 2.0) - (poptipSize / 2.0);
        }

        #endregion
    }
}
