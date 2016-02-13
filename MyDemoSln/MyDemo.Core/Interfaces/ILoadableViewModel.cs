using System;
using ReactiveUI;
using System.Reactive;

namespace MyDemo.Core
{
    public interface ILoadableViewModel
    {
        IReactiveCommand<Unit> LoadCommand { get; }
    }
}

