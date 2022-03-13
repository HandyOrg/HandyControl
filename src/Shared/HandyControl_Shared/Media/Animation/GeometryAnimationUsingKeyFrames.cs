using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools;

// ReSharper disable PossibleInvalidOperationException
namespace HandyControl.Media.Animation;

[ContentProperty("KeyFrames")]
public class GeometryAnimationUsingKeyFrames : GeometryAnimationBase, IKeyFrameAnimation, IAddChild
{
    private string[] _strings;

    public string[] Strings
    {
        get
        {
            if (_keyFrames == null || _keyFrames.Count == 0)
            {
                return null;
            }

            return _strings ??= Regex.Split(_keyFrames[0].Value.ToString(CultureInfo.InvariantCulture), RegexPatterns.DigitsPattern);
        }
    }

    private GeometryKeyFrameCollection _keyFrames;

    private ResolvedKeyFrameEntry[] _sortedResolvedKeyFrames;

    private bool _areKeyTimesValid;

    public GeometryAnimationUsingKeyFrames()
    {
        _areKeyTimesValid = true;
    }

    public new GeometryAnimationUsingKeyFrames Clone() => (GeometryAnimationUsingKeyFrames) base.Clone();

    public new GeometryAnimationUsingKeyFrames CloneCurrentValue() => (GeometryAnimationUsingKeyFrames) base.CloneCurrentValue();

    protected override bool FreezeCore(bool isChecking)
    {
        var canFreeze = base.FreezeCore(isChecking);

        canFreeze &= Freeze(_keyFrames, isChecking);

        if (canFreeze & !_areKeyTimesValid)
        {
            ResolveKeyTimes();
        }

        return canFreeze;
    }

    protected override void OnChanged()
    {
        _areKeyTimesValid = false;

        base.OnChanged();
    }

    protected override Freezable CreateInstanceCore() => new GeometryAnimationUsingKeyFrames();

    protected override void CloneCore(Freezable sourceFreezable)
    {
        var sourceAnimation = (GeometryAnimationUsingKeyFrames) sourceFreezable;
        base.CloneCore(sourceFreezable);

        CopyCommon(sourceAnimation, false);
    }

    protected override void CloneCurrentValueCore(Freezable sourceFreezable)
    {
        var sourceAnimation = (GeometryAnimationUsingKeyFrames) sourceFreezable;
        base.CloneCurrentValueCore(sourceFreezable);

        CopyCommon(sourceAnimation, true);
    }

    protected override void GetAsFrozenCore(Freezable source)
    {
        var sourceAnimation = (GeometryAnimationUsingKeyFrames) source;
        base.GetAsFrozenCore(source);

        CopyCommon(sourceAnimation, false);
    }

    protected override void GetCurrentValueAsFrozenCore(Freezable source)
    {
        var sourceAnimation = (GeometryAnimationUsingKeyFrames) source;
        base.GetCurrentValueAsFrozenCore(source);

        CopyCommon(sourceAnimation, true);
    }

    private void CopyCommon(GeometryAnimationUsingKeyFrames sourceAnimation, bool isCurrentValueClone)
    {
        _areKeyTimesValid = sourceAnimation._areKeyTimesValid;

        if (_areKeyTimesValid && sourceAnimation._sortedResolvedKeyFrames != null)
        {
            _sortedResolvedKeyFrames = (ResolvedKeyFrameEntry[]) sourceAnimation._sortedResolvedKeyFrames.Clone();
        }

        if (sourceAnimation._keyFrames != null)
        {
            if (isCurrentValueClone)
            {
                _keyFrames = (GeometryKeyFrameCollection) sourceAnimation._keyFrames.CloneCurrentValue();
            }
            else
            {
                _keyFrames = sourceAnimation._keyFrames.Clone();
            }

            OnFreezablePropertyChanged(null, _keyFrames);
        }
    }

