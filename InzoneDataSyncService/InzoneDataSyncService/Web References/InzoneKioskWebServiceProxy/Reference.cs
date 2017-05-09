﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.269.
// 
#pragma warning disable 1591

namespace InzoneDataSyncService.InzoneKioskWebServiceProxy {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="InzoneKioskWebServiceSoap", Namespace="http://tempuri.org/")]
    public partial class InzoneKioskWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private AuthHeader authHeaderValueField;
        
        private System.Threading.SendOrPostCallback GetServerInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetSchemaOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetChangesOperationCompleted;
        
        private System.Threading.SendOrPostCallback ApplyChangesOperationCompleted;
        
        private System.Threading.SendOrPostCallback ServerDateTimeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public InzoneKioskWebService() {
            this.Url = "http://www.inzone.co.nz/InzoneKioskWebService.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public AuthHeader AuthHeaderValue {
            get {
                return this.authHeaderValueField;
            }
            set {
                this.authHeaderValueField = value;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetServerInfoCompletedEventHandler GetServerInfoCompleted;
        
        /// <remarks/>
        public event GetSchemaCompletedEventHandler GetSchemaCompleted;
        
        /// <remarks/>
        public event GetChangesCompletedEventHandler GetChangesCompleted;
        
        /// <remarks/>
        public event ApplyChangesCompletedEventHandler ApplyChangesCompleted;
        
        /// <remarks/>
        public event ServerDateTimeCompletedEventHandler ServerDateTimeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetServerInfo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SyncServerInfo GetServerInfo(SyncSession session) {
            object[] results = this.Invoke("GetServerInfo", new object[] {
                        session});
            return ((SyncServerInfo)(results[0]));
        }
        
        /// <remarks/>
        public void GetServerInfoAsync(SyncSession session) {
            this.GetServerInfoAsync(session, null);
        }
        
        /// <remarks/>
        public void GetServerInfoAsync(SyncSession session, object userState) {
            if ((this.GetServerInfoOperationCompleted == null)) {
                this.GetServerInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetServerInfoOperationCompleted);
            }
            this.InvokeAsync("GetServerInfo", new object[] {
                        session}, this.GetServerInfoOperationCompleted, userState);
        }
        
        private void OnGetServerInfoOperationCompleted(object arg) {
            if ((this.GetServerInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetServerInfoCompleted(this, new GetServerInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetSchema", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SyncSchema GetSchema(string[] tableNames, SyncSession session) {
            object[] results = this.Invoke("GetSchema", new object[] {
                        tableNames,
                        session});
            return ((SyncSchema)(results[0]));
        }
        
        /// <remarks/>
        public void GetSchemaAsync(string[] tableNames, SyncSession session) {
            this.GetSchemaAsync(tableNames, session, null);
        }
        
        /// <remarks/>
        public void GetSchemaAsync(string[] tableNames, SyncSession session, object userState) {
            if ((this.GetSchemaOperationCompleted == null)) {
                this.GetSchemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSchemaOperationCompleted);
            }
            this.InvokeAsync("GetSchema", new object[] {
                        tableNames,
                        session}, this.GetSchemaOperationCompleted, userState);
        }
        
        private void OnGetSchemaOperationCompleted(object arg) {
            if ((this.GetSchemaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSchemaCompleted(this, new GetSchemaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetChanges", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SyncContext GetChanges(SyncGroupMetadata groupMetadata, SyncSession syncSession) {
            object[] results = this.Invoke("GetChanges", new object[] {
                        groupMetadata,
                        syncSession});
            return ((SyncContext)(results[0]));
        }
        
        /// <remarks/>
        public void GetChangesAsync(SyncGroupMetadata groupMetadata, SyncSession syncSession) {
            this.GetChangesAsync(groupMetadata, syncSession, null);
        }
        
        /// <remarks/>
        public void GetChangesAsync(SyncGroupMetadata groupMetadata, SyncSession syncSession, object userState) {
            if ((this.GetChangesOperationCompleted == null)) {
                this.GetChangesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetChangesOperationCompleted);
            }
            this.InvokeAsync("GetChanges", new object[] {
                        groupMetadata,
                        syncSession}, this.GetChangesOperationCompleted, userState);
        }
        
        private void OnGetChangesOperationCompleted(object arg) {
            if ((this.GetChangesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetChangesCompleted(this, new GetChangesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ApplyChanges", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SyncContext ApplyChanges(SyncGroupMetadata groupMetadata, System.Data.DataSet dataSet, SyncSession syncSession) {
            object[] results = this.Invoke("ApplyChanges", new object[] {
                        groupMetadata,
                        dataSet,
                        syncSession});
            return ((SyncContext)(results[0]));
        }
        
        /// <remarks/>
        public void ApplyChangesAsync(SyncGroupMetadata groupMetadata, System.Data.DataSet dataSet, SyncSession syncSession) {
            this.ApplyChangesAsync(groupMetadata, dataSet, syncSession, null);
        }
        
        /// <remarks/>
        public void ApplyChangesAsync(SyncGroupMetadata groupMetadata, System.Data.DataSet dataSet, SyncSession syncSession, object userState) {
            if ((this.ApplyChangesOperationCompleted == null)) {
                this.ApplyChangesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnApplyChangesOperationCompleted);
            }
            this.InvokeAsync("ApplyChanges", new object[] {
                        groupMetadata,
                        dataSet,
                        syncSession}, this.ApplyChangesOperationCompleted, userState);
        }
        
        private void OnApplyChangesOperationCompleted(object arg) {
            if ((this.ApplyChangesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ApplyChangesCompleted(this, new ApplyChangesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ServerDateTime", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ServerDateTime() {
            object[] results = this.Invoke("ServerDateTime", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ServerDateTimeAsync() {
            this.ServerDateTimeAsync(null);
        }
        
        /// <remarks/>
        public void ServerDateTimeAsync(object userState) {
            if ((this.ServerDateTimeOperationCompleted == null)) {
                this.ServerDateTimeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServerDateTimeOperationCompleted);
            }
            this.InvokeAsync("ServerDateTime", new object[0], this.ServerDateTimeOperationCompleted, userState);
        }
        
        private void OnServerDateTimeOperationCompleted(object arg) {
            if ((this.ServerDateTimeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServerDateTimeCompleted(this, new ServerDateTimeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class AuthHeader : System.Web.Services.Protocols.SoapHeader {
        
        private string kioskIDField;
        
        private string passwordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        public string KioskID {
            get {
                return this.kioskIDField;
            }
            set {
                this.kioskIDField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncConflict {
        
        private ConflictType conflictTypeField;
        
        private string errorMessageField;
        
        private SyncStage syncStageField;
        
        private System.Data.DataTable serverChangeField;
        
        private System.Data.DataTable clientChangeField;
        
        /// <remarks/>
        public ConflictType ConflictType {
            get {
                return this.conflictTypeField;
            }
            set {
                this.conflictTypeField = value;
            }
        }
        
        /// <remarks/>
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        public SyncStage SyncStage {
            get {
                return this.syncStageField;
            }
            set {
                this.syncStageField = value;
            }
        }
        
        /// <remarks/>
        public System.Data.DataTable ServerChange {
            get {
                return this.serverChangeField;
            }
            set {
                this.serverChangeField = value;
            }
        }
        
        /// <remarks/>
        public System.Data.DataTable ClientChange {
            get {
                return this.clientChangeField;
            }
            set {
                this.clientChangeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum ConflictType {
        
        /// <remarks/>
        Unknown,
        
        /// <remarks/>
        ErrorsOccurred,
        
        /// <remarks/>
        ClientUpdateServerUpdate,
        
        /// <remarks/>
        ClientUpdateServerDelete,
        
        /// <remarks/>
        ClientDeleteServerUpdate,
        
        /// <remarks/>
        ClientInsertServerInsert,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum SyncStage {
        
        /// <remarks/>
        ReadingSchema,
        
        /// <remarks/>
        CreatingSchema,
        
        /// <remarks/>
        ReadingMetadata,
        
        /// <remarks/>
        CreatingMetadata,
        
        /// <remarks/>
        DeletingMetadata,
        
        /// <remarks/>
        WritingMetadata,
        
        /// <remarks/>
        UploadingChanges,
        
        /// <remarks/>
        DownloadingChanges,
        
        /// <remarks/>
        ApplyingInserts,
        
        /// <remarks/>
        ApplyingUpdates,
        
        /// <remarks/>
        ApplyingDeletes,
        
        /// <remarks/>
        GettingInserts,
        
        /// <remarks/>
        GettingUpdates,
        
        /// <remarks/>
        GettingDeletes,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncTableProgress {
        
        private string tableNameField;
        
        private int insertsField;
        
        private int updatesField;
        
        private int deletesField;
        
        private int changesAppliedField;
        
        private int changesFailedField;
        
        private SyncConflict[] conflictsField;
        
        /// <remarks/>
        public string TableName {
            get {
                return this.tableNameField;
            }
            set {
                this.tableNameField = value;
            }
        }
        
        /// <remarks/>
        public int Inserts {
            get {
                return this.insertsField;
            }
            set {
                this.insertsField = value;
            }
        }
        
        /// <remarks/>
        public int Updates {
            get {
                return this.updatesField;
            }
            set {
                this.updatesField = value;
            }
        }
        
        /// <remarks/>
        public int Deletes {
            get {
                return this.deletesField;
            }
            set {
                this.deletesField = value;
            }
        }
        
        /// <remarks/>
        public int ChangesApplied {
            get {
                return this.changesAppliedField;
            }
            set {
                this.changesAppliedField = value;
            }
        }
        
        /// <remarks/>
        public int ChangesFailed {
            get {
                return this.changesFailedField;
            }
            set {
                this.changesFailedField = value;
            }
        }
        
        /// <remarks/>
        public SyncConflict[] Conflicts {
            get {
                return this.conflictsField;
            }
            set {
                this.conflictsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncGroupProgress {
        
        private string groupNameField;
        
        private SyncTableProgress[] tablesProgressField;
        
        /// <remarks/>
        public string GroupName {
            get {
                return this.groupNameField;
            }
            set {
                this.groupNameField = value;
            }
        }
        
        /// <remarks/>
        public SyncTableProgress[] TablesProgress {
            get {
                return this.tablesProgressField;
            }
            set {
                this.tablesProgressField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncContext {
        
        private SyncGroupProgress groupProgressField;
        
        private int originatorIdField;
        
        private SyncAnchor newAnchorField;
        
        private SyncAnchor maxAnchorField;
        
        private int batchCountField;
        
        private System.Data.DataSet dataSetField;
        
        /// <remarks/>
        public SyncGroupProgress GroupProgress {
            get {
                return this.groupProgressField;
            }
            set {
                this.groupProgressField = value;
            }
        }
        
        /// <remarks/>
        public int OriginatorId {
            get {
                return this.originatorIdField;
            }
            set {
                this.originatorIdField = value;
            }
        }
        
        /// <remarks/>
        public SyncAnchor NewAnchor {
            get {
                return this.newAnchorField;
            }
            set {
                this.newAnchorField = value;
            }
        }
        
        /// <remarks/>
        public SyncAnchor MaxAnchor {
            get {
                return this.maxAnchorField;
            }
            set {
                this.maxAnchorField = value;
            }
        }
        
        /// <remarks/>
        public int BatchCount {
            get {
                return this.batchCountField;
            }
            set {
                this.batchCountField = value;
            }
        }
        
        /// <remarks/>
        public System.Data.DataSet DataSet {
            get {
                return this.dataSetField;
            }
            set {
                this.dataSetField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncAnchor {
        
        private byte[] anchorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Anchor {
            get {
                return this.anchorField;
            }
            set {
                this.anchorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncTableMetadata {
        
        private string tableNameField;
        
        private SyncDirection syncDirectionField;
        
        private SyncAnchor lastSentAnchorField;
        
        private SyncAnchor lastReceivedAnchorField;
        
        /// <remarks/>
        public string TableName {
            get {
                return this.tableNameField;
            }
            set {
                this.tableNameField = value;
            }
        }
        
        /// <remarks/>
        public SyncDirection SyncDirection {
            get {
                return this.syncDirectionField;
            }
            set {
                this.syncDirectionField = value;
            }
        }
        
        /// <remarks/>
        public SyncAnchor LastSentAnchor {
            get {
                return this.lastSentAnchorField;
            }
            set {
                this.lastSentAnchorField = value;
            }
        }
        
        /// <remarks/>
        public SyncAnchor LastReceivedAnchor {
            get {
                return this.lastReceivedAnchorField;
            }
            set {
                this.lastReceivedAnchorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum SyncDirection {
        
        /// <remarks/>
        DownloadOnly,
        
        /// <remarks/>
        UploadOnly,
        
        /// <remarks/>
        Bidirectional,
        
        /// <remarks/>
        Snapshot,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncGroupMetadata {
        
        private string groupNameField;
        
        private SyncAnchor newAnchorField;
        
        private SyncAnchor maxAnchorField;
        
        private int batchCountField;
        
        private SyncTableMetadata[] tablesMetadataField;
        
        /// <remarks/>
        public string GroupName {
            get {
                return this.groupNameField;
            }
            set {
                this.groupNameField = value;
            }
        }
        
        /// <remarks/>
        public SyncAnchor NewAnchor {
            get {
                return this.newAnchorField;
            }
            set {
                this.newAnchorField = value;
            }
        }
        
        /// <remarks/>
        public SyncAnchor MaxAnchor {
            get {
                return this.maxAnchorField;
            }
            set {
                this.maxAnchorField = value;
            }
        }
        
        /// <remarks/>
        public int BatchCount {
            get {
                return this.batchCountField;
            }
            set {
                this.batchCountField = value;
            }
        }
        
        /// <remarks/>
        public SyncTableMetadata[] TablesMetadata {
            get {
                return this.tablesMetadataField;
            }
            set {
                this.tablesMetadataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncSchema {
        
        private System.Data.DataSet schemaDataSetField;
        
        /// <remarks/>
        public System.Data.DataSet SchemaDataSet {
            get {
                return this.schemaDataSetField;
            }
            set {
                this.schemaDataSetField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncTableInfo {
        
        private string tableNameField;
        
        private string descriptionField;
        
        /// <remarks/>
        public string TableName {
            get {
                return this.tableNameField;
            }
            set {
                this.tableNameField = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncServerInfo {
        
        private SyncTableInfo[] tablesInfoField;
        
        /// <remarks/>
        public SyncTableInfo[] TablesInfo {
            get {
                return this.tablesInfoField;
            }
            set {
                this.tablesInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncParameter {
        
        private string nameField;
        
        private object valueField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public object Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SyncSession {
        
        private int originatorIdField;
        
        private System.Guid clientIdField;
        
        private SyncParameter[] customParametersField;
        
        /// <remarks/>
        public int OriginatorId {
            get {
                return this.originatorIdField;
            }
            set {
                this.originatorIdField = value;
            }
        }
        
        /// <remarks/>
        public System.Guid ClientId {
            get {
                return this.clientIdField;
            }
            set {
                this.clientIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CustomParameters")]
        public SyncParameter[] CustomParameters {
            get {
                return this.customParametersField;
            }
            set {
                this.customParametersField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetServerInfoCompletedEventHandler(object sender, GetServerInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetServerInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetServerInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SyncServerInfo Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SyncServerInfo)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetSchemaCompletedEventHandler(object sender, GetSchemaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSchemaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSchemaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SyncSchema Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SyncSchema)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetChangesCompletedEventHandler(object sender, GetChangesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetChangesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetChangesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SyncContext Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SyncContext)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ApplyChangesCompletedEventHandler(object sender, ApplyChangesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ApplyChangesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ApplyChangesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SyncContext Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SyncContext)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ServerDateTimeCompletedEventHandler(object sender, ServerDateTimeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServerDateTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ServerDateTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591