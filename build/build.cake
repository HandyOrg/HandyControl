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
    var vsLatest = VSWhereLatest();
    var msBuildPath = vsLatest?.CombineWithFilePath("./MSBuild/Current/Bin/MSBuild.exe");
    NuGetRestore("../src/Net_40/HandyControl_Net_40/HandyControl_Net_40.csproj");
    
    var settingsNet40 = new MSBuildSettings
    {
        Configuration = "Release",
        ToolPath = msBuildPath,
    }.WithProperty("OutputPath", MakeAbsolute(Directory("lib/net40")).FullPath);

    var settingsNet45 = new MSBuildSettings
    {
        Configuration = "Release-Net45",
        ToolPath = msBuildPath,
    }.WithProperty("OutputPath", MakeAbsolute(Directory("lib/net45")).FullPath);

    var settingsNet462 = new MSBuildSettings
    {
        Configuration = "Release-Net462",
        ToolPath = msBuildPath,
    }.WithProperty("OutputPath", MakeAbsolute(Directory("lib/net462")).FullPath);

    var settingsNet47 = new MSBuildSettings
    {
        Configuration = "Release-Net47",
        ToolPath = msBuildPath,
    }.WithProperty("OutputPath", MakeAbsolute(Directory("lib/net47")).FullPath);

    var settingsNet48 = new MSBuildSettings
    {
        Configuration = "Release-Net48",
        ToolPath = msBuildPath,
    }.WithProperty("OutputPath", MakeAbsolute(Directory("lib/net48")).FullPath);

    var settingsCore30 = new DotNetCoreBuildSettings
    {
        Framework = "netcoreapp3.0",
        Configuration = "Release-Core30",
        OutputDirectory = "lib/netcoreapp3.0"
    };

    var settingsCore31 = new DotNetCoreBuildSettings
    {
        Framework = "netcoreapp3.1",
        Configuration = "Release-Core31",
        OutputDirectory = "lib/netcoreapp3.1"
    };

    MSBuild("../src/Net_40/HandyControl_Net_40/HandyControl_Net_40.csproj", settingsNet40);
    MSBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet45);
    MSBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet462);
    MSBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet47);
    MSBuild("../src/Net_GE45/HandyControl_Net_GE45/HandyControl_Net_GE45.csproj", settingsNet48);
    DotNetCoreBuild("../src/Core_GE30/HandyControl_Core_GE30/HandyControl_Core_GE30.csproj", settingsCore30);
    DotNetCoreBuild("../src/Core_GE30/HandyControl_Core_GE30/HandyControl_Core_GE30.csproj", settingsCore31);
});

RunTarget(target);