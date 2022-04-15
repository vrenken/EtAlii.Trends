// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Trends;

public class MailItem
{
    public string Id { get; init; }
    public string ParentId { get; init; }
    public string FolderName { get; init; }
    public bool Expanded { get; init; }
    public bool HasSubFolders { get; init; }
}
