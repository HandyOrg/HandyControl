./tools/nuget pack build.nuspec -Symbols -SymbolPackageFormat snupkg
Get-ChildItem -Path ./build.Lang.*.nuspec | ForEach-Object {
    ./tools/nuget pack $_
}
