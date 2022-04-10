// // Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends
//
// namespace EtAlii.Trends.Pages;
//
// using System.Collections.ObjectModel;
// using Syncfusion.Blazor.Diagram;
//
// public class AddRightTool : DragController
// {
//     private readonly SfDiagramComponent _diagram;
//     public AddRightTool(SfDiagramComponent diagram) : base(diagram)
//     {
//         _diagram = diagram;
//     }
//     public override async void OnMouseDown(DiagramMouseEventArgs args)
//     {
//         var newChildID = _diagram.Nodes.Count + 1;
//         var newchildColor = "";
//         var type = (_diagram.SelectionSettings.Nodes[0].Data as MindMapDetails).Branch;
//         var childType = BranchType.Left;
//         switch (type.ToString())
//         {
//             case "Root":
//                 childType = BranchType.Right;
//                 break;
//             case "Right":
//                 childType = BranchType.SubRight;
//                 break;
//             case "SubRight":
//                 childType = BranchType.SubRight;
//                 break;
//         }
//         if (_diagram.SelectionSettings.Nodes[0].Style.Fill == "#034d6d")
//         {
//             newchildColor = "#1b80c6";
//         }
//         else
//         {
//             newchildColor = "#3dbfc9";
//         }
//         var childNode = new MindMapDetails
//         {
//             Id = newChildID.ToString(),
//             ParentId = (_diagram.SelectionSettings.Nodes[0].Data as MindMapDetails).Id,
//             Fill = newchildColor,
//             Branch = childType,
//             Label = "New Child"
//         };
//         _diagram.BeginUpdate();
//         await TimeMap.UpdatePortConnection(childNode, _diagram).ConfigureAwait(false);
//         await _diagram.EndUpdate().ConfigureAwait(false);
//         TimeMap.DataSource.Add(childNode);
//         _diagram.ClearSelection();
//         base.OnMouseDown(args);
//         _diagram.Select(new ObservableCollection<IDiagramObject> { _diagram.Nodes[^1] });
//         _diagram.StartTextEdit(_diagram.Nodes[^1]);
//         InAction = true;
//     }
// }
