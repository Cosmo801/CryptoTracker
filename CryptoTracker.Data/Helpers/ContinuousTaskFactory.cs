using System;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Helpers
{
    public class ContinuousTaskFactory
    {
        /// <summary>
        /// A task is passed in that needs to continuously operate. The callback action is used to process results
        /// </summary>

        public ContinuousTaskFactory()
        {
            Initialize();
        }

        protected void Initialize()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            IsRunning = false;
        }

        public void CreateTask(Func<Task> task, int delay, Action callback)
        {

            try
            {
                Task.Factory.StartNew(async () =>
                {
                    OnTaskRunning(true);

                    while (true)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            _cancellationToken.ThrowIfCancellationRequested();
                        }
                        await Task.Run(task).ConfigureAwait(false);
                        await Task.Run(callback).ConfigureAwait(false);
                        await Task.Delay(delay).ConfigureAwait(false);
                    }



                }, _cancellationToken);

            }
            catch (OperationCanceledException)
            {
                Initialize();
                return;
            }


            catch (Exception)
            {

                throw;
            }
        }

        public Action<object, EventArgs> TaskStarted;
        public Action<object, EventArgs> TaskEnded;

        protected void OnTaskRunning(bool state)
        {
            if (state == true)
            {
                IsRunning = true;
                if (TaskStarted == null) return;
                TaskStarted(this, new EventArgs());
            }

            if (state == false)
            {
                IsRunning = false;
                if (TaskEnded == null) return; 
                TaskEnded(this, new EventArgs());
            }
        }
        public bool IsRunning { get; private set; }

        protected CancellationTokenSource _cancellationTokenSource;
        protected CancellationToken _cancellationToken;

        public void EndTask()
        {
            if (!IsRunning || _cancellationTokenSource == null) return;

            _cancellationTokenSource.Cancel();


        }

        

    }



    public class ContinuousTaskFactory<TResult> : ContinuousTaskFactory
    {
        public ContinuousTaskFactory()
            :base()
        {
           
        }

        public void CreateTask(Func<Task<TResult>> task, int delay, Action callback)
        {
            try
            {
                Task.Factory.StartNew(async () =>
                {
                    OnTaskRunning(true);

                    while (true)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            _cancellationToken.ThrowIfCancellationRequested();
                        }
                        await Task.Run(task).ConfigureAwait(false);
                        await Task.Run(callback).ConfigureAwait(false);
                        await Task.Delay(delay).ConfigureAwait(false);
                    }



                }, _cancellationToken);

            }
            catch (OperationCanceledException)
            {
                Initialize();
                return;
            }


            catch (Exception)
            {

                throw;
            }
        }  


     
    }




}
