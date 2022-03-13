using System;

namespace HandyControl.Media.Animation;

internal struct ResolvedKeyFrameEntry : IComparable
{
    internal int _originalKeyFrameIndex;

    internal TimeSpan _resolvedKeyTime;

    public int CompareTo(object other)
    {
        var otherEntry = (ResolvedKeyFrameEntry) other;

        if (otherEntry._resolvedKeyTime > _resolvedKeyTime)
        {
            return -1;
        }

        if (otherEntry._resolvedKeyTime < _resolvedKeyTime)
        {
            return 1;
        }

        if (otherEntry._originalKeyFrameIndex > _originalKeyFrameIndex)
        {
            return -1;
        }

        if (otherEntry._originalKeyFrameIndex < _originalKeyFrameIndex)
        {
            return 1;
        }

        return 0;
    }
}
