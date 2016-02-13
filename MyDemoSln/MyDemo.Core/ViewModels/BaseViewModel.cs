using System;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Subjects;

namespace MyDemo.Core
{
    public class BaseViewModel :ReactiveObject,IBaseViewModel
    {
        #region Members
        private readonly ViewModelActivator _viewModelActivator = new ViewModelActivator();
        private readonly ISubject<IBaseViewModel> _requestNavigationSubject = new Subject<IBaseViewModel>();
        private readonly ISubject<Unit> _requestDismissSubject = new Subject<Unit>();
        #endregion

        public BaseViewModel()
        {
        }

        //private IReactiveCommand<Unit> _loadCommand;

        #region ISupportsActivation implementation

        ViewModelActivator ISupportsActivation.Activator
        {
            get
            {
                return _viewModelActivator;
            }
        }

        #endregion

        protected void NavigateTo(IBaseViewModel viewModel)
        {
            var loadableViewModel = viewModel as ILoadableViewModel;
            _requestNavigationSubject.OnNext(viewModel);
            loadableViewModel?.LoadCommand.ExecuteIfCan();
        }

//        IObservable<IBaseViewModel> IRoutingViewModel.RequestNavigation
//        {
//            get { return _requestNavigationSubject; }
//        }
//
//        IObservable<Unit> IRoutingViewModel.RequestDismiss
//        {
//            get { return _requestDismissSubject; }
//        }

        protected void Dismiss()
        {
            _requestDismissSubject.OnNext(Unit.Default);
        }
    }
}

