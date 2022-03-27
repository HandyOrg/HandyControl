using System;

namespace HandyControl.Data;

internal class GifFrameDimension
{
    public GifFrameDimension(Guid guid) => Guid = guid;

    public Guid Guid { get; }

    public static GifFrameDimension Time { get; } = new(new Guid("{6aedbd6d-3fb5-418a-83a6-7f45229dc872}"));

    public static GifFrameDimension Resolution { get; } = new(new Guid("{84236f7b-3bd3-428f-8dab-4ea1439ca315}"));

    public static GifFrameDimension Page { get; } = new(new Guid("{7462dc86-6180-4c7e-8e3f-ee7333a7a483}"));

    public override bool Equals(object o) => o is GifFrameDimension format && Guid == format.Guid;

    public override int GetHashCode() => Guid.GetHashCode();

    public override string ToString()
    {
        if (Equals(this, Time)) return "Time";
        if (Equals(this, Resolution)) return "Resolution";
        if (Equals(this, Page)) return "Page";
        return "[FrameDimension: " + Guid + "]";
    }
}
