using System;
using System.Reactive;

namespace MyDemo.Core
{
    public interface IRoutingViewModel
    {
        IObservable<IBaseViewModel> RequestNavigation { get; }

        IObservable<Unit> RequestDismiss { get; }
    }
}

