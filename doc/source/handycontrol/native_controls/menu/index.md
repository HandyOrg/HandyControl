---
title: Menu 菜单
---

# MenuBaseStyle

`HandyControl`中的菜单默认样式，一般不建议直接使用，而是选择继承，便于个人自定义风格

样式

```xml
    <Style BasedOn="{StaticResource MenuItemBaseStyle}" TargetType="MenuItem"/>
    <Style BasedOn="{StaticResource MenuBaseStyle}" TargetType="Menu"/>
```

使用

```
<Menu ItemsSource="{Binding Menus}">
        <Menu.ItemTemplate>
            <HierarchicalDataTemplate ItemsSource="{Binding Children}">
                <TextBlock Text="{Binding Name}"></TextBlock>
            </HierarchicalDataTemplate>
        </Menu.ItemTemplate>
</Menu>
```

数据视图

```c#
    //菜单子项视图类
    public class MenuItemViewModel
    {
        public List<MenuItemViewModel> Children { get; set; }
        public MenuItemViewModel Parent { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
    }
    //菜单数据视图类
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            Menus = new List<MenuItemViewModel>
            {
               new MenuItemViewModel
               {
                 Name="一级菜单",
                 Children =  new List<MenuItemViewModel>{
                  new MenuItemViewModel{
                    Name="二级菜单"
                  }
                 }
               },
               new MenuItemViewModel
               {
                 Name="一级菜单",
                 Children =  new List<MenuItemViewModel>{
                  new MenuItemViewModel{
                    Name="二级菜单"
                  }
                 }
               },
               new MenuItemViewModel
               {
                 Name="一级菜单",
                 Children =  new List<MenuItemViewModel>{
                  new MenuItemViewModel{
                    Name="二级菜单"
                  }
                 }
               }
            };
        }
        public List<MenuItemViewModel> Menus { get; set; }
    }
```

效果

![Menu.BaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Menu.BaseStyle.png)

