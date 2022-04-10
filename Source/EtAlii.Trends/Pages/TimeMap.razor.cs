// // Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends
//
// namespace EtAlii.Trends.Pages;
//
// using System.Collections.ObjectModel;
// using Syncfusion.Blazor.Diagram;
//
// public partial class TimeMap
// {
//     public static List<MindMapDetails> DataSource = new();
//     private ScrollLimitMode ScrollLimit { get; set; } = ScrollLimitMode.Diagram;
//     private SfDiagramComponent _diagram;
//
//     private InteractionController interactionController = InteractionController.SingleSelect;
//     private int _verticalSpacing = 20;
//     private int _horizontalSpacing = 80;
//     private int? _horizontalValue = 80;
//     private int? _verticalValue = 20;
//     private readonly DiagramSelectionSettings _selectionSettings = new();
//     private readonly DiagramObjectCollection<UserHandle> _handles = new();
//
//     private BranchType GetBranch(IDiagramObject obj)
//     {
//         var branch = ((obj as Node).Data as MindMapDetails).Branch;
//         return branch;
//     }
//     private void OnCreated()
//     {
//         _diagram.Select(new ObservableCollection<IDiagramObject> { _diagram.Nodes[0] });
//     }
//     // private void OnHorizontalSpaceChange(Syncfusion.Blazor.Inputs.ChangeEventArgs<int?> args)
//     // {
//     //     _horizontalValue = (int)args.Value;
//     //     _horizontalSpacing = int.Parse(args.Value.ToString());
//     // }
//     // private void OnVerticalSpaceChange(Syncfusion.Blazor.Inputs.ChangeEventArgs<int?> args)
//     // {
//     //     _verticalValue = (int)args.Value;
//     //     _verticalSpacing = int.Parse(args.Value.ToString());
//     // }
//
//     // Method to customize the tool
//     private InteractionControllerBase GetCustomTool(Actions action, string id)
//     {
//         InteractionControllerBase tool;
//         if (id == "AddLeft")
//         {
//             tool = new AddRightTool(_diagram);
//         }
//         else if(id == "AddRight")
//         {
//             tool = new AddLeftTool(_diagram);
//         }
//         else
//         {
//             tool = new DeleteTool(_diagram);
//         }
//         return tool;
//     }
//     // Custom tool to add the node.
//     public static async Task UpdatePortConnection(MindMapDetails childNode,SfDiagramComponent diagram)
//     {
//         var node = new Node
//         {
//             Data = childNode,
//         };
//         var connector = new Connector
//         {
//             TargetID = node.ID,
//             SourceID = diagram.SelectionSettings.Nodes[0].ID
//         };
//         await diagram
//             .AddDiagramElements(new DiagramObjectCollection<NodeBase> { node, connector })
//             .ConfigureAwait(false);
//         var sourceNode = diagram.GetObject(connector.SourceID) as Node;
//         if (diagram.GetObject(connector.TargetID) is Node targetNode && targetNode.Data != null)
//         {
//             var nodeInfo = targetNode.Data as MindMapDetails;
//             if (nodeInfo.Branch == BranchType.Right || nodeInfo.Branch == BranchType.SubRight)
//             {
//                 connector.SourcePortID = sourceNode.Ports[0].ID;
//                 connector.TargetPortID = targetNode.Ports[1].ID;
//             }
//             else if (nodeInfo.Branch == BranchType.Left || nodeInfo.Branch == BranchType.SubLeft)
//             {
//                 connector.SourcePortID = sourceNode.Ports[1].ID;
//                 connector.TargetPortID = targetNode.Ports[0].ID;
//             }
//         }
//         await diagram.DoLayout().ConfigureAwait(false);
//     }
//     // Custom tool to add the node.
//
//     public static void RemoveData(Node node,SfDiagramComponent diagram)
//     {
//         var mindMapDetails = (MindMapDetails)node.Data;
//         if(node.OutEdges.Count>0)
//         {
//             for(var i=0;i< node.OutEdges.Count;i++)
//             {
//                 var connector = diagram.GetObject(node.OutEdges[i]) as Connector;
//                 var targetNode = diagram.GetObject(connector.TargetID) as Node;
//                 if(targetNode.OutEdges.Count > 0)
//                 {
//                     RemoveData(targetNode, diagram);
//                 }
//                 else
//                 {
//                     DataSource.Remove(mindMapDetails);
//                 }
//             }
//             DataSource.Remove(mindMapDetails);
//         }
//         else
//         {
//             DataSource.Remove(mindMapDetails);
//         }
//     }
//     private void OnSelectionChanging(SelectionChangingEventArgs args)
//     {
//         if (args.NewValue.Count > 0)
//         {
//             if (args.NewValue[0] is Node node && node.Data != null)
//             {
//                 var mindMapDetails = (MindMapDetails)node.Data;
//                 var type = mindMapDetails.Branch;
//                 if (type == BranchType.Root)
//                 {
//                     _selectionSettings.UserHandles[0].Visible = false;
//                     _selectionSettings.UserHandles[1].Visible = false;
//                     _selectionSettings.UserHandles[2].Visible = true;
//                     _selectionSettings.UserHandles[3].Visible = true;
//                 }
//                 else if(type==BranchType.Left||type==BranchType.SubLeft)
//                 {
//                     _selectionSettings.UserHandles[0].Visible = false;
//                     _selectionSettings.UserHandles[1].Visible = true;
//                     _selectionSettings.UserHandles[2].Visible = true;
//                     _selectionSettings.UserHandles[3].Visible = false;
//                 }
//                 else if (type == BranchType.Right || type == BranchType.SubRight)
//                 {
//                     _selectionSettings.UserHandles[0].Visible = true;
//                     _selectionSettings.UserHandles[1].Visible = false;
//                     _selectionSettings.UserHandles[2].Visible = false;
//                     _selectionSettings.UserHandles[3].Visible = true;
//                 }
//             }
//         }
//     }
//     private void NodeCreating(IDiagramObject obj)
//     {
//         var node = obj as Node;
//         //node.Constraints = NodeConstraints.Default & ~NodeConstraints.Drag;
//         if (node.Data is MindMapDetails mindMapDetails)
//         {
//             if (node.Data is System.Text.Json.JsonElement)
//             {
//                 node.Data = System.Text.Json.JsonSerializer.Deserialize<MindMapDetails>(node.Data.ToString());
//             }
//         }
//         else
//         {
//             mindMapDetails = null;
//         }
//         node.Height = 50;
//         node.Width = 100;
//         node.Shape = new BasicShape { Type = Shapes.Basic, Shape = BasicShapeType.Ellipse };
//         var port21 = new PointPort
//         {
//             ID = "left",
//             Offset = new DiagramPoint { X = 0, Y = 0.5 },
//             Height = 10,
//             Width = 10,
//         };
//         var port22 = new PointPort
//         {
//             ID = "right",
//             Offset = new DiagramPoint { X = 1, Y = 0.5 },
//             Height = 10,
//             Width = 10,
//         };
//         if (node.Data != null)
//         {
//             node.Style.Fill = mindMapDetails.Fill;
//             node.Style.StrokeColor = mindMapDetails.Fill;
//             node.Ports = new DiagramObjectCollection<PointPort>
//             {
//                 port21,port22
//             };
//         }
//         var name = "";
//         if (mindMapDetails != null)
//         {
//             name = mindMapDetails.Label;
//         }
//         else
//         {
//             name = "New Child";
//         }
//         node.Annotations = new DiagramObjectCollection<ShapeAnnotation>
//         {
//             new()
//             {
//                 Content = name,
//                 Style=new TextStyle {Color="White",FontSize = 12,FontFamily="Segoe UI"},
//                 Offset=new DiagramPoint {X=0.5,Y=0.5}
//             }
//         };
//         node.Constraints &= ~NodeConstraints.Rotate;
//     }
//
//     private void ConnectorCreating(IDiagramObject connector) => ConnectorCreating((Connector)connector);
//
//     private void ConnectorCreating(Connector connector)
//     {
//         connector.Type = ConnectorSegmentType.Bezier;
//         connector.Constraints = ConnectorConstraints.Default & ~ConnectorConstraints.Select;
//         connector.Style = new ShapeStyle { StrokeColor = "#4f4f4f", StrokeWidth = 1 };
//         connector.TargetDecorator = new DecoratorSettings { Shape = DecoratorShape.None };
//         connector.SourceDecorator.Shape = DecoratorShape.None;
//         var sourceNode = (Node)_diagram.GetObject(connector.SourceID);
//         if (_diagram.GetObject(connector.TargetID) is Node targetNode && targetNode.Data != null)
//         {
//             var nodeInfo = (MindMapDetails)targetNode.Data;
//             if (nodeInfo.Branch == BranchType.Right || nodeInfo.Branch == BranchType.SubRight)
//             {
//                 connector.SourcePortID = sourceNode.Ports[0].ID;
//                 connector.TargetPortID = targetNode.Ports[1].ID;
//             }
//             else if (nodeInfo.Branch == BranchType.Left || nodeInfo.Branch == BranchType.SubLeft)
//             {
//                 connector.SourcePortID = sourceNode.Ports[1].ID;
//                 connector.TargetPortID = targetNode.Ports[0].ID;
//             }
//         }
//     }
//     private void UpdateHandle()
//     {
//         var deleteLeftHandle = AddHandle("DeleteRight", "delete", Direction.Right);
//         var addRightHandle = AddHandle("AddLeft", "add", Direction.Left);
//         var addLeftHandle = AddHandle("AddRight", "add", Direction.Right);
//         var deleteRightHandle = AddHandle("DeleteLeft", "delete", Direction.Left);
//         _handles.Add(deleteLeftHandle);
//         _handles.Add(deleteRightHandle);
//         _handles.Add(addLeftHandle);
//         _handles.Add(addRightHandle);
//         _selectionSettings.UserHandles = _handles;
//     }
//     private UserHandle AddHandle(string name, string path, Direction direction)
//     {
//         var handle = new UserHandle
//         {
//             Name = name,
//             Visible = true,
//             Offset = 0.5,
//             Side = direction,
//             Margin = new Margin { Top = 0, Bottom = 0, Left = 0, Right = 0 }
//         };
//         if (path == "delete")
//         {
//             handle.PathData = "M1.0000023,3 L7.0000024,3 7.0000024,8.75 C7.0000024,9.4399996 6.4400025,10 5.7500024,10 L2.2500024,10 C1.5600024,10 1.0000023,9.4399996 1.0000023,8.75 z M2.0699998,0 L5.9300004,0 6.3420029,0.99999994 8.0000001,0.99999994 8.0000001,2 0,2 0,0.99999994 1.6580048,0.99999994 z";
//         }
//         else
//         {
//             handle.PathData = "M4.0000001,0 L6,0 6,4.0000033 10,4.0000033 10,6.0000033 6,6.0000033 6,10 4.0000001,10 4.0000001,6.0000033 0,6.0000033 0,4.0000033 4.0000001,4.0000033 z";
//         }
//         return handle;
//     }
//
//     protected override void OnInitialized()
//     {
//         _selectionSettings.Constraints &= ~(SelectorConstraints.ResizeAll | SelectorConstraints.Rotate);
//         DataSource = new List<MindMapDetails>
//         {
//             new() { Id="1",Label="Business Planning",ParentId ="",Branch= BranchType.Root, Fill="#034d6d" },
//             new() { Id="2",Label= "Expectation",ParentId = "1",Branch= BranchType.Left,Fill= "#1b80c6" },
//             new() { Id="3",Label= "Requirements", ParentId="1",Branch= BranchType.Right,Fill= "#1b80c6" },
//             new() { Id="4",Label= "Marketing", ParentId="1",Branch= BranchType.Left,Fill= "#1b80c6" },
//             new() { Id="5",Label= "Budgets",ParentId= "1",Branch= BranchType.Right,Fill= "#1b80c6" },
//             new() { Id="6", Label="Situation in Market", ParentId= "1", Branch = BranchType.Left, Fill= "#1b80c6" },
//             new() { Id="7", Label="Product Sales", ParentId= "2", Branch = BranchType.SubLeft, Fill= "#3dbfc9" },
//             new() { Id = "8", Label= "Strategy", ParentId="2", Branch = BranchType.SubLeft, Fill="#3dbfc9" },
//             new() { Id = "9", Label="Contacts", ParentId="2", Branch = BranchType.SubLeft, Fill="#3dbfc9" },
//             new() { Id = "10", Label="Customer Groups", ParentId= "4", Branch = BranchType.SubLeft,Fill= "#3dbfc9" },
//             new() { Id = "11", Label= "Branding", ParentId= "4", Branch = BranchType.SubLeft, Fill= "#3dbfc9" },
//             new() { Id = "12", Label= "Advertising", ParentId= "4", Branch = BranchType.SubLeft, Fill= "#3dbfc9" },
//             new() { Id = "13", Label= "Competitors", ParentId= "6", Branch = BranchType.SubLeft, Fill="#3dbfc9" },
//             new() { Id = "14", Label="Location", ParentId="6", Branch = BranchType.SubLeft, Fill= "#3dbfc9" },
//             new() { Id = "15", Label= "Director", ParentId= "3", Branch = BranchType.SubRight, Fill="#3dbfc9" },
//             new() { Id = "16", Label="Accounts Department", ParentId= "3", Branch = BranchType.SubRight, Fill= "#3dbfc9" },
//             new() { Id = "17", Label="Administration", ParentId= "3", Branch = BranchType.SubRight, Fill="#3dbfc9" },
//             new() { Id = "18", Label= "Development", ParentId="3", Branch = BranchType.SubRight, Fill= "#3dbfc9" },
//             new() { Id = "19", Label= "Estimation", ParentId= "5", Branch = BranchType.SubRight, Fill="#3dbfc9" },
//             new() { Id = "20", Label= "Profit", ParentId= "5", Branch = BranchType.SubRight, Fill= "#3dbfc9" },
//             new() { Id="21", Label="Funds", ParentId= "5", Branch = BranchType.SubRight, Fill= "#3dbfc9" }
//         };
//     }
//     protected override void OnAfterRender(bool firstRender)
//     {
//         if (firstRender)
//         {
//             UpdateHandle();
//         }
//     }
// }
