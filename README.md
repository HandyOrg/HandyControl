![logo](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/icon.png)

![dotnet-version](https://img.shields.io/badge/.net-%3E%3D4.0-blue.svg) ![csharp-version](https://img.shields.io/badge/C%23-7.3-blue.svg) ![IDE-version](https://img.shields.io/badge/IDE-vs2019-blue.svg) [![nuget-version](https://img.shields.io/nuget/v/HandyControl.svg)](https://www.nuget.org/packages/HandyControl) [![build-status](https://ci.appveyor.com/api/projects/status/github/handyorg/handycontrol?svg=true)](https://ci.appveyor.com/project/handyorg/handycontrol) [![Join the chat at https://gitter.im/HandyControl/Lobby](https://badges.gitter.im/HandyControl/Lobby.svg)](https://gitter.im/HandyControl/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) [![qq-group](https://img.shields.io/badge/qq-714704041-red.svg)](//shang.qq.com/wpa/qunwpa?idkey=a571e5553c9d41e49c4f22f3a8b2865451497a795ff281fedf3285def247efc1)
[![wiki](https://img.shields.io/badge/wiki-Complete-brightgreen.svg)](https://github.com/ghost1372/HandyControl/wiki)

# Welcome to HandyControl

## We're all here

![qq-group](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/qq-group.png)

## 面向中国

### 中文文档（刚开始写）

[https://handyorg.github.io/handycontrol/](https://handyorg.github.io/handycontrol/)

### 捐赠
如果您觉得HandyControl还不错，并且刚好有些闲钱，那么可以选择以下两种方式来捐赠：

* [以HandyControl的名义为慈善事业做贡献](http://www.chinacharityfederation.org/ConfirmDonation/0.html?zhijie=3)  

* 为我们购买防脱洗发水  
![qrcode](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/qrcode.png)

## Special thanks to

[![JetBrains](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/resharper_logo.png)](https://www.jetbrains.com/?from=HandyControl)

## Usage

Step 1：Add a reference to HandyControl or search for HandyControl on the nuget; 

```Install-Package HandyControl```

Step 2：Add code in App.xaml as follows:
```XML
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/Theme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

Step 3: Add NameSpace:
`xmlns:hc="https://handyorg.github.io/handycontrol"`

Step 4：enjoy coding

## VSIX packages for Visual Studio

| [VS2019](https://marketplace.visualstudio.com/items?itemName=HandyOrg.handycontrolforvs2019) |
| ------------- |

## Overview

![Overview](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Overview.png)

![Overview-dark](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Overview-dark.png)

## Latest examples

### ImageBlock

![ImageBlock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ImageBlock.gif)  

![ImageBlock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ImageBlock.png)

### Magnifier

![Magnifier](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Magnifier.png)

### Card

![Card](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Card.png)

### ButtonGroup

![ButtonGroup](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ButtonGroup.png)

## History publication

### Grid

![Grid](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Grid.gif)

### SideMenu

![SideMenu](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/SideMenu.png)

### NotifyIcon

![NotifyIcon](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/NotifyIcon.png)

### Dialog

![Dialog](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Dialog.png)

### WaveProgressBar

![WaveProgressBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/WaveProgressBar.gif)

### Badge

![Badge](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Badge.png)

### Gravatar

![Gravatar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Gravatar.gif)

### GoToTop

![GoToTop](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/GoToTop.gif)

### ChatBubble

![ChatBubble](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ChatBubble.png)

### Label

![Label](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Label.png)

### Transfer

![Transfer](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Transfer.png)

### ProgressButton

![ProgressButton](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ProgressButton.png)

### CoverFlow

![CoverFlow](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CoverFlow.gif)

### CoverView

![CoverView](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CoverView.gif)

### MessageBox

![MessageBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/MessageBox.png)

### WaterfallPanel

![WaterfallPanel](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/WaterfallPanel.png)

### Rate

![Rate](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Rate.png)

### BlurWindow

![BlurWindow](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/BlurWindow.png)

### FlipClock

![FlipClock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/FlipClock.gif)

### Shield

![Shield](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Shield.png)

### OutlineText

![OutlineText](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/OutlineText.png)

### Tag

![Tag](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Tag.png)

### ToolBar

![ToolBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ToolBar.png)

### Slider

![Slider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Slider.png)

### CircleProgressBar

![CircleProgressBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CircleProgressBar.png)

### ButtonStyle

![ButtonStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Button.png)

### ToggleButtonStyle

![ToggleButtonStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ToggleButton.png)

### RadioButtonStyle

![RadioButtonStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/RadioButton.png)

### CheckBoxStyle

![CheckBoxStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CheckBox.png)

### ListBoxStyle

![ListBoxStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ListBox.png)

### TreeViewStyle

![TreeViewStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TreeView.png)

### ListViewStyle

![ListViewStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ListView.png)

### DataGrid

![DataGrid](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/DataGrid.png)

### Now you can switch to dark theme

![dark theme](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/DarkTheme.png)

### ColorPicker

![ColorPicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ColorPicker.gif)

### Loading

![Loading](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Loading.gif)

### Carousel

![Carousel](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Carousel.gif)

### Pagination

![Pagination](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Pagination.gif)

### Expander

![Expander](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Expander.gif)

### TimeBar

![TimeBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TimeBar.gif)

### ImageBrowser

![ImageBrowser](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ImageBrowser.gif)

### PreviewSlider

![PreviewSlider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/PreviewSlider.gif)

### CompareSlider

![CompareSlider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CompareSlider-h.gif)

![CompareSlider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CompareSlider-v.gif)

### Growl

![Growl](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Growl.gif)

### AnimationPath

![AnimationPath](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/AnimationPath.gif)

### ProgressBar

![ProgressBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ProgressBar.gif)

### TabControl

![TabControl](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TabControl.gif)

### TabControlStyle

![TabControlStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TabControl.png)

### GroupBox

![GroupBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/GroupBox.png)

### StepBar

![StepBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/StepBar.png)

### GifImage

![GifImage](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/GifImage.gif)

### ContextMenu

![ContextMenu](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ContextMenu.png)

### Calendar

![Calendar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Calendar.jpg)

### Clock

![Clock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Clock.jpg)

### CalendarWithClock

![CalendarWithClock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CalendarWithClock.png)

### TextBlock

![TextBlock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TextBlock.jpg)

### RichTextBoxStyle

![RichTextBoxStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/RichTextBox.png)

### TextBox

![TextBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TextBox.jpg)

### ComboBox

![ComboBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ComboBox.jpg)

### NumericUpDown

![NumericUpDown](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/NumericUpDown.png)

### SearchBar

![SearchBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/SearchBar.png)

### PasswordBox

![PasswordBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/PasswordBox.jpg)

### DataPicker

![DataPicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/DataPicker.jpg)

### TimePicker

![TimePicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TimePicker.jpg)

### DateTimePicker

![DateTimePicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/DateTimePicker.png)

### ScrollViewer

![ScrollViewer](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ScrollViewer.png)

### CirclePanel

![CirclePanel](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CirclePanel.jpg)

### BorderStyle

![BorderStyle](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Border.png)

### Brush

![Brush](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Brush.png)

## Switching configuration

![Switching configuration](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/SwitchConfig.png)
