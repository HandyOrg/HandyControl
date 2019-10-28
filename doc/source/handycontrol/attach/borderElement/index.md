---
title: BorderElement 具有边框的元素
---

# 准备工作

添加`HandyControl`命名空间

```xml
xmlns:hc="https://handyorg.github.io/handycontrol"
```

# 相关属性

| 名称 | 用途 |
|-|-|
| CornerRadius | 设置边框圆角值 |
| Circular | 是否呈现为圆形 True为是、False为否 |

# 使用案例

## CornerRadius 设置边框圆角值

```xml
<Button Content="[Button]测试CornerRadius" hc:BorderElement.CornerRadius="0"></Button>
<hc:TextBox Text="[hc:TextBox]测试CornerRadius" hc:BorderElement.CornerRadius="15"></hc:TextBox>
<TextBox Text="[TextBox]测试CornerRadius" hc:BorderElement.CornerRadius="0"></TextBox>
<Button Content="[Button]测试CornerRadius" hc:BorderElement.CornerRadius="0,0,4,4"></Button>
```

![CorberRadius_Case_01](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach\borderelment_case_01.png)

## Circular 是否呈现为圆形

借助`BorderElement.Circular`附加属性实现圆形Border
```xml
<Border Style="{StaticResource BorderCircular}" Background="OrangeRed" Width="100" Height="100"/>
```
![borderElement_Case_02](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach\borderElement_Case_02.png)

# 温馨提示

## 常见控件支持项

**所属**  是指该控件为`WPF默认`控件还是为`HandControl自定义`实现的控件
**适用属性 ** 为当前控件`BorderElement`对应有效的附加属性

| 控件类型 | 所属                  | 适用属性     |
| -------- | --------------------- | ------------ |
|Button   | WPF默认               | CornerRadius |
|DatePicker|WPF默认|  CornerRadius|
|ProgressBar| WPF默认|CornerRadus|
|RadioButton|WPF默认|CornerRadus|
|ToggleButton|WPF默认|CornerRadus|
|RepeatButton|WPF默认|CornerRadus|
|ToolBar|WPF默认|CornerRadus|
|**Border**|WPF默认|**Circular**|
|GroupBox|WPF默认(部分Style)|CornerRadus|
|TextBox  | WPF默认、HandyControl | CornerRadius |
|PasswordBox| WPF默认、HandyControl|CornerRadus|
|ComboBox| WPF默认、HandyControl|CornerRadus|
|DateTimePicker|HandyControl|CornerRadus|
|GotoTop|HandyControl|CornerRadus|
|Gravatar|HandyControl|CornerRadus|
|NumericUpDown|HandyControl|CornerRadus|
|ProgressButton| HandyControl|CornerRadus|
|RunningBlock|HandyControl|CornerRadus|
|SearchBar|HandyControl|CornerRadus|
|SplitButton|HandyControl|CornerRadus|
|TabItem|HandyControl|CornerRadus|
|Growl|HandyControl|CornerRadus|
|Shield|HandyControl|CornerRadus(暂时无效)|
|Tag|HandyControl|CornerRadus(暂时无效)|


