---
title: FlowDocument 流文档
---

# FlowDocumentScrollViewerBaseStyle

流文档滚动视图默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

案例：

``` xml
<FlowDocumentScrollViewer IsToolBarVisible="True" Width="502" Height="400" Margin="32">
            <FlowDocument ColumnWidth="400" IsOptimalParagraphEnabled="True" IsHyphenationEnabled="True">
                <Section FontSize="12">
                    <Paragraph>
                        <Bold>Neptune</Bold> (planet), major planet in the solar system, eighth planet
                        from the Sun and fourth largest in diameter.  Neptune maintains an almost constant
                        distance, about 4,490 million km (about 2,790 million mi), from the Sun.  Neptune
                        revolves outside the orbit of Uranus and for most of its orbit moves inside the
                        elliptical path of the outermost planet Pluto (see Solar System). Every 248 years,
                        Pluto’s elliptical orbit brings the planet inside Neptune’s nearly circular orbit
                        for about 20 years, temporarily making Neptune the farthest planet from the Sun.
                        The last time Pluto’s orbit brought it inside Neptune’s orbit was in 1979. In
                        1999 Pluto’s orbit carried it back outside Neptune’s orbit.
                        <Figure Width="140" Height="50" Background="GhostWhite" HorizontalAnchor="PageLeft" HorizontalOffset="100" VerticalOffset="20">
                            <Paragraph FontStyle="Italic" TextAlignment="Left" Background="Beige" Foreground="DarkGreen">
                                Neptune has 72 times Earth's volume...
                            </Paragraph>
                        </Figure>
                        <Floater Background="GhostWhite" Width="285" HorizontalAlignment="Left">
                            <Table CellSpacing="5">
                                <Table.Columns>
                                    <TableColumn Width="155"/>
                                    <TableColumn Width="130"/>
                                </Table.Columns>
                                <TableRowGroup>
                                    <TableRow>
                                        <TableCell ColumnSpan="3">
                                            <Paragraph>Neptune Stats</Paragraph>
                                        </TableCell>
                                    </TableRow>
                                    <TableRow Background="LightGoldenrodYellow" FontSize="12">
                                        <TableCell>
                                            <Paragraph FontWeight="Bold">Mean Distance from Sun</Paragraph>
                                        </TableCell>
                                        <TableCell>
                                            <Paragraph>4,504,000,000 km</Paragraph>
                                        </TableCell>
                                    </TableRow>
                                    <TableRow FontSize="12" Background="LightGray">
                                        <TableCell>
                                            <Paragraph FontWeight="Bold">Mean Diameter</Paragraph>
                                        </TableCell>
                                        <TableCell>
                                            <Paragraph>49,532 km</Paragraph>
                                        </TableCell>
                                    </TableRow>
                                    <TableRow Background="LightGoldenrodYellow" FontSize="12">
                                        <TableCell>
                                            <Paragraph FontWeight="Bold">Approximate Mass</Paragraph>
                                        </TableCell>
                                        <TableCell>
                                            <Paragraph>1.0247e26 kg</Paragraph>
                                        </TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell ColumnSpan="4">
                                            <Paragraph FontSize="10" FontStyle="Italic">
                                                Information from the
                                                <Hyperlink NavigateUri="http://encarta.msn.com/encnet/refpages/artcenter.aspx">Encarta</Hyperlink>
                                                web site.
                                            </Paragraph>
                                        </TableCell>
                                    </TableRow>
                                </TableRowGroup>
                            </Table>
                        </Floater>
                    </Paragraph>
                    <Paragraph>
                        Astronomers believe Neptune has an inner rocky core that is surrounded by a vast
                        ocean of water mixed with rocky material. From the inner core, this ocean extends
                        upward until it meets a gaseous atmosphere of hydrogen, helium, and trace amounts
                        of methane. Neptune has four rings and 11 known moons. Even though Neptune's volume
                        is 72 times Earth’s volume, its mass is only 17.15 times Earth’s mass. Because of
                        its size, scientists classify Neptune—along with Jupiter, Saturn, and Uranus—as
                        one of the giant or Jovian planets (so-called because they resemble Jupiter).
                    </Paragraph>
                    <Paragraph>
                        <Figure Width="140" Height="50" Background="GhostWhite" TextAlignment="Left" HorizontalAnchor="PageCenter" WrapDirection="Both">
                            <Paragraph FontStyle="Italic" Background="Beige" Foreground="DarkGreen" >
                                Neptune has an orbital period of ~20 years...
                            </Paragraph>
                        </Figure>
                        Mathematical theories of astronomy led to the discovery of Neptune. To account for
                        wobbles in the orbit of the planet Uranus, British astronomer John Couch Adams and
                        French astronomer Urbain Jean Joseph Leverrier independently calculated the existence
                        and position of a new planet in 1845 and 1846, respectively. They theorized that the
                        gravitational attraction of this planet for Uranus was causing the wobbles in Uranus’s
                        orbit. Using information from Leverrier, German astronomer Johann Gottfried Galle first
                        observed the planet in 1846.
                    </Paragraph>
                </Section>
            </FlowDocument>
        </FlowDocumentScrollViewer>
```

效果：

![FlowDocumentScrollViewer](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/FlowDocumentScrollViewer.png)

# FlowDocumentPageViewerBaseStyle

流文档单页视图默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

# FlowDocumentReaderBaseStyle

流文档查看器默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。