using System;

namespace Standard;

internal static class CLSID
{
    public static T CoCreateInstance<T>(string clsid)
    {
        return (T) ((object) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(clsid))));
    }

    public const string TaskbarList = "56FDF344-FD6D-11d0-958A-006097C9A090";

    public const string EnumerableObjectCollection = "2d3468c1-36a7-43b6-ac24-d3f02fd9607a";

    public const string ShellLink = "00021401-0000-0000-C000-000000000046";

    public const string DestinationList = "77f10cf0-3db5-4966-b520-b7c54fd35ed6";

    public const string ApplicationDestinations = "86c14003-4d6b-4ef3-a7b4-0506663b2e68";

    public const string ApplicationDocumentLists = "86bec222-30f2-47e0-9f25-60d11cd75c28";
}
