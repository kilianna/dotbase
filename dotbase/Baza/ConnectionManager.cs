using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using DotBase.Logging;

namespace DotBase.Baza
{
    class ConnectionManager
    {
        private Logger log = Log.create();

        const int DISCONNECT_TIME_MS = 1000;
        const int DATABASE_LOCKING = 1;
        const int LOG_LOCKING = 2;
        DatabaseConnection database;
        DatabaseConnection databaseLog;
        AccessLocker locker;
        DateTime lastActive;
        bool inIdle;
        FailureWaitForm failureWaitForm = new FailureWaitForm();
        bool failureReported = false;
        System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();

        public ConnectionManager(string path, string logPath, string password)
        {
            log("Construct {0} {1}", path, logPath);
            database = new DatabaseConnection(path, password);
            if (logPath != null)
                databaseLog = new DatabaseConnection(logPath, password);
            locker = new AccessLocker(getLockFilePath(path));
            lastActive = DateTime.Now;
            inIdle = false;
            closeTimer.Interval = DISCONNECT_TIME_MS / 4;
            closeTimer.Tick += new EventHandler(closeTimer_Tick);
        }

        void closeTimer_Tick(object sender, EventArgs e)
        {
            log("Tick");
            if (DateTime.Now > (lastActive + new TimeSpan(0, 0, 0, 0, DISCONNECT_TIME_MS)))
            {
                if (database.Connection != null)
                {
                    closeConnection();
                }
                else
                {
                    close();
                }
            }
        }

        public OleDbConnection connection { get { prepareConnection(); return database.Connection; } }
        public OleDbTransaction transaction { get { prepareConnection(); return database.Transaction; } }
        public OleDbConnection connectionLog { get { prepareConnectionLog(); return databaseLog == null ? null : databaseLog.Connection; } }
        public OleDbTransaction transactionLog { get { prepareConnectionLog(); return databaseLog == null ? null : databaseLog.Transaction; } }

        public OleDbCommand command(string query)
        {
            prepareConnection();
            log("Command {0}", Log.common(query));
            return new OleDbCommand(query, database.Connection, database.Transaction);
        }

        public OleDbCommand commandLog(string query)
        {
            prepareConnectionLog();
            log("Command Log {0}", Log.common(query));
            if (databaseLog != null && databaseLog.Connection != null && databaseLog.Transaction != null)
            {
                return new OleDbCommand(query, databaseLog.Connection, databaseLog.Transaction);
            }
            else
            {
                return null;
            }
        }

        public void close()
        {
            log("Close all");
            closeConnection();
            locker.unblock(DATABASE_LOCKING | LOG_LOCKING);
            closeTimer.Enabled = false;
        }

        private void closeConnection()
        {
            DatabaseConnection.ForceCommandsCleanup();
            if (failureReported) {
                log("Close connection with rollback");
                failureReported = false;
                failureWaitForm.Hide();
                database.rollback();
                if (databaseLog != null) databaseLog.rollback();
            } else {
                log("Close connection with commit");
                database.commit();
                if (databaseLog != null) databaseLog.commit();
            }
            DatabaseConnection.ForceCommandsCleanup();
        }

        public void failure()
        {
            if (database.Connection != null) {
                log("Setting failure");
                failureReported = true;
                failureWaitForm.Show();
            } else {
                log("Setting failure outside transaction - ignored");
            }
        }

        private void prepareConnection()
        {
            log("Connect request");
            lastActive = DateTime.Now;
            inIdle = false;
            closeTimer.Enabled = true;
            int counter = 0;
            while (true)
            {
                locker.block(DATABASE_LOCKING);
                try
                {
                    database.connect();
                    if (counter > 0) log("Connection success");
                    return;
                }
                catch (Exception ex)
                {
                    locker.unblock(DATABASE_LOCKING);
                    counter++;
                    log("Open database attempt {0} exception: {1}", counter, ex.ToString());
                    Thread.Sleep(AccessLocker.RETRY_DELAY_MS);
                    if (counter > AccessLocker.SILENT_RETRY_MS / AccessLocker.RETRY_DELAY_MS)
                    {
                        var res = (new AccessLockerForm(ex.Message + "\r\n\r\n" + ex.ToString())).ShowDialog();
                        log("Dialog result {0}", res);
                        if (res == DialogResult.Ignore || res == DialogResult.Retry)
                        {
                            // nothing to do, retry
                        }
                        else if (res == DialogResult.Abort)
                        {
                            throw new ApplicationException("Aborted");
                        }
                    }
                }
            }
        }

        private void prepareConnectionLog()
        {
            log("Connect log request");
            lastActive = DateTime.Now;
            inIdle = false;
            closeTimer.Enabled = true;
            int counter = 0;
            while (true)
            {
                locker.block(LOG_LOCKING);
                try
                {
                    databaseLog.connect();
                    if (counter > 0) log("Connection success");
                    return;
                }
                catch (Exception ex)
                {
                    locker.unblock(LOG_LOCKING);
                    counter++;
                    log("Open log database attempt {0} exception: {1}", counter, ex.ToString());
                    Thread.Sleep(AccessLocker.RETRY_DELAY_MS);
                    if (counter > AccessLocker.LOG_RETRY_MS / AccessLocker.RETRY_DELAY_MS)
                    {
                        log("Open log database failed!");
                        return;
                    }
                }
            }
        }

        private string getLockFilePath(string sciezkaDoBazy)
        {
            int pos = sciezkaDoBazy.LastIndexOf('.');
            if (pos < 0) return sciezkaDoBazy + ".lock";
            return sciezkaDoBazy.Substring(0, pos) + ".lock";
        }

        internal void idle()
        {
            if (!inIdle)
            {
                log("Switch to idle");
                lastActive = DateTime.Now;
                inIdle = true;
            }
        }
    }
}
