// reference from https://github.com/sourcechord/FluentWPF/blob/master/FluentWPF/Utility/VersionInfo.cs
// LICENSE: https://github.com/sourcechord/FluentWPF/blob/master/LICENSE

using System;

namespace HandyControl.Data;

public readonly struct SystemVersionInfo
{
    public static SystemVersionInfo Windows10 => new(10, 0, 10240);
    public static SystemVersionInfo Windows10_1809 => new(10, 0, 17763);
    public static SystemVersionInfo Windows10_1903 => new(10, 0, 18362);
    public static SystemVersionInfo Windows11_22H2 => new(10, 0, 22621);

    public SystemVersionInfo(int major, int minor, int build)
    {
        Major = major;
        Minor = minor;
        Build = build;
    }

    public int Major { get; }

    public int Minor { get; }

    public int Build { get; }

    public bool Equals(SystemVersionInfo other) => Major == other.Major && Minor == other.Minor && Build == other.Build;

    public override bool Equals(object obj) => obj is SystemVersionInfo other && Equals(other);

    public override int GetHashCode() => Major.GetHashCode() ^ Minor.GetHashCode() ^ Build.GetHashCode();

    public static bool operator ==(SystemVersionInfo left, SystemVersionInfo right) => left.Equals(right);

    public static bool operator !=(SystemVersionInfo left, SystemVersionInfo right) => !(left == right);

    public int CompareTo(SystemVersionInfo other)
    {
        if (Major != other.Major)
        {
            return Major.CompareTo(other.Major);
        }

        if (Minor != other.Minor)
        {
            return Minor.CompareTo(other.Minor);
        }

        if (Build != other.Build)
        {
            return Build.CompareTo(other.Build);
        }

        return 0;
    }

    public int CompareTo(object obj)
    {
        if (!(obj is SystemVersionInfo other))
        {
            throw new ArgumentException();
        }

        return CompareTo(other);
    }

    public static bool operator <(SystemVersionInfo left, SystemVersionInfo right) => left.CompareTo(right) < 0;

    public static bool operator <=(SystemVersionInfo left, SystemVersionInfo right) => left.CompareTo(right) <= 0;

    public static bool operator >(SystemVersionInfo left, SystemVersionInfo right) => left.CompareTo(right) > 0;

    public static bool operator >=(SystemVersionInfo left, SystemVersionInfo right) => left.CompareTo(right) >= 0;

    public override string ToString() => $"{Major}.{Minor}.{Build}";
}
