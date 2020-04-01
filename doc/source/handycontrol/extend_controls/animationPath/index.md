---
title: AnimationPath 动画路径
---

`AnimationPath`动画路径，设定`Geometry `派生类型几何图形的数据，例如`PathGeometry `、`LineGeometry `等，实现固定的路径动画效果。

```xml
public class AnimationPath : Shape
```

# 属性

## Data 数据

动画路径需要显示的几何图形数据。

```xml
<Geometry x:Key="GithubGeometry">M512 0C229.12 0 0 229.12 0 512c0 226.56 146.56 417.92 350.08 485.76 25.6 4.48 35.2-10.88 35.2-24.32 0-12.16-0.64-52.48-0.64-95.36-128.64 23.68-161.92-31.36-172.16-60.16-5.76-14.72-30.72-60.16-52.48-72.32-17.92-9.6-43.52-33.28-0.64-33.92 40.32-0.64 69.12 37.12 78.72 52.48 46.08 77.44 119.68 55.68 149.12 42.24 4.48-33.28 17.92-55.68 32.64-68.48-113.92-12.8-232.96-56.96-232.96-252.8 0-55.68 19.84-101.76 52.48-137.6-5.12-12.8-23.04-65.28 5.12-135.68 0 0 42.88-13.44 140.8 52.48 40.96-11.52 84.48-17.28 128-17.28 43.52 0 87.04 5.76 128 17.28 97.92-66.56 140.8-52.48 140.8-52.48 28.16 70.4 10.24 122.88 5.12 135.68 32.64 35.84 52.48 81.28 52.48 137.6 0 196.48-119.68 240-233.6 252.8 18.56 16 34.56 46.72 34.56 94.72 0 68.48-0.64 123.52-0.64 140.8 0 13.44 9.6 29.44 35.2 24.32A512.832 512.832 0 0 0 1024 512c0-282.88-229.12-512-512-512z</Geometry>
```
案例：
```xml
<Grid Width="100" Height="100">
    <hc:AnimationPath Data="{DynamicResource GithubGeometry}" 
                          Duration="00:00:05" 
                          Stretch="Uniform" StrokeThickness="1"
                          Stroke="Black"></hc:AnimationPath>
</Grid>
```

效果：

![Animation-Default](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/AnimationPath-Default.gif)

## PathLength 路径长度

默认为0，设置属性为50，效果如下：

![AnimationPath-PathLength](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/AnimationPath-PathLength.gif)

## Duration 时间间隔

路径动画的执行时间间隔，此属性为必须设置项，否则动画路径效果无显示。

## IsPlaying 是否正在播放动画

判定当前动画播放状态，常作为控件内部行为控制判定标识，可通过该属性进行路径动画的暂停控制。

## RepeatBehavior 重复行为

设置动画重复行为，默认为`Forever`，可根据需要设置重复行为。

## 继承至Shape常用属性

| 属性名称        | 用途     |
| --------------- | -------- |
| Stretch         | 拉伸属性 |
| StrokeThickness | 边线宽度 |

# 相关事件

## Completed

当动画完成时，触发此事件。