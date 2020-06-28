using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    public class AnimationPath : Shape
    {
        /// <summary>
        ///     故事板
        /// </summary>
        private Storyboard _storyboard;

        /// <summary>
        ///     路径长度
        /// </summary>
        private double _pathLength;

        /// <summary>
        ///     路径
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data),
            typeof(Geometry), typeof(AnimationPath), new FrameworkPropertyMetadata(null,
                OnPropertiesChanged));

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimationPath path)
            {
                path.UpdatePath();
            }
        }

        /// <summary>
        ///     路径
        /// </summary>
        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Geometry DefiningGeometry => Data ?? Geometry.Empty;

        /// <summary>
        ///     路径长度
        /// </summary>
        public static readonly DependencyProperty PathLengthProperty = DependencyProperty.Register(
            "PathLength", typeof(double), typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, OnPropertiesChanged));

        /// <summary>
        ///     路径长度
        /// </summary>
        public double PathLength
        {
            get => (double)GetValue(PathLengthProperty);
            set => SetValue(PathLengthProperty, value);
        }

        /// <summary>
        ///     动画间隔时间
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration", typeof(Duration), typeof(AnimationPath), new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2)),
                OnPropertiesChanged));

        /// <summary>
        ///     动画间隔时间
        /// </summary>
        public Duration Duration
        {
            get => (Duration)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
            "IsPlaying", typeof(bool), typeof(AnimationPath), new FrameworkPropertyMetadata(true, (o, args) =>
            {
                var ctl = (AnimationPath)o;
                var v = (bool)args.NewValue;
                if (v)
                {
                    ctl.UpdatePath();
                }
                else
                {
                    ctl._storyboard?.Pause();
                }
            }));

        /// <summary>
        ///     是否正在播放动画
        /// </summary>
        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty RepeatBehaviorProperty = DependencyProperty.Register(
            "RepeatBehavior", typeof(RepeatBehavior), typeof(AnimationPath), new PropertyMetadata(RepeatBehavior.Forever));

        /// <summary>
        ///     动画重复行为
        /// </summary>
        public RepeatBehavior RepeatBehavior
        {
            get => (RepeatBehavior)GetValue(RepeatBehaviorProperty);
            set => SetValue(RepeatBehaviorProperty, value);
        }

        static AnimationPath()
        {
            StretchProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(Stretch.Uniform,
                OnPropertiesChanged));

            StrokeThicknessProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.Double1Box,
                OnPropertiesChanged));
        }

        public AnimationPath() => Loaded += (s, e) => UpdatePath();

        /// <summary>
        ///     动画完成事件
        /// </summary>
        public static readonly RoutedEvent CompletedEvent =
            EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(AnimationPath));

        /// <summary>
        ///     动画完成事件
        /// </summary>
        public event EventHandler Completed
        {
            add => AddHandler(CompletedEvent, value);
            remove => RemoveHandler(CompletedEvent, value);
        }

        /// <summary>
        ///     更新路径
        /// </summary>
        private void UpdatePath()
        {
            if (!Duration.HasTimeSpan || !IsPlaying) return;

            _pathLength = PathLength > 0 ? PathLength : Data.GetTotalLength(new Size(ActualWidth, ActualHeight), StrokeThickness);

            if (MathHelper.IsVerySmall(_pathLength)) return;

            StrokeDashOffset = _pathLength;
            StrokeDashArray = new DoubleCollection(new List<double>
            {
                _pathLength,
                _pathLength
            });

            //定义动画
            if (_storyboard != null)
            {
                _storyboard.Stop();
                _storyboard.Completed -= Storyboard_Completed;
            }
            _storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior
            };
            _storyboard.Completed += Storyboard_Completed;

            var frames = new DoubleAnimationUsingKeyFrames();
            //开始位置
            var frame0 = new LinearDoubleKeyFrame
            {
                Value = _pathLength,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
            };
            //结束位置
            var frame1 = new LinearDoubleKeyFrame
            {
                Value = -_pathLength,
                KeyTime = KeyTime.FromTimeSpan(Duration.TimeSpan)
            };
            frames.KeyFrames.Add(frame0);
            frames.KeyFrames.Add(frame1);

            Storyboard.SetTarget(frames, this);
            Storyboard.SetTargetProperty(frames, new PropertyPath(StrokeDashOffsetProperty));
            _storyboard.Children.Add(frames);

            _storyboard.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e) => RaiseEvent(new RoutedEventArgs(CompletedEvent));
    }
}