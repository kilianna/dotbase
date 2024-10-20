using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DotBase.Test
{
    internal class TestBase : IDisposable
    {
        private const int SHORT_DELAY = 100;
        private const int LONG_DELAY = 3000;

        struct QueueItem
        {
            public int delay;
            public ResumeDelegate resume;
        };

        public delegate void ResumeDelegate();
        Timer timer = new Timer();
        bool idleWait = false;
        Queue<QueueItem> queue = new Queue<QueueItem>();
        EventHandler handler;
        protected BazaDanychWrapper baza = new BazaDanychWrapper();

        public TestBase()
        {
            handler = new EventHandler(Application_Idle);
            timer.Tick += new EventHandler(timer_Tick);
            Application.Idle += handler;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            if (queue.Count > 0)
            {
                var active = queue.Dequeue();
                active.resume();
                if (!timer.Enabled && queue.Count > 0)
                {
                    idleWait = false;
                    timer.Interval = queue.Peek().delay + LONG_DELAY;
                    timer.Enabled = true;
                }
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (queue.Count > 0 && (!timer.Enabled || !idleWait))
            {
                timer.Enabled = false;
                idleWait = true;
                timer.Interval = queue.Peek().delay + SHORT_DELAY;
                timer.Enabled = true;
            }
        }

        public void wait(int delay, ResumeDelegate resume)
        {
            QueueItem item;
            item.delay = delay;
            item.resume = resume;
            queue.Enqueue(item);
            if (!timer.Enabled)
            {
                idleWait = false;
                timer.Interval = item.delay + LONG_DELAY;
                timer.Enabled = true;
            }
        }

        public void wait(ResumeDelegate resume)
        {
            wait(0, resume);
        }

        public static T getForm<T>(bool optional = false)
        {
            var type = typeof(T);
            List<object> list = new List<object>();
            foreach (var form in Application.OpenForms)
            {
                if (form.GetType() == typeof(T))
                {
                    list.Add(form);
                }
                Debug.WriteLine(((Form)form).Text + " - " + ((Form)form).Name);
            }
            if (list.Count == 0)
            {
                if (optional) return default(T);
                throw new ApplicationException("Missing expected form: " + typeof(T).Name);
            }
            else if (list.Count != 1)
            {
                throw new ApplicationException("Invalid number of matching forms: " + typeof(T).Name);
            }
            return (T)list[0];
        }

        public static T field<T>(object obj, string name)
        {
            var info = obj.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)info.GetValue(obj);
        }

        public static void send(Control ctrl, string text)
        {
            if (!ctrl.Focus())
            {
                throw new ApplicationException("Cannot focus control.");
            }
            SendKeys.Send(text);
        }

        public static void press(Control ctrl)
        {
            send(ctrl, " ");
        }

        public void Dispose()
        {
            Application.Idle -= handler;
            timer.Dispose();
        }
    }
}
