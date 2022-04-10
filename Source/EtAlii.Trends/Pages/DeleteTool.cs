// // Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends
//
// namespace EtAlii.Trends.Pages;
//
// using Syncfusion.Blazor.Diagram;
//
// public class DeleteTool : DragController
// {
//     private readonly SfDiagramComponent _diagram;
//     public DeleteTool(SfDiagramComponent diagram) : base(diagram)
//     {
//         _diagram = diagram;
//     }
//     public override void OnMouseDown(DiagramMouseEventArgs args)
//     {
//         var deleteObject = new Node();
//         if (_diagram.SelectionSettings.Nodes.Count > 0)
//         {
//             deleteObject = _diagram.SelectionSettings.Nodes[0] as Node;
//         }
//         _diagram.BeginUpdate();
//         TimeMap.RemoveData(deleteObject,_diagram);
//         _diagram.Nodes.Remove(deleteObject);
//         _ = _diagram.RefreshDataSource();
//         _ = _diagram.EndUpdate();
//         base.OnMouseDown(args);
//         InAction = true;
//     }
// }
