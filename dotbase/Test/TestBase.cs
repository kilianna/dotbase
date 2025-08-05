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
        private const int POLL_PERIOD = 50;

        struct QueueItem
        {
            public DateTime timeout;
            public ResumeDelegate resume;
            public PollDelegate poll;
        };

        public delegate void ResumeDelegate();
        public delegate bool PollDelegate();
        Timer timer = new Timer();
        Queue<QueueItem> queue = new Queue<QueueItem>();
        protected BazaDanychWrapper baza = new BazaDanychWrapper();
        static HashSet<TestBase> activeTests = new HashSet<TestBase>();

        public TestBase()
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = POLL_PERIOD;
            activeTests.Add(this);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            var next = queue.Peek();
            if (next.poll())
            {
                timer.Enabled = false;
                queue.Dequeue();
                next.resume();
            }
            else if (next.timeout < DateTime.Now)
            {
                throw new ApplicationException("Timeout when waiting in test.");
            }
        }

        public void wait(PollDelegate poll, ResumeDelegate resume)
        {
            QueueItem item;
            item.poll = poll;
            item.resume = resume;
            item.timeout = DateTime.Now.AddSeconds(15);
            queue.Enqueue(item);
            if (!timer.Enabled)
            {
                timer.Enabled = true;
            }
        }

        public void wait(ResumeDelegate resume)
        {
            wait(() => { return true; }, resume);
        }

        public static T getForm<T>(bool required)
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
                if (!required) return default(T);
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

        public static void method(object obj, string name, params object[] parameters)
        {
            var info = obj.GetType().GetMethod(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            info.Invoke(obj, parameters);
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
            timer.Dispose();
            activeTests.Remove(this);
        }
    }
}
