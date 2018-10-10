![logo](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/icon.png)

![csharp-version](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/csharp-version.png) ![IDE-version](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/IDE-version.png) [![nuget-version](https://img.shields.io/nuget/v/HandyControl.svg)](https://www.nuget.org/packages/HandyControl)  [![build-status](https://ci.appveyor.com/api/projects/status/github/NaBian/handycontrol?svg=true)](https://ci.appveyor.com/project/NaBian/handycontrol) [![Join the chat at https://gitter.im/HandyControl/Lobby](https://badges.gitter.im/HandyControl/Lobby.svg)](https://gitter.im/HandyControl/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

# Welcome to HandyControl

## Latest examples

No latest examples

## History publication

### 1、ColorPicker

![ColorPicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ColorPicker.gif)

### 2、Loading

![Loading](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Loading.gif)

### 3、Carousel

![Carousel](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Carousel.gif)

### 4、Pagination

![Pagination](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Pagination.gif)

### 5、Expander

![Expander](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Expander.gif)

### 6、TimeBar

![TimeBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TimeBar.gif)

### 7、ImageBrowser

![ImageBrowser](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ImageBrowser.gif)

### 8、CompareSlider

![CompareSlider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CompareSlider-h.gif)

![CompareSlider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CompareSlider-v.gif)

### 9、Growl

![Growl](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Growl.gif)

### 10、AnimationPath

![AnimationPath](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/AnimationPath.gif)

### 11、ProgressBar

![ProgressBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ProgressBar.gif)

### 12、TabControl

![TabControl](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TabControl.gif)

### 13、StepBar

![StepBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/StepBar.gif)

### 14、Calendar

![Calendar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Calendar.jpg)

### 15、Clock

![Clock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Clock.jpg)

### 16、TextBlock

![TextBlock](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TextBlock.jpg)

### 17、TextBox

![TextBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TextBox.jpg)

### 18、ComboBox

![ComboBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ComboBox.jpg)

### 19、PasswordBox

![PasswordBox](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/PasswordBox.jpg)

### 20、DataPicker

![DataPicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/DataPicker.jpg)

### 21、TimePicker

![TimePicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TimePicker.jpg)

### 22、CirclePanel

![CirclePanel](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CirclePanel.jpg)

# Usage

Step 1：Add a reference to HandyControl or search for HandyControl on the nuget;  
Step 2：Add code in App.xaml as follows:
```XML
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/ThemesDefault.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```
Step 3：enjoy coding

# FAQ
* How to make **Scrollviewer** inertial? 
```XML
<controls:ScrollViewer IsEnableInertia="True">
    
</controls:ScrollViewer>
```

* How to make **Scrollviewer** Penetrating?
```XML
<controls:ScrollViewer IsPenetrating="True">
    
</controls:ScrollViewer>
```

# v1.4.0 Plan
- Partial control refactoring
- add Dark Theme