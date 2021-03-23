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

    var settingsNet45 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net45",
        OutputDirectory = "lib/net45"
    };

    var settingsNet451 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net451",
        OutputDirectory = "lib/net451"
    };

    var settingsNet452 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net452",
        OutputDirectory = "lib/net452"
    };

    var settingsNet46 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net46",
        OutputDirectory = "lib/net46"
    };

    var settingsNet461 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net461",
        OutputDirectory = "lib/net461"
    };

    var settingsNet462 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net462",
        OutputDirectory = "lib/net462"
    };

    var settingsNet47 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net47",
        OutputDirectory = "lib/net47"
    };

    var settingsNet471 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net471",
        OutputDirectory = "lib/net471"
    };

    var settingsNet472 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net472",
        OutputDirectory = "lib/net472"
    };

    var settingsNet48 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net48",
        OutputDirectory = "lib/net48"
    };

    var settingsCore30 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "netcoreapp3.0",
        OutputDirectory = "lib/netcoreapp3.0"
    };

    var settingsCore31 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "netcoreapp3.1",
        OutputDirectory = "lib/netcoreapp3.1"
    };

    var settingsNet50 = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        Framework = "net5.0-windows",
        OutputDirectory = "lib/net5.0"
    };

    DotNetCoreBuild("../src/Net_40/HandyControl_Net_40/HandyControl_Net_40.csproj", settingsNet40);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet45);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet451);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet452);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet46);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet461);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet462);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet47);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet471);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet472);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet48);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsCore30);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsCore31);
    DotNetCoreBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet50);
});

RunTarget(target);