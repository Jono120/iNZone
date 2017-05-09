//-------------------------------------------------------------------------- 
//
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
//
//  File: SqlExpressClientSyncProvider.cs 
//
//  Description: Generic client synchronization provider.
//
//--------------------------------------------------------------------------
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Microsoft.Samples.Synchronization.Data.SqlExpress
{
    ///<summary>
    /// A generic client sync provider that can connect to SQL Express
    /// </summary>
    /// <remarks>    
    /// SqlExpressClientSyncProvider inherits from ClientSyncProvider, and it is a generic 
    /// implementation of ServerSyncProvider. SqlExpressClientSyncProvider uses the mechanisms
    /// of DbServerSyncProvider to connect to the client (ie using a DBConnection)
    /// </remarks>
    public class SqlExpressClientSyncProvider : ClientSyncProvider
    {
        // wraps a DbServerSyncProvider
        private DbServerSyncProvider _dbSyncProvider;
        private Guid _clientId;

        private int _refCntSession;
        private IDbTransaction _transaction;

        private const string GuidTableName = "guid";
        private const string AnchorTableName = "anchor";

        public event EventHandler<ApplyChangeFailedEventArgs> ApplyChangeFailed;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SqlExpressClientSyncProvider()
        {
            _dbSyncProvider = new DbServerSyncProvider();
            _dbSyncProvider.ApplyingChanges += new EventHandler<ApplyingChangesEventArgs>(_dbSyncProvider_ApplyingChanges);
            _dbSyncProvider.ApplyChangeFailed += new EventHandler<ApplyChangeFailedEventArgs>(_dbSyncProvider_ApplyChangeFailed);
            _clientId = Guid.Empty;
            _refCntSession = 0;
            _transaction = null;
        }

        void _dbSyncProvider_ApplyChangeFailed(object sender, ApplyChangeFailedEventArgs e)
        {
            if (ApplyChangeFailed != null)
            {
                ApplyChangeFailed(sender, e);
            }
        }

        /// <summary>
        ///  Roll ApplyChanges() modifications done by inner _dbSyncProvider 
        ///  into one transaction with anchor changes
        /// </summary>
        /// <param name="sender"> Event params </param>
        /// <param name="e"> Event params </param>
        void _dbSyncProvider_ApplyingChanges(object sender, ApplyingChangesEventArgs e)
        {
            if (_transaction != null)
                e.Transaction = _transaction;
        }

        /// <summary>
        /// Apply changes downloaded from the server. 
        /// </summary>
        /// <remarks>
        /// Inner _dbSyncProvider will take care of applying changes to actual 
        /// data, but we need to take care of updating anchor metadata. 
        /// </remarks>
        /// <param name="groupMetadata"> Contains table metadata info </param>
        /// <param name="dataSet"> Contains changes to be applied </param>
        /// <param name="syncSession"> Current sync session </param>
        /// <returns> SyncContext object to Sync Agent </returns>
        public override SyncContext ApplyChanges(SyncGroupMetadata groupMetadata, DataSet dataSet, SyncSession syncSession)
        {
            SyncContext syncContext = _dbSyncProvider.ApplyChanges(groupMetadata, dataSet, syncSession);
            foreach (SyncTableMetadata table in groupMetadata.TablesMetadata)
            {
                SetTableReceivedAnchor(table.TableName, groupMetadata.NewAnchor);
            }
            return syncContext;

        }

        /// <summary>
        /// Creates the database schema on client database -- NOT IMPLEMENTED
        /// </summary>
        /// <remarks>
        /// In the current implementation of this class, we assume that the 
        /// client already has the same schema as the server (run the demo scripts).
        /// </remarks>
        public override void CreateSchema(SyncTable syncTable, SyncSchema syncSchema)
        {
            throw new NotSupportedException("Create Schema is not supported in this version."
                    + "Please make sure client and server have same schema!");
        }


        /// <summary>
        /// Gets the changes made on the client since last sync.
        /// </summary>
        /// <param name="groupMetadata"> Contains table metadata </param>
        /// <param name="syncSession"> The current sync session </param>
        /// <returns> SyncContext populated with the incremental changes </returns>
        public override SyncContext GetChanges(SyncGroupMetadata groupMetadata, SyncSession syncSession)
        {
            // neet to set the LastReceivedAnchor as the LastSentAnchor since 
            // DbServerSyncProvider operates from the server's perspective, so
            // we swap the two fields temporarily. 
            foreach (SyncTableMetadata metaTable in groupMetadata.TablesMetadata)
            {
                SyncAnchor temp = metaTable.LastReceivedAnchor;
                metaTable.LastReceivedAnchor = metaTable.LastSentAnchor;
                metaTable.LastSentAnchor = temp;
            }

            SyncContext context = _dbSyncProvider.GetChanges(groupMetadata, syncSession);

            //swap them back for consistency
            foreach (SyncTableMetadata metaTable in groupMetadata.TablesMetadata)
            {
                SyncAnchor temp = metaTable.LastReceivedAnchor;
                metaTable.LastReceivedAnchor = metaTable.LastSentAnchor;
                metaTable.LastSentAnchor = temp;
            }
            return context;
        }

        /// <summary>
        /// Gets the client's ID
        /// </summary>
        /// <remarks>
        /// This function currently just reads the value in a database table
        /// named 'guid,' which is initialized upon client database creation
        /// (see demo script). 
        /// </remarks>
        /// <returns> A Guid object containing client's ID </returns>
        public Guid GetClientId()
        {
            if (_clientId == Guid.Empty)
            {
                IDbCommand guidCom = null;
                IDataReader reader = null;
                try
                {
                    BeginTransaction(null);
                    string queryStr = "SELECT Guid FROM " + GuidTableName;
                    guidCom = new SqlCommand(queryStr);
                    guidCom.Connection = _dbSyncProvider.Connection;
                    guidCom.CommandType = CommandType.Text;
                    guidCom.Transaction = _transaction;
                    reader = guidCom.ExecuteReader();
                    if (reader.Read())
                    {
                        _clientId = reader.GetGuid(0);
                    }
                }
                catch
                {
                    _clientId = Guid.Empty;
                    throw;
                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                        reader.Close();
                    reader.Dispose();
                    guidCom.Dispose();
                    EndTransaction(true, null);
                }

            }
            return _clientId;
        }

        /// <summary>
        /// Retrieves the last received anchor from the 'anchor' metatable.
        /// </summary>
        /// <param name="tableName"> The name of the table which we want the anchor for. </param>
        /// <returns> A sync anchor object containing the last received anchor. </returns>
        public override SyncAnchor GetTableReceivedAnchor(string tableName)
        {
            string queryStr = "SELECT ReceivedAnchor FROM " + AnchorTableName + " WHERE TableName = '" + tableName + "'";
            IDbCommand receivedAnchorCom = new SqlCommand(queryStr);
            receivedAnchorCom.Connection = _dbSyncProvider.Connection;
            receivedAnchorCom.CommandType = CommandType.Text;
            receivedAnchorCom.Transaction = _transaction;

            object anchorVal = null;
            bool commandPassed = false;
            try
            {
                BeginTransaction(null);
                anchorVal = receivedAnchorCom.ExecuteScalar();
                commandPassed = true;
            }
            finally
            {
                receivedAnchorCom.Dispose();
                EndTransaction(commandPassed, null);
            }

            if (anchorVal == null || anchorVal == System.DBNull.Value)
                return new SyncAnchor();
            else
                return new SyncAnchor((byte[])anchorVal);
        }

        /// <summary>
        /// Retrieves the last sent anchor from the 'anchor' metatable.
        /// </summary>
        /// <param name="tableName"> The name of the table for which we want the anchor. </param>
        /// <returns> A sync anchor object containing the last sent anchor. </returns>
        public override SyncAnchor GetTableSentAnchor(string tableName)
        {
            string queryStr = "SELECT SentAnchor FROM " + AnchorTableName + " WHERE TableName = '" + tableName + "'";
            IDbCommand sentAnchorCom = new SqlCommand(queryStr);
            sentAnchorCom.Connection = _dbSyncProvider.Connection;
            sentAnchorCom.CommandType = CommandType.Text;
            sentAnchorCom.Transaction = _transaction;
            object anchorVal = null;
            bool commandPassed = false;
            try
            {
                BeginTransaction(null);
                anchorVal = sentAnchorCom.ExecuteScalar();
                commandPassed = true;
            }
            finally
            {
                sentAnchorCom.Dispose();
                EndTransaction(commandPassed, null);
            }

            if (anchorVal == System.DBNull.Value)
                return new SyncAnchor();
            else
                return new SyncAnchor((byte[])anchorVal);
        }

        /// <summary>
        /// Sets the last received anchor in the 'anchor' metatable
        /// </summary>
        /// <param name="tableName"> The name of the table for which we want to set the anchor </param>
        /// <param name="anchor"> SyncAnchor object containing the anchor. </param>
        public override void SetTableReceivedAnchor(string tableName, SyncAnchor anchor)
        {
            string queryStr = "UPDATE " + AnchorTableName +
                " SET ReceivedAnchor = @anchor WHERE TableName = '" + tableName + "'";
            SqlCommand anchorCom = new SqlCommand(queryStr);
            anchorCom.Parameters.AddWithValue("@anchor", anchor.Anchor);
            anchorCom.Connection = (SqlConnection)_dbSyncProvider.Connection;
            anchorCom.Transaction = (SqlTransaction)_transaction;
            bool commandPassed = false;
            try
            {
                BeginTransaction(null);
                if (anchorCom.ExecuteNonQuery() == 0)
                    throw new Exception("SetTableReceivedAnchor() had no effect");
                commandPassed = true;
            }
            finally
            {
                anchorCom.Dispose();
                EndTransaction(commandPassed, null);
            }
        }

        /// <summary>
        /// Sets teh last sent anchor in the 'anchor' metatable
        /// </summary>
        /// <param name="tableName"> The name of the table for whcih we want to set the anchor </param>
        /// <param name="anchor"> SyncAnchor object containing the anchor. </param>
        public override void SetTableSentAnchor(string tableName, SyncAnchor anchor)
        {
            string queryStr = "UPDATE " + AnchorTableName +
                " SET SentAnchor = @anchor WHERE TableName = '" + tableName + "'";
            SqlCommand anchorCom = new SqlCommand(queryStr);
            anchorCom.Parameters.AddWithValue("@anchor", anchor.Anchor);
            anchorCom.Connection = (SqlConnection)_dbSyncProvider.Connection;
            anchorCom.Transaction = (SqlTransaction)_transaction;

            bool commandPassed = false;
            try
            {
                BeginTransaction(null);
                if (anchorCom.ExecuteNonQuery() == 0)
                    throw new Exception("SetTableSentAnchor() had no effect");
                commandPassed = true;
            }
            finally
            {
                anchorCom.Dispose();
                EndTransaction(commandPassed, null);
            }
        }

        /// <summary>
        /// Begin a transaction. This method is invoked to mark atomic operations.   
        /// </summary>
        /// <param name="syncSession">SyncSession information</param>               
        public override void BeginTransaction(SyncSession syncSession)
        {
            _refCntSession++;
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            if (_transaction == null)
            {
                _transaction = _dbSyncProvider.Connection.BeginTransaction();
            }
        }

        /// <summary>
        /// End transaction. This method is called by the agent at to conclude an atomic operation.
        /// </summary>
        /// <param name="commit">Commit/Abort SyncSession</param>               
        /// <param name="syncSession">SyncSession information</param>               
        public override void EndTransaction(bool commit, SyncSession syncSession)
        {
            _refCntSession--;
            if (_refCntSession == 0)
            {
                if (commit)
                    _transaction.Commit();
                else
                    _transaction.Rollback();

                _transaction.Dispose();
                _transaction = null;

                Connection.Close();
            }
            if (_refCntSession < 0) _refCntSession = 0;
            //else if (_refCntSession < 0)
            //    throw new SyncException("Transaction error: _refCntSession < 0");
        }
        /// <summary>
        /// Disposes this Client Sync Provider instance 
        /// </summary>
        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this Client Sync Provider instance 
        /// </summary>
        /// <param name="disposing">True if explicit finalization, false if through GC</param>
        //[SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive")]
        protected virtual void Dispose(bool disposing)
        {
            _dbSyncProvider.Dispose();
        }

        #region public properties

        /// <summary>
        /// Gets or sets the command that will return the new anchor value. 
        /// </summary>
        /// <value>
        /// A <b>IDbCommand</b>-inheritated object.
        /// </value>
        public IDbCommand SelectNewAnchorCommand
        {
            get
            {
                return _dbSyncProvider.SelectNewAnchorCommand;
            }
            set
            {
                _dbSyncProvider.SelectNewAnchorCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the server connection object.
        /// </summary>
        /// <value>
        /// A <b>DbConnection</b> object.
        /// </value>
        public IDbConnection Connection
        {
            get
            {
                return _dbSyncProvider.Connection;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (value.State == ConnectionState.Closed)
                {
                    // giving the connection to the _dbSyncPRovider as open 
                    // will prevent it from closing off the connection at end 
                    // of operations
                    value.Open();
                    _dbSyncProvider.Connection = value;
                    value.Close();
                }
                else _dbSyncProvider.Connection = value;

            }
        }

        /// <summary>
        /// Gets the collection of <b>SyncAdapter</b>.
        /// </summary>
        /// <value>
        /// A <b>SyncAdapterCollection</b> object.
        /// </value>
        public SyncAdapterCollection SyncAdapters
        {
            get
            {
                return _dbSyncProvider.SyncAdapters;
            }
        }


        /// <summary>
        /// Gets and sets ClientId
        /// </summary>
        /// <remarks>
        /// A new client is generated and saved if none is present
        /// </remarks>
        /// <value>
        /// A <b>Guid</b> object.
        /// </value>
        public override Guid ClientId
        {
            get
            {
                return GetClientId();
            }

            set
            {
                _clientId = value;
            }
        }
        #endregion
    }
}
