using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Threading;
using System.IO;
using DotBase.Logging;

namespace DotBase.Baza
{
    class DatabaseConnection
    {
        private Logger log = Log.create();

        string path;
        string connectionString;
        public OleDbConnection Connection { get; private set; }
        public OleDbTransaction Transaction { get; private set; }

        public DatabaseConnection(string path, string password)
        {
            log("!", Path.GetFileName(path));
            log("construct: {0}", path);
            this.path = path;
            connectionString = String.Format(
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Jet OLEDB:Database Password={1};Mode=Share Exclusive",
                path, password);
        }

        public static OleDbConnection directConnection(string path, string password)
        {
            string connectionString;
            if (password != null)
            {
                connectionString = String.Format(
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Jet OLEDB:Database Password={1};Mode=Share Exclusive",
                    path, password);
            }
            else
            {
                connectionString = String.Format(
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Mode=Share Exclusive",
                    path);
            }
            var ret = new OleDbConnection(connectionString);
            ret.Open();
            return ret;
        }

        public void connect()
        {
            while (true)
            {
                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Connecting)
                    {
                        log("Connecting");
                        Thread.Sleep(100);
                        continue;
                    }
                    else if (Connection.State == ConnectionState.Closed || Connection.State == ConnectionState.Broken)
                    {
                        log("Database is closed or broken");
                        rollback();
                    }
                }

                if (Connection == null)
                {
                    try
                    {
                        log("Connecting database...");
                        Connection = new OleDbConnection(connectionString);
                        Connection.Open();
                        Transaction = Connection.BeginTransaction();
                        log("Connected");
                    }
                    catch (Exception)
                    {
                        log("Connection failed!");
                        rollback();
                        throw;
                    }
                }
                return;
            }
        }

        public void rollback()
        {
            log("Rolback requested");
            if (Transaction != null)
            {
                log("Rolback and dispose transaction");
                try { Transaction.Rollback(); }
                catch (Exception) { }
                try { Transaction.Dispose(); }
                catch (Exception) { }
            }
            if (Connection != null)
            {
                log("Close and dispose connection");
                try { Connection.Close(); }
                catch (Exception) { }
                try { Connection.Dispose(); }
                catch (Exception) { }
            }
            Connection = null;
            Transaction = null;
        }

        public void commit()
        {
            log("Commit requested");
            if (Transaction != null)
            {
                log("Commit and dispose transaction");
                Transaction.Commit();
                try { Transaction.Dispose(); }
                catch (Exception) { }
            }
            if (Connection != null)
            {
                log("Close and dispose connection");
                try { Connection.Close(); }
                catch (Exception) { }
                try { Connection.Dispose(); }
                catch (Exception) { }
            }
            Connection = null;
            Transaction = null;
        }

        internal static void ForceCommandsCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
