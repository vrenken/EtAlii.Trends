// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Navigations;

//
// using System.Collections.ObjectModel;
// using Syncfusion.Blazor.Diagram;
//
public partial class TimeMap
{
    private ToolbarItem? _zoomInItem;
    private string? _zoomInItemCssClass;
    private ToolbarItem? _zoomOutItem;
    private string? _zoomOutItemCssClass;

    private ToolbarItem? _panItem;
    private string? _panItemCssClass;
    private ToolbarItem? _pointerItem;
    private string? _pointerItemCssClass;

    private ToolbarItem? _viewItem;
    private string? _viewCssClass;
    private bool _view;

    private ToolbarItem? _centerItem;
    private string? _centerCssClass;
    private bool _center;

    private ToolbarItem? _fitToPageItem;
    private string? _fitCssClass;

    private InteractionController _diagramTool;

    private int _horizontalSpacing = 0;
    private int _verticalSpacing = 0;
    private LayoutOrientation _orientationType = LayoutOrientation.TopToBottom;
    private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Stretch;
    private VerticalAlignment _verticalAlignment = VerticalAlignment.Stretch;

    private ScrollLimitMode _scrollLimit = ScrollLimitMode.Infinity;

    private LayoutType _type = LayoutType.HierarchicalTree;

    private double _left;
    private double _top;
    private double _right;
    private double _bottom;

    //Defines sfdiagramComponent
#pragma warning disable CS8618
    private SfDiagramComponent _diagram;
#pragma warning restore CS8618
    //Defines diagram constraints
    // private DiagramConstraints? constraints;
    //Defines diagrams's nodes collection
    private readonly DiagramObjectCollection<Node> _nodes = new();
    //Defines diagrams's connectors collection
    private readonly DiagramObjectCollection<Connector> _connectors = new();

}
