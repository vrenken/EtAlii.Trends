// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Pages;

using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Navigations;

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

#pragma warning disable CS8618
    private SfDiagramComponent _diagram;
#pragma warning restore CS8618
    //Defines diagram constraints
    // private DiagramConstraints? constraints;
    //Defines diagrams nodes collection
    //private readonly DiagramObjectCollection<Node> _nodes = new();
    //Defines diagrams connectors collection
    //private readonly DiagramObjectCollection<Connector> _connectors = new();


    private void OnNodeCreating(IDiagramObject diagramObject)
    {
        var node = (Node)diagramObject;
        node.Width = 180;
        node.Height = 53;
        node.Shape = new Shape { Type = Shapes.HTML };
    }

    //Initialize Data Source
    private readonly List<HierarchicalDetails> _dataSource = new()
    {
        // ReSharper disable StringLiteralTypo
        new HierarchicalDetails ( Name:"Maria Anders", Designation:"Managing Director", EmployeeID:"SF10001", TeamName:"Web-Diagram", TeamSize:"10", ParentID : "", BG: "images/diagram/people/people-circle14.png" ),
        new HierarchicalDetails ( Name:"Ana Trujillo", Designation:"Project Manager ", EmployeeID:"SF10002", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10001", BG: "images/diagram/people/people-circle12.png" ),
        new HierarchicalDetails ( Name:"Patricio Simpson" ,Designation:"Project Lead", EmployeeID:"SF10003", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10002", BG: "images/diagram/people/people-circle18.png" ),
        new HierarchicalDetails ( Name:"Aria Cruz" ,Designation:"Project Lead", EmployeeID:"SF10004", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10003", BG: "images/diagram/people/people-circle0.png" ),
        new HierarchicalDetails ( Name:"Antonio Moreno", Designation:"Project Lead", EmployeeID:"SF10005", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10004", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails ( Name:"Thomas Hardy", Designation:"Senior S/w Engg", EmployeeID:"SF10006", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10005", BG: "images/diagram/people/people-circle29.png" ),
        new HierarchicalDetails ( Name:"Christina Berglund" ,Designation:"S/w Engg", EmployeeID:"SF10007", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10006", BG: "images/diagram/people/people-circle12.png" ),
        new HierarchicalDetails ( Name:"Hanna Moos", Designation:"Project Trainee", EmployeeID:"SF10008", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10007", BG: "images/diagram/people/people-circle27.png" ),
        new HierarchicalDetails ( Name:"Frédérique Citeaux", Designation:"Project Trainee", EmployeeID:"SF100050", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10007", BG: "images/diagram/people/people-circle1.png" ),
        new HierarchicalDetails ( Name:"Martín Sommer" ,Designation:"Senior S/w Engg",EmployeeID:"SF10009", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10006", BG: "images/diagram/people/people-circle25.png" ),
        new HierarchicalDetails ( Name:"Maria Larsson" ,Designation:"Senior S/w Engg", EmployeeID:"SF100010", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10004", BG: "images/diagram/people/people-circle11.png" ),
        new HierarchicalDetails ( Name:"Isabel de Castro", Designation:"Project Manager", EmployeeID:"SF100011", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10001", BG: "images/diagram/people/people-circle1.png" ),
        new HierarchicalDetails ( Name:"Horst Batista", Designation:"Project Lead", EmployeeID:"SF100012", TeamName:"Web-Diagram", TeamSize:"10", ParentID : "SF100021", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails ( Name:"Lúcia Carvalho" ,Designation:"Senior S/w Engg", EmployeeID:"SF100013", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100012", BG: "images/diagram/people/people-circle27.png" ),
        new HierarchicalDetails ( Name:"José Pedro Freyre" ,Designation:"Senior S/w Engg", EmployeeID:"SF100014", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100013", BG: "images/diagram/people/people-circle21.png" ),
        new HierarchicalDetails ( Name:"André Fonseca", Designation:"Senior S/w Engg", EmployeeID:"SF100015", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100014", BG: "images/diagram/people/people-circle5.png" ),
        new HierarchicalDetails ( Name:"Paula Wilson", Designation:"S/w Engg", EmployeeID:"SF100016", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100015", BG: "images/diagram/people/people-circle0.png" ),
        new HierarchicalDetails ( Name:"John Bergulfsen", Designation:"Project Trainee", EmployeeID:"SF100017", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100015", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails ( Name:"Jose Pavarotti", Designation:"S/w Engg", EmployeeID:"SF100018", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100015", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails ( Name:"Miguel Angel Paolino" ,Designation:"Project Trainee", EmployeeID:"SF100019", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100015", BG: "images/diagram/people/people-circle14.png" ),
        new HierarchicalDetails ( Name:"Horst Kloss", Designation:"Project Trainee",EmployeeID:"SF100020", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100015", BG: "images/diagram/people/people-circle1.png" ),
        new HierarchicalDetails ( Name:"Wilson Holz" ,Designation:"Project Lead", EmployeeID:"SF100021", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100011", BG: "images/diagram/people/people-circle12.png" ),
        new HierarchicalDetails ( Name:"Jytte Petersen" ,Designation:"Project Manager", EmployeeID:"SF100022", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF10001", BG: "images/diagram/people/people-circle2.png" ),
        new HierarchicalDetails ( Name:"Liz Nixon" ,Designation:"Project Lead", EmployeeID:"SF100023", TeamName:"Web-Diagram", TeamSize:"10", ParentID : "SF100022", BG: "images/diagram/people/people-circle26.png" ),
        new HierarchicalDetails ( Name:"Liu Wong" ,Designation:"Senior S/w Engg", EmployeeID:"SF100024", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100023", BG: "images/diagram/people/people-circle29.png" ),
        new HierarchicalDetails ( Name:"Dominique Perrier" ,Designation:"Project Lead", EmployeeID:"SF100025", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100023", BG: "images/diagram/people/people-circle10.png" ),
        new HierarchicalDetails ( Name:"Annette Roulet" ,Designation:"Senior S/w Engg", EmployeeID:"SF100026", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100025", BG: "images/diagram/people/people-circle4.png" ),
        new HierarchicalDetails ( Name:"Carlos González", Designation:"SR", EmployeeID:"SF100027", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100026", BG: "images/diagram/people/people-circle1.png" ),
        new HierarchicalDetails ( Name:"Felipe Izquierdo" ,Designation:"SR", EmployeeID:"SF100028", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100027", BG: "images/diagram/people/people-circle21.png" ),
        new HierarchicalDetails ( Name:"Yoshi Tannamuri", Designation:"S/w Engg", EmployeeID:"SF100029", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100028", BG: "images/diagram/people/people-circle5.png" ),
        new HierarchicalDetails ( Name:"Fran Wilson", Designation:"SR", EmployeeID:"SF100030", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100028", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails ( Name:"Jean Fresnière", Designation:"S/w Engg", EmployeeID:"SF100031", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100028", BG: "images/diagram/people/people-circle27.png" ),
        new HierarchicalDetails ( Name:"Giovanni Rovelli", Designation:"SR", EmployeeID:"SF100032", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100028", BG: "images/diagram/people/people-circle26.png" ),
        new HierarchicalDetails ( Name:"Renate Messner", Designation:"Project Trainee", EmployeeID:"SF100033", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100028", BG: "images/diagram/people/people-circle11.png" ),
        new HierarchicalDetails ( Name:"Jaime Yorres" ,Designation:"Project Trainee", EmployeeID:"SF100034", TeamName:"Web-Diagram", TeamSize:"10", ParentID : "SF100028", BG: "images/diagram/people/people-circle27.png" ),
        new HierarchicalDetails ( Name:"John Steel", Designation:"Project Trainee", EmployeeID:"SF100035", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100027", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails (  Name:"Yvonne Moncada", Designation:"Project Trainee", EmployeeID:"SF100036", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100037", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails ( Name:"Michael Suyama", Designation:"S/w Engg", EmployeeID:"SF100037", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100024", BG: "images/diagram/people/people-circle27.png" ),
        new HierarchicalDetails ( Name:"Alexander Feuer", Designation:"Project Trainee", EmployeeID:"SF100038", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100037", BG: "images/diagram/people/people-circle1.png" ),
        new HierarchicalDetails ( Name:"Art Braunschweiger" ,Designation:"Project Trainee", EmployeeID:"SF100039", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100037", BG: "images/diagram/people/people-circle21.png" ),
        new HierarchicalDetails ( Name:"Pascale Cartrain", Designation:"Project Trainee", EmployeeID:"SF100040", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100037", BG: "images/diagram/people/people-circle10.png" ),
        new HierarchicalDetails ( Name:"Mary Saveley", Designation:"Project Manager" , EmployeeID:"SF100041", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"", BG: "images/diagram/people/people-circle25.png" ),
        new HierarchicalDetails ( Name:"Paul Henriot", Designation:"Project Lead",EmployeeID:"SF100042", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100041", BG: "images/diagram/people/people-circle21.png" ),
        new HierarchicalDetails ( Name:"Rita Müller" ,Designation:"Project Trainee", EmployeeID:"SF100043", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100044", BG: "images/diagram/people/people-circle23.png" ),
        new HierarchicalDetails ( Name:"Pirkko Koskitalo", Designation:"Senior S/w Engg", EmployeeID:"SF100044", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100042", BG: "images/diagram/people/people-circle8.png" ),
        new HierarchicalDetails ( Name:"Karl Jablonski", Designation:"Senior S/w Engg" , EmployeeID:"SF100045", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100044", BG: "images/diagram/people/people-circle17.png" ),
        new HierarchicalDetails ( Name:"Paula Parente" ,Designation:"Project Lead", EmployeeID:"SF100046",TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100048", BG: "images/diagram/people/people-circle5.png" ),
        new HierarchicalDetails ( Name:"John Camino" ,Designation:"Senior S/w Engg", EmployeeID:"SF100047", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100013", BG: "images/diagram/people/people-circle21.png" ),
        new HierarchicalDetails ( Name:"Matti Karttunen", Designation:"Project Lead", EmployeeID:"SF100048", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100041", BG: "images/diagram/people/people-circle0.png" ),
        new HierarchicalDetails ( Name:"Nancy" ,Designation:"Senior S/w Engg", EmployeeID:"SF100049", TeamName:"Web-Diagram", TeamSize:"10", ParentID :"SF100044", BG: "images/diagram/people/people-circle29.png" ),
        // ReSharper restore StringLiteralTypo
    };
}
