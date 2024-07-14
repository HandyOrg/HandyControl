using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using HandyControl.Data;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace HandyControlDemo.Tools;

internal class HighlightingProvider
{
    private static readonly Lazy<HighlightingProvider> DefaultInternal = new(() => new HighlightingProvider());

    public static HighlightingProvider Default => DefaultInternal.Value;

    protected static readonly Lazy<IHighlightingDefinition> DefaultDefinition = new(() => HighlightingManager.Instance.GetDefinition("C#"));

    private static readonly Dictionary<SkinType, HighlightingProvider> Providers = new();

    protected Dictionary<string, Lazy<IHighlightingDefinition>> Definition;

    public static void Register(SkinType skinType, HighlightingProvider provider)
    {
        Providers[skinType] = provider ?? throw new ArgumentNullException(nameof(provider));
        provider.InitDefinitions();
    }

    public static IHighlightingDefinition GetDefinition(SkinType skinType, string name)
    {
        if (Providers.TryGetValue(skinType, out var provider))
        {
            return provider.GetDefinition(name);
        }

        return DefaultDefinition.Value;
    }

    protected static IHighlightingDefinition LoadDefinition(string xshdName)
    {
        var streamResourceInfo = Application.GetResourceStream(new Uri($"pack://application:,,,/Resources/xshd/{xshdName}.xshd"));
        if (streamResourceInfo == null)
        {
            return DefaultDefinition.Value;
        }

        using var reader = new XmlTextReader(streamResourceInfo.Stream);
        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
    }

    protected virtual IHighlightingDefinition GetDefinition(string name)
    {
        if (Definition.TryGetValue(name, out var definition))
        {
            return definition.Value;
        }

        return DefaultDefinition.Value;
    }

    protected virtual void InitDefinitions()
    {
        Definition = new Dictionary<string, Lazy<IHighlightingDefinition>>
        {
            ["XML"] = new(() => HighlightingManager.Instance.GetDefinition("XML")),
            ["C#"] = new(() => DefaultDefinition.Value)
        };
    }
}

internal class HighlightingProviderDark : HighlightingProvider
{
    protected override void InitDefinitions()
    {
        Definition = new Dictionary<string, Lazy<IHighlightingDefinition>>
        {
            ["XML"] = new(() => LoadDefinition("XML-Dark")),
            ["C#"] = new(() => LoadDefinition("CSharp-Dark")),
        };
    }
}
