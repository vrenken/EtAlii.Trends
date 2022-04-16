// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends.Editor.Trends;

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

public partial class ActivityIndicator
{
    private bool _showIndicator;
    private readonly ThrottledInvocation _hideIndicator;

    public ActivityIndicator()
    {
        _hideIndicator = new ThrottledInvocation(TimeSpan.FromSeconds(1), async () =>
        {
            _showIndicator = false;
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        });
    }

    protected override void OnInitialized()
    {
        _queryDispatcher.Started += OnActivityStarted;
        _commandDispatcher.Started += OnActivityStarted;
        _queryDispatcher.Stopped += OnActivityStopped;
        _commandDispatcher.Stopped += OnActivityStopped;
    }

    private void OnActivityStarted()
    {
        _showIndicator = true;
        StateHasChanged();
    }
    private void OnActivityStopped()
    {
        _hideIndicator.Raise();
    }
}
