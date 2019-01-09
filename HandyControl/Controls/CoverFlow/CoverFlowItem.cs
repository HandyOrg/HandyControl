using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class CoverFlowItem : ModelVisual3D
    {
        internal static readonly double Interval = .2;

        internal static readonly double AnimationSpeed = 400;

        private readonly AxisAngleRotation3D _rotation3D;

        private readonly TranslateTransform3D _transform3D;

        private readonly Model3DGroup _model3DGroup;

        private readonly ImageSource _source;

        internal int Index { get; set; }

        public CoverFlowItem(int itemIndex, int currentIndex, ImageSource source)
        {
            Index = itemIndex;
            _source = source;

            _rotation3D = new AxisAngleRotation3D(new Vector3D(0, 1, 0), GetAngleByPos(currentIndex));
            _transform3D = new TranslateTransform3D(GetXByPos(currentIndex), 0, GetZByPos(currentIndex));

            var transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(new RotateTransform3D(_rotation3D));
            transformGroup.Children.Add(_transform3D);

            _model3DGroup = new Model3DGroup();
            _model3DGroup.Children.Add(new GeometryModel3D(CreateItemGeometry(), new DiffuseMaterial(new ImageBrush(_source))));
            _model3DGroup.Transform = transformGroup;

            Content = _model3DGroup;
        }

        /// <summary>
        ///     创建网格形状
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        private static MeshGeometry3D CreateMeshGeometry(Point3D p0, Point3D p1, Point3D p2, Point3D p3)
        {
            var positions = new Point3DCollection
            {
                p0,
                p1,
                p2,
                p3
            };

            var triangleIndices = new Int32Collection
            {
                0,
                1,
                2,
                2,
                3,
                0
            };

            var textureCoordinates = new PointCollection
            {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1)
            };

            var normals = new Vector3DCollection
            {
                ArithmeticHelper.CalNormal(p0, p1, p2),
                ArithmeticHelper.CalNormal(p2, p3, p0)
            };

            var geometry3D = new MeshGeometry3D
            {
                Positions = positions,
                TriangleIndices = triangleIndices,
                TextureCoordinates = textureCoordinates,
                Normals = normals
            };

            geometry3D.Freeze();
            return geometry3D;
        }

        /// <summary>
        ///     创建内容形状
        /// </summary>
        /// <returns></returns>
        private Geometry3D CreateItemGeometry()
        {
            var sx = _source.Width > _source.Height ? 0 : 1 - _source.Width / _source.Height;
            var sy = _source.Width < _source.Height ? 0 : 1 - _source.Height / _source.Width;

            var p0 = new Point3D(-1 + sx, -1 + sy, 0);
            var p1 = new Point3D(1 - sx, -1 + sy, 0);
            var p2 = new Point3D(1 - sx, 1 - sy, 0);
            var p3 = new Point3D(-1 + sx, 1 - sy, 0);

            return CreateMeshGeometry(p0, p1, p2, p3);
        }

        private double GetAngleByPos(int index) => Math.Sign(Index - index) * -90;

        private double GetXByPos(int index)
        {
            Console.WriteLine(Index * Interval + Math.Sign(Index - index) * 1.5);
            return Index * Interval + Math.Sign(Index - index) * 1.5;
        }

        private double GetZByPos(int index) => Index == index ? 1 : 0;

        /// <summary>
        ///     移动
        /// </summary>
        internal void Move(int currentIndex, bool animated)
        {
            if (animated || _rotation3D.HasAnimatedProperties)
            {
                _rotation3D.BeginAnimation(AxisAngleRotation3D.AngleProperty,
                    AnimationHelper.CreateAnimation(GetAngleByPos(currentIndex), AnimationSpeed));
                _transform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty,
                    AnimationHelper.CreateAnimation(GetXByPos(currentIndex), AnimationSpeed));
                _transform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty,
                    AnimationHelper.CreateAnimation(GetZByPos(currentIndex), AnimationSpeed));
            }
            else
            {
                _rotation3D.Angle = GetAngleByPos(currentIndex);
                _transform3D.OffsetX = GetXByPos(currentIndex);
                _transform3D.OffsetZ = GetZByPos(currentIndex);
            }
        }

        /// <summary>
        ///     命中测试
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        internal bool HitTest(MeshGeometry3D mesh)
        {
            foreach (var item in _model3DGroup.Children)
            {
                if (item is GeometryModel3D geometryModel3D)
                {
                    if (Equals(geometryModel3D.Geometry, mesh))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}