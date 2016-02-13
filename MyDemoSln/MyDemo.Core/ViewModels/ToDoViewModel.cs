using System;
using ReactiveUI;
using System.Reactive;
using Splat;
using System.Collections.Generic;
using MyDemo.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
namespace MyDemo.Core
{
    public class ToDoViewModel :ReactiveObject
    {
        #region Members
        IDataService _dataService;

        public ReactiveList<TodoItem> TodoItemList { get; }

        public ReactiveCommand<Unit> AddTodoItemCmd { get; private set; }
        //public ReactiveCommand<Unit> ListTodoCommand { get; private set; }
        ReactiveCommand<List<TodoItem>> Refresh { get; }

        #endregion

        public ToDoViewModel()
        {
            _dataService = Locator.Current.GetService<IDataService>();
           

            Refresh = ReactiveCommand.CreateAsyncObservable(x=>GetTodoItemList());
            Refresh
                .Subscribe(todolist=> 
                    {
                        TodoItemList.Clear(); TodoItemList.AddRange(todolist);
                    });
            Refresh.ThrownExceptions
                .Subscribe(thrownException => 
                    { 
                        this.Log().Error(thrownException); 
                    });
            
            TodoItemList.WhenAnyValue(changes =>
                {
                    TodoItem newItem = changes.ItemsAdded.Cast<TodoItem>();
                    AddTodoItem(newItem).ToObservable();
                });
        }

        #region Methods

        async Task<List<TodoItem>> GetTodoItems()
        {
            List<TodoItem> result = new List<TodoItem>();
            Expression<Func<TodoItem, bool>> predicate = (t => t.Text.ToLower().Contains("stuff"));

            result = await _dataService.QueryDataAsync(predicate,"getTodo",true);
            return result;
        }

        async Task<TodoItem> AddTodoItem(TodoItem newItem)
        {
            TodoItem result = new TodoItem();
            result = await _dataService.InsertDataAsync<TodoItem>(newItem,true);
            return result;
        }

        IObservable<TodoItem> AddTodoItemLocal(TodoItem newItem)
        {
            
            return _dataService.InsertDataAsync(newItem,false).ToObservable();
        }

        IObservable<List<TodoItem>> GetTodoItemList(bool queryCloud)
        {
            Expression<Func<TodoItem, bool>> predicate = (t => t.Text.ToLower().Contains("stuff"));
            return _dataService.QueryDataAsync(predicate, "getTodo", queryCloud).ToObservable();
        }
        #endregion
    }
}