    void IAddChild.AddChild(object child)
    {
        WritePreamble();

        if (child == null)
        {
            throw new ArgumentNullException(nameof(child));
        }

        AddChild(child);

        WritePostscript();
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual void AddChild(object child)
    {
        if (child is GeometryKeyFrame keyFrame)
        {
            KeyFrames.Add(keyFrame);
        }
        else
        {
            throw new ArgumentException("Animation_ChildMustBeKeyFrame", nameof(child));
        }
    }

    void IAddChild.AddText(string childText)
    {
        if (childText == null)
        {
            throw new ArgumentNullException(nameof(childText));
        }

        AddText(childText);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual void AddText(string childText) => throw new InvalidOperationException("Animation_NoTextChildren");

    protected override Geometry GetCurrentValueCore(Geometry defaultOriginValue, Geometry defaultDestinationValue, AnimationClock animationClock)
    {
        if (_keyFrames == null)
        {
            return defaultDestinationValue;
        }

        if (!_areKeyTimesValid)
        {
            ResolveKeyTimes();
        }

        if (_sortedResolvedKeyFrames == null)
        {
            return defaultDestinationValue;
        }

        var currentTime = animationClock.CurrentTime.Value;
        var keyFrameCount = _sortedResolvedKeyFrames.Length;
        var maxKeyFrameIndex = keyFrameCount - 1;

        double[] currentIterationValue;

        var currentResolvedKeyFrameIndex = 0;

        while (currentResolvedKeyFrameIndex < keyFrameCount && currentTime > _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex]._resolvedKeyTime)
        {
            currentResolvedKeyFrameIndex++;
        }

        while (currentResolvedKeyFrameIndex < maxKeyFrameIndex && currentTime == _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex + 1]._resolvedKeyTime)
        {
            currentResolvedKeyFrameIndex++;
        }

        if (currentResolvedKeyFrameIndex == keyFrameCount)
        {
            currentIterationValue = GetResolvedKeyFrameValue(maxKeyFrameIndex);
        }
        else if (currentTime == _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex]._resolvedKeyTime)
        {
            currentIterationValue = GetResolvedKeyFrameValue(currentResolvedKeyFrameIndex);
        }
        else
        {
            double currentSegmentProgress;
            double[] fromValue;

            if (currentResolvedKeyFrameIndex == 0)
            {
                AnimationHelper.DecomposeGeometryStr(defaultOriginValue.ToString(CultureInfo.InvariantCulture), out fromValue);

                currentSegmentProgress = currentTime.TotalMilliseconds / _sortedResolvedKeyFrames[0]._resolvedKeyTime.TotalMilliseconds;
            }
            else
            {
                var previousResolvedKeyFrameIndex = currentResolvedKeyFrameIndex - 1;
                var previousResolvedKeyTime = _sortedResolvedKeyFrames[previousResolvedKeyFrameIndex]._resolvedKeyTime;

                fromValue = GetResolvedKeyFrameValue(previousResolvedKeyFrameIndex);

                var segmentCurrentTime = currentTime - previousResolvedKeyTime;
                var segmentDuration = _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex]._resolvedKeyTime - previousResolvedKeyTime;

                currentSegmentProgress = segmentCurrentTime.TotalMilliseconds / segmentDuration.TotalMilliseconds;
            }

