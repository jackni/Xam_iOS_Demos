using System;
using ReactiveUI;
using UIKit;
using System.Reactive.Subjects;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MyApp.iOS
{
    //TODO: 
    public abstract class BaseTableViewController<TViewModel> : ReactiveTableViewController,IViewFor<TViewModel> where TViewModel: class
    { 
        #region Members

        private readonly ISubject<bool> _appearingSubject = new Subject<bool>();
        private readonly ISubject<bool> _appearedSubject = new Subject<bool>();
        private readonly ISubject<bool> _disappearingSubject = new Subject<bool>();
        private readonly ISubject<bool> _disappearedSubject = new Subject<bool>();
        private readonly ICollection<IDisposable> _activations = new LinkedList<IDisposable>();

        public IObservable<bool> Appearing
        {
            get { return _appearingSubject; }
        }

        public IObservable<bool> Appeared
        {
            get { return _appearedSubject; }
        }

        public IObservable<bool> Disappearing
        {
            get { return _disappearingSubject; }
        }

        public IObservable<bool> Disappeared
        {
            get { return _disappearedSubject; }
        }

        private TViewModel _viewModel;
        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }


        object IViewFor.ViewModel
        {
            get { return _viewModel; }
            set { ViewModel = (TViewModel)value; }
        }
        #endregion

        #region Constructors
        protected BaseTableViewController()
            : this(UITableViewStyle.Plain)
        {
        }

        protected BaseTableViewController(UITableViewStyle style)
            : base(style)
        {
//            var titleObs = this.WhenAnyValue(x => x.ViewModel)
//                .OfType<IProvidesTitle>()
//                .Select(x => x.WhenAnyValue(y => y.Title))
//                .Switch();
//
//            var routeObs = this.WhenAnyValue(x => x.ViewModel)
//                .OfType<IRoutingViewModel>()
//                .Select(x => x.RequestNavigation)
//                .Switch();

//            OnActivation(d => {
//                d(titleObs.Subscribe(x => Title = x ?? string.Empty));
//                d(routeObs.Subscribe(x => {
//                    var viewModelViewService = Locator.Current.GetService<IViewModelViewService>();
//                    var serviceConstructor = Locator.Current.GetService<IServiceConstructor>();
//                    var viewType = viewModelViewService.GetViewFor(x.GetType());
//                    var view = (IViewFor)serviceConstructor.Construct(viewType);
//                    view.ViewModel = x;
//                    HandleNavigation(x, view as UIViewController);
//                }));
//            });


            //            this.Appearing
            //                .Take(1)
            //                .Subscribe(_ => SetupLoadMore());
            //
            //            this.Appeared.Take(1)
            //                .Select(_ => this.WhenAnyValue(x => x.ViewModel))
            //                .Switch()
            //                .OfType<IProvidesEmpty>()
            //                .Select(x => x.WhenAnyValue(y => y.IsEmpty))
            //                .Switch()
            //                .Where(x => EmptyView != null)
            //                .Subscribe(CreateEmptyHandler);
        }

        #endregion


        #region Methods
        public void OnActivation(Action<Action<IDisposable>> d)
        {
            Appearing.ObserveOn(Scheduler.CurrentThread).Subscribe(_ => d(x => _activations.Add(x)));
        }

        #endregion
    }
}

