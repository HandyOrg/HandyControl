var target = Argument("target", "Build");

Task("Clean")
    .Does(() =>
{
    CleanDirectory("lib");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settingsNet40 = new DotNetCoreBuildSettings
    {
        Configuration = "Release-Net40",
        OutputDirectory = "lib/net40"
    };

    DotNetCoreBuild("../src/Net_40/HandyControl_Net_40/HandyControl_Net_40.csproj", settingsNet40);

    var frameworkList = new List<string> 
    { 
        "net45",
        "net451",
        "net452",
        "net46",
        "net461",
        "net462",
        "net47",
        "net471",
        "net472",
        "net48",
        "netcoreapp3.0",
        "netcoreapp3.1",
        "net5.0-windows",
        "net6.0-windows"
    };

    foreach (var framework in frameworkList)
    {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = "Release",
            Framework = framework,
            OutputDirectory = $"lib/{framework.Split('-')[0]}"
        };

        DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settings);
    }
});

RunTarget(target);