            currentIterationValue = GetResolvedKeyFrame(currentResolvedKeyFrameIndex).InterpolateValue(fromValue, currentSegmentProgress);
        }

        return AnimationHelper.ComposeGeometry(Strings, currentIterationValue);
    }

    protected sealed override Duration GetNaturalDurationCore(Clock clock) => new(LargestTimeSpanKeyTime);

    IList IKeyFrameAnimation.KeyFrames
    {
        get => KeyFrames;
        set => KeyFrames = (GeometryKeyFrameCollection) value;
    }

    public GeometryKeyFrameCollection KeyFrames
    {
        get
        {
            ReadPreamble();

            if (_keyFrames == null)
            {
                if (IsFrozen)
                {
                    _keyFrames = GeometryKeyFrameCollection.Empty;
                }
                else
                {
                    WritePreamble();

                    _keyFrames = new GeometryKeyFrameCollection();

                    OnFreezablePropertyChanged(null, _keyFrames);

                    WritePostscript();
                }
            }

            return _keyFrames;
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            WritePreamble();

            if (value != _keyFrames)
            {
                OnFreezablePropertyChanged(_keyFrames, value);
                _keyFrames = value;

                WritePostscript();
            }
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeKeyFrames()
    {
        ReadPreamble();

        return _keyFrames != null && _keyFrames.Count > 0;
    }

    private struct KeyTimeBlock
    {
        public int BeginIndex;
        public int EndIndex;
    }

    private double[] GetResolvedKeyFrameValue(int resolvedKeyFrameIndex) =>
        GetResolvedKeyFrame(resolvedKeyFrameIndex).Numbers;

    private GeometryKeyFrame GetResolvedKeyFrame(int resolvedKeyFrameIndex) =>
        _keyFrames[_sortedResolvedKeyFrames[resolvedKeyFrameIndex]._originalKeyFrameIndex];

    private TimeSpan LargestTimeSpanKeyTime
    {
        get
        {
            var hasTimeSpanKeyTime = false;
            var largestTimeSpanKeyTime = TimeSpan.Zero;

            if (_keyFrames != null)
            {
                var keyFrameCount = _keyFrames.Count;

                for (var index = 0; index < keyFrameCount; index++)
                {
                    var keyTime = _keyFrames[index].KeyTime;

                    if (keyTime.Type == KeyTimeType.TimeSpan)
                    {
                        hasTimeSpanKeyTime = true;

                        if (keyTime.TimeSpan > largestTimeSpanKeyTime)
                        {
                            largestTimeSpanKeyTime = keyTime.TimeSpan;
                        }
                    }
                }
            }

            return hasTimeSpanKeyTime ? largestTimeSpanKeyTime : TimeSpan.FromSeconds(1.0);
        }
    }

    private void ResolveKeyTimes()
    {
        var keyFrameCount = 0;

        if (_keyFrames != null)
        {
            keyFrameCount = _keyFrames.Count;
        }

        if (keyFrameCount == 0)
        {
            _sortedResolvedKeyFrames = null;
            _areKeyTimesValid = true;
            return;
        }

        _sortedResolvedKeyFrames = new ResolvedKeyFrameEntry[keyFrameCount];

        var index = 0;

        for (; index < keyFrameCount; index++)
        {
            _sortedResolvedKeyFrames[index]._originalKeyFrameIndex = index;
        }

        var duration = Duration;
        var calculationDuration = duration.HasTimeSpan ? duration.TimeSpan : LargestTimeSpanKeyTime;
        var maxKeyFrameIndex = keyFrameCount - 1;
        var unspecifiedBlocks = new List<KeyTimeBlock>();
        var hasPacedKeyTimes = false;

        index = 0;
        while (index < keyFrameCount)
        {
            // ReSharper disable once PossibleNullReferenceException
            var keyTime = _keyFrames[index].KeyTime;

            switch (keyTime.Type)
            {
                case KeyTimeType.Percent:

                    _sortedResolvedKeyFrames[index]._resolvedKeyTime = TimeSpan.FromMilliseconds(keyTime.Percent * calculationDuration.TotalMilliseconds);
                    index++;
                    break;

                case KeyTimeType.TimeSpan:

                    _sortedResolvedKeyFrames[index]._resolvedKeyTime = keyTime.TimeSpan;

                    index++;
                    break;

                case KeyTimeType.Paced:
                case KeyTimeType.Uniform:

                    if (index == maxKeyFrameIndex)
                    {
                        _sortedResolvedKeyFrames[index]._resolvedKeyTime = calculationDuration;
                        index++;
                    }
                    else if (index == 0 && keyTime.Type == KeyTimeType.Paced)
                    {
                        _sortedResolvedKeyFrames[index]._resolvedKeyTime = TimeSpan.Zero;
                        index++;
                    }
                    else
                    {
                        if (keyTime.Type == KeyTimeType.Paced)
                        {
                            hasPacedKeyTimes = true;
                        }

                        var block = new KeyTimeBlock
                        {
                            BeginIndex = index
                        };

                        while (++index < maxKeyFrameIndex)
                        {
                            var type = _keyFrames[index].KeyTime.Type;

                            if (type == KeyTimeType.Percent || type == KeyTimeType.TimeSpan)
                            {
                                break;
                            }

                            if (type == KeyTimeType.Paced)
                            {
                                hasPacedKeyTimes = true;
                            }
                        }

                        block.EndIndex = index;
                        unspecifiedBlocks.Add(block);
                    }

                    break;
            }
        }

        foreach (var block in unspecifiedBlocks)
        {
            var blockBeginTime = TimeSpan.Zero;

            if (block.BeginIndex > 0)
            {
                blockBeginTime = _sortedResolvedKeyFrames[block.BeginIndex - 1]._resolvedKeyTime;
            }

            long segmentCount = block.EndIndex - block.BeginIndex + 1;
            var uniformTimeStep = TimeSpan.FromTicks((_sortedResolvedKeyFrames[block.EndIndex]._resolvedKeyTime - blockBeginTime).Ticks / segmentCount);

            index = block.BeginIndex;
            var resolvedTime = blockBeginTime + uniformTimeStep;

            while (index < block.EndIndex)
            {
                _sortedResolvedKeyFrames[index]._resolvedKeyTime = resolvedTime;

                resolvedTime += uniformTimeStep;
                index++;
            }
        }

        if (hasPacedKeyTimes)
        {
            ResolvePacedKeyTimes();
        }

        Array.Sort(_sortedResolvedKeyFrames);

        _areKeyTimesValid = true;
    }

    private void ResolvePacedKeyTimes()
    {
        var index = 1;
        var maxKeyFrameIndex = _sortedResolvedKeyFrames.Length - 1;

        do
        {
            if (_keyFrames[index].KeyTime.Type == KeyTimeType.Paced)
            {
                var firstPacedBlockKeyFrameIndex = index;

                var segmentLengths = new List<double>();
                var prePacedBlockKeyTime = _sortedResolvedKeyFrames[index - 1]._resolvedKeyTime;
                var totalLength = 0.0;
                var prevKeyValue = _keyFrames[index - 1].Numbers;

                do
                {
                    var currentKeyValue = _keyFrames[index].Numbers;
                    totalLength += Math.Abs(currentKeyValue[0] - prevKeyValue[0]);
                    segmentLengths.Add(totalLength);
                    prevKeyValue = currentKeyValue;
                    index++;
                }
                while (index < maxKeyFrameIndex && _keyFrames[index].KeyTime.Type == KeyTimeType.Paced);
                totalLength += Math.Abs(_keyFrames[index].Numbers[0] - prevKeyValue[0]);

                var pacedBlockDuration = _sortedResolvedKeyFrames[index]._resolvedKeyTime - prePacedBlockKeyTime;

                for (int i = 0, currentKeyFrameIndex = firstPacedBlockKeyFrameIndex; i < segmentLengths.Count; i++, currentKeyFrameIndex++)
                {
                    _sortedResolvedKeyFrames[currentKeyFrameIndex]._resolvedKeyTime = prePacedBlockKeyTime + TimeSpan.FromMilliseconds(segmentLengths[i] / totalLength * pacedBlockDuration.TotalMilliseconds);
                }
            }
            else
            {
                index++;
            }
        }
        while (index < maxKeyFrameIndex);
    }
}
