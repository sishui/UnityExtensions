using System;
using System.Threading;

namespace UnityExtensions
{
    /// <summary>
    /// 背景线程存档管理
    /// </summary>
    public class BackgroundThreadSaveManager : SaveManager
    {
        AutoResetEvent _event;
        Thread _thread;
        SaveTask _task;


        public BackgroundThreadSaveManager()
        {
            _event = new AutoResetEvent(false);

            _thread = new Thread(BackgroundThread)
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest,
                Name = "Background Thread",
            };

            _thread.Start();
        }


        public override void Dispose()
        {
            base.Dispose();
            _event.Close();
            _thread.Abort();
        }
        

        protected override void BeginTask(SaveTask task)
        {
            _task = task;
            _event.Set();
        }


        protected override void FinishTask(SaveTask task)
        {
        }


        // 后台线程
        void BackgroundThread()
        {
            while (true)
            {
                _event.WaitOne();

                switch (_task.type)
                {
                    case SaveTask.Type.Load:
                        try
                        {
                            _task.data = _task.target.Read();
                        }
                        catch (Exception e)
                        {
                            _task.exception = e;
                        }
                        break;

                    case SaveTask.Type.Save:
                        if (_task.success)
                        {
                            try
                            {
                                _task.target.Write(_task.data);
                            }
                            catch (Exception e)
                            {
                                _task.exception = e;
                            }
                        }
                        break;

                    case SaveTask.Type.Delete:
                        try
                        {
                            _task.target.Delete();
                        }
                        catch (Exception e)
                        {
                            _task.exception = e;
                        }
                        break;
                }

                _task = null;
                _finished = true;
            }
        }

    } // class BackgroundThreadSaveManager

} // namespace UnityExtensions