using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CryptoTracker.WPF.MVVM
{
    public class TaskWatcher<TResult> : INotifyPropertyChanged
    {
        
        //source https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
        //Raise the task properties at certain stages of completion/error


       

        public TaskWatcher(Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                WatchTaskAsync(task);
            }

        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception)
            {

                
            }

            //if no error occurs these task properties will change

            RaisePropertyChanged(this, "Status");
            RaisePropertyChanged(this, "IsCompleted");
            RaisePropertyChanged(this, "IsNotCompleted");


            if (task.IsFaulted)
            {
                RaisePropertyChanged(this, "IsFaulted");
                RaisePropertyChanged(this, "Exception");
                RaisePropertyChanged(this, "InnerException");
                RaisePropertyChanged(this, "ErrorMessage");
            }
            
            else if (task.IsCanceled)
            {
                RaisePropertyChanged(this, "IsCanceled");
            }

            else
            {
                RaisePropertyChanged(this, "IsSuccessfullyCompleted");
                RaisePropertyChanged(this, "Result");
            }
            


        }

        public void RaisePropertyChanged(object sender, string property)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(sender, new PropertyChangedEventArgs(property));
        }


        public event PropertyChangedEventHandler PropertyChanged;



        #region task properties
        public Task<TResult> Task { get; private set; }
        public TResult Result
        {
            get
            {
                return (Task.Status == TaskStatus.RanToCompletion) ?
Task.Result : default(TResult);
            }
        }
        public TaskStatus Status { get { return Task.Status; } }
        public bool IsCompleted { get { return Task.IsCompleted; } }
        public bool IsNotCompleted { get { return !Task.IsCompleted; } }
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task.Status ==
TaskStatus.RanToCompletion;
            }
        }
        public bool IsCanceled { get { return Task.IsCanceled; } }
        public bool IsFaulted { get { return Task.IsFaulted; } }
        public AggregateException Exception { get { return Task.Exception; } }
        public Exception InnerException
        {
            get
            {
                return (Exception == null) ?
null : Exception.InnerException;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return (InnerException == null) ?
null : InnerException.Message;
            }
        }
        #endregion

    }
}
