param($installPath, $toolsPath, $package, $project)

# find the App.xaml file 
$appxaml = $project.ProjectItems | where {$_.Name -eq "App.xaml"}
$appxamlPath = ($appxaml.Properties | where {$_.Name -eq "LocalPath"}).Value

# find the CSPROJ file 
$projectPath = ($project.Properties | where {$_.Name -eq "LocalPath"}).Value + $project.Name + ".csproj"
$namespace = $project.Properties.Item("RootNamespace").Value


$sharedFolderPath = $null

if ($appxamlPath -eq $null)
{
	# Might be a universal project, try it
	$pathElements = $projectPath.Split('\')
	$appxamlPath = ""
	$lastElement = ""

	for ($i = 0; $i -le $pathElements.Length - 3; $i++)
	{
		$sharedFolderPath += $pathElements[$i] + "/"
		$appxamlPath += $pathElements[$i] + "/"
		$lastElement = $pathElements[$i]
	}
	
	$appxamlPath += $lastElement  + ".Shared/App.xaml"
	
#	[System.Windows.MessageBox]::Show("New appxamlPath " + $appxamlPath, 'TEST', 'OK')

	if (Test-Path $appxamlPath)
	{
#		[System.Windows.MessageBox]::Show("UNIVERSAL", 'TEST', 'OK')
	}
	else
	{
		$appxamlPath = $null
	}
}

if ($appxamlPath -eq $null)
{
	# add the required .NET assembly:
	Add-Type -AssemblyName PresentationFramework 
	[System.Windows.MessageBox]::Show('Cannot find App.xaml in this project, no other changes made. If you are installing in a PCL, please use "MVVM Light Libs Only" instead.', 'Warning', 'OK')
}
else
{
	$projectXml = New-Object xml
	$projectXml.Load($projectPath)

	$propertyGroups = $projectXml.SelectNodes("//*") | where { $_.Name -eq "PropertyGroup" }

	$found = "Nothing"

	# load App.xaml as XML 
	$appXamlXml = New-Object xml 
	$appXamlXml.Load($appxamlPath)

	$appresources = $appXamlXml.SelectNodes("//*") | where { $_.Name -eq "Application.Resources" }
	$resources = $null
	$resourcesDictionary = $null
	if ($appresources -eq $null)
	{
#		[System.Windows.MessageBox]::Show("No resources found", 'TEST', 'OK')

		$app = $appXamlXml.SelectNodes("//*") | where { $_.Name -eq "Application" }
		
		if ($app -eq $null)
		{
#			[System.Windows.MessageBox]::Show("No application node found", 'TEST', 'OK')
			break
		}

		$appresources = $appXamlXml.CreateNode("element", "Application.Resources", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
		$resources = $appXamlXml.CreateNode("element", "ResourceDictionary", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
		$resourcesDictionary = $appXamlXml.CreateNode("element", "ResourceDictionary.MergedDictionaries","http://schemas.microsoft.com/winfx/2006/xaml/presentation")
		$resourcesElementSkin = $appXamlXml.CreateNode("element", "ResourceDictionary", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
		$resourcesElementSkin.SetAttribute("Source", $null, "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml")
		$resourcesElementTheme = $appXamlXml.CreateNode("element", "ResourceDictionary", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
		$resourcesElementTheme.SetAttribute("Source", $null, "pack://application:,,,/HandyControl;component/Themes/Theme.xaml")

		$resourcesDictionary.AppendChild($resourcesElementSkin)
		$resourcesDictionary.AppendChild($resourcesElementTheme)
		$resources.AppendChild($resourcesDictionary)
		$appresources.AppendChild($resources)		
				
		$app.AppendChild($appresources)
	}
	else
	{
		$resources = $appresources.SelectNodes("//*") | where { $_.Name -eq "ResourceDictionary" }
		
		if ($resources -eq $null)
		{
			$resources = $appXamlXml.CreateNode("element", "ResourceDictionary", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
			$resourcesDictionary = $appXamlXml.CreateNode("element", "ResourceDictionary.MergedDictionaries","http://schemas.microsoft.com/winfx/2006/xaml/presentation")
			$resourcesElementSkin = $appXamlXml.CreateNode("element", "ResourceDictionary", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
			$resourcesElementSkin.SetAttribute("Source", $null, "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml")
			$resourcesElementTheme = $appXamlXml.CreateNode("element", "ResourceDictionary", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
			$resourcesElementTheme.SetAttribute("Source", $null, "pack://application:,,,/HandyControl;component/Themes/Theme.xaml")

			$resourcesDictionary.AppendChild($resourcesElementSkin)
			$resourcesDictionary.AppendChild($resourcesElementTheme)
			$resources.AppendChild($resourcesDictionary)
			$appresources.AppendChild($resources)		
		}
	}

	$app = $appXamlXml.ChildNodes | where { $_.Name -eq "Application" }

	$appXamlXml.Save($appxamlPath)

}
$DTE.ItemOperations.Navigate("https://handyorg.github.io/")