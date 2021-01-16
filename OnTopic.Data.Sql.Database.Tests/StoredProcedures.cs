﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OnTopic.Data.Sql.Database.Tests {
  [TestClass()]
  public class StoredProcedures: SqlDatabaseTestClass {

    public StoredProcedures() {
      InitializeComponent();
    }

    [TestInitialize()]
    public void TestInitialize() {
      base.InitializeTest();
    }
    [TestCleanup()]
    public void TestCleanup() {
      base.CleanupTest();
    }

    #region Designer support code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_CreateTopicTest_TestAction;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoredProcedures));
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition topicTotal;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_DeleteTopicTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_GetTopicVersionTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition3;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_GetTopicsTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition4;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_MoveTopicTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition5;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_UpdateAttributesTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition6;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_UpdateExtendedAttributesTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition7;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_UpdateReferencesTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition8;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_UpdateRelationshipsTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition9;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_UpdateTopicTest_TestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition10;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_CreateTopicTest_PosttestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition deleteCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_DeleteTopicTest_PretestAction;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition initializeTopicCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition postDeleteTopicCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition initializeAttributeCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition initializeRelationshipCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition initializeReferenceCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition postDeleteAttributeCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition postDeleteRelationshipCount;
      Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition postDeleteReferenceCount;
      this.dbo_CreateTopicTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_DeleteTopicTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_GetTopicVersionTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_GetTopicsTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_MoveTopicTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_UpdateAttributesTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_UpdateExtendedAttributesTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_UpdateReferencesTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_UpdateRelationshipsTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      this.dbo_UpdateTopicTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
      dbo_CreateTopicTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      topicTotal = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      dbo_DeleteTopicTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      dbo_GetTopicVersionTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_GetTopicsTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_MoveTopicTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition5 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_UpdateAttributesTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition6 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_UpdateExtendedAttributesTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition7 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_UpdateReferencesTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition8 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_UpdateRelationshipsTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition9 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_UpdateTopicTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      inconclusiveCondition10 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
      dbo_CreateTopicTest_PosttestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      deleteCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      dbo_DeleteTopicTest_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
      initializeTopicCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      postDeleteTopicCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      initializeAttributeCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      initializeRelationshipCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      initializeReferenceCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      postDeleteAttributeCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      postDeleteRelationshipCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      postDeleteReferenceCount = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
      // 
      // dbo_CreateTopicTest_TestAction
      // 
      dbo_CreateTopicTest_TestAction.Conditions.Add(topicTotal);
      resources.ApplyResources(dbo_CreateTopicTest_TestAction, "dbo_CreateTopicTest_TestAction");
      // 
      // topicTotal
      // 
      topicTotal.Enabled = true;
      topicTotal.Name = "topicTotal";
      topicTotal.ResultSet = 1;
      topicTotal.RowCount = 1;
      // 
      // dbo_DeleteTopicTest_TestAction
      // 
      dbo_DeleteTopicTest_TestAction.Conditions.Add(postDeleteTopicCount);
      dbo_DeleteTopicTest_TestAction.Conditions.Add(postDeleteAttributeCount);
      dbo_DeleteTopicTest_TestAction.Conditions.Add(postDeleteRelationshipCount);
      dbo_DeleteTopicTest_TestAction.Conditions.Add(postDeleteReferenceCount);
      resources.ApplyResources(dbo_DeleteTopicTest_TestAction, "dbo_DeleteTopicTest_TestAction");
      // 
      // dbo_GetTopicVersionTest_TestAction
      // 
      dbo_GetTopicVersionTest_TestAction.Conditions.Add(inconclusiveCondition3);
      resources.ApplyResources(dbo_GetTopicVersionTest_TestAction, "dbo_GetTopicVersionTest_TestAction");
      // 
      // inconclusiveCondition3
      // 
      inconclusiveCondition3.Enabled = true;
      inconclusiveCondition3.Name = "inconclusiveCondition3";
      // 
      // dbo_GetTopicsTest_TestAction
      // 
      dbo_GetTopicsTest_TestAction.Conditions.Add(inconclusiveCondition4);
      resources.ApplyResources(dbo_GetTopicsTest_TestAction, "dbo_GetTopicsTest_TestAction");
      // 
      // inconclusiveCondition4
      // 
      inconclusiveCondition4.Enabled = true;
      inconclusiveCondition4.Name = "inconclusiveCondition4";
      // 
      // dbo_MoveTopicTest_TestAction
      // 
      dbo_MoveTopicTest_TestAction.Conditions.Add(inconclusiveCondition5);
      resources.ApplyResources(dbo_MoveTopicTest_TestAction, "dbo_MoveTopicTest_TestAction");
      // 
      // inconclusiveCondition5
      // 
      inconclusiveCondition5.Enabled = true;
      inconclusiveCondition5.Name = "inconclusiveCondition5";
      // 
      // dbo_UpdateAttributesTest_TestAction
      // 
      dbo_UpdateAttributesTest_TestAction.Conditions.Add(inconclusiveCondition6);
      resources.ApplyResources(dbo_UpdateAttributesTest_TestAction, "dbo_UpdateAttributesTest_TestAction");
      // 
      // inconclusiveCondition6
      // 
      inconclusiveCondition6.Enabled = true;
      inconclusiveCondition6.Name = "inconclusiveCondition6";
      // 
      // dbo_UpdateExtendedAttributesTest_TestAction
      // 
      dbo_UpdateExtendedAttributesTest_TestAction.Conditions.Add(inconclusiveCondition7);
      resources.ApplyResources(dbo_UpdateExtendedAttributesTest_TestAction, "dbo_UpdateExtendedAttributesTest_TestAction");
      // 
      // inconclusiveCondition7
      // 
      inconclusiveCondition7.Enabled = true;
      inconclusiveCondition7.Name = "inconclusiveCondition7";
      // 
      // dbo_UpdateReferencesTest_TestAction
      // 
      dbo_UpdateReferencesTest_TestAction.Conditions.Add(inconclusiveCondition8);
      resources.ApplyResources(dbo_UpdateReferencesTest_TestAction, "dbo_UpdateReferencesTest_TestAction");
      // 
      // inconclusiveCondition8
      // 
      inconclusiveCondition8.Enabled = true;
      inconclusiveCondition8.Name = "inconclusiveCondition8";
      // 
      // dbo_UpdateRelationshipsTest_TestAction
      // 
      dbo_UpdateRelationshipsTest_TestAction.Conditions.Add(inconclusiveCondition9);
      resources.ApplyResources(dbo_UpdateRelationshipsTest_TestAction, "dbo_UpdateRelationshipsTest_TestAction");
      // 
      // inconclusiveCondition9
      // 
      inconclusiveCondition9.Enabled = true;
      inconclusiveCondition9.Name = "inconclusiveCondition9";
      // 
      // dbo_UpdateTopicTest_TestAction
      // 
      dbo_UpdateTopicTest_TestAction.Conditions.Add(inconclusiveCondition10);
      resources.ApplyResources(dbo_UpdateTopicTest_TestAction, "dbo_UpdateTopicTest_TestAction");
      // 
      // inconclusiveCondition10
      // 
      inconclusiveCondition10.Enabled = true;
      inconclusiveCondition10.Name = "inconclusiveCondition10";
      // 
      // dbo_CreateTopicTest_PosttestAction
      // 
      dbo_CreateTopicTest_PosttestAction.Conditions.Add(deleteCount);
      resources.ApplyResources(dbo_CreateTopicTest_PosttestAction, "dbo_CreateTopicTest_PosttestAction");
      // 
      // deleteCount
      // 
      deleteCount.Enabled = true;
      deleteCount.Name = "deleteCount";
      deleteCount.ResultSet = 1;
      deleteCount.RowCount = 0;
      // 
      // dbo_CreateTopicTestData
      // 
      this.dbo_CreateTopicTestData.PosttestAction = dbo_CreateTopicTest_PosttestAction;
      this.dbo_CreateTopicTestData.PretestAction = null;
      this.dbo_CreateTopicTestData.TestAction = dbo_CreateTopicTest_TestAction;
      // 
      // dbo_DeleteTopicTestData
      // 
      this.dbo_DeleteTopicTestData.PosttestAction = null;
      this.dbo_DeleteTopicTestData.PretestAction = dbo_DeleteTopicTest_PretestAction;
      this.dbo_DeleteTopicTestData.TestAction = dbo_DeleteTopicTest_TestAction;
      // 
      // dbo_GetTopicVersionTestData
      // 
      this.dbo_GetTopicVersionTestData.PosttestAction = null;
      this.dbo_GetTopicVersionTestData.PretestAction = null;
      this.dbo_GetTopicVersionTestData.TestAction = dbo_GetTopicVersionTest_TestAction;
      // 
      // dbo_GetTopicsTestData
      // 
      this.dbo_GetTopicsTestData.PosttestAction = null;
      this.dbo_GetTopicsTestData.PretestAction = null;
      this.dbo_GetTopicsTestData.TestAction = dbo_GetTopicsTest_TestAction;
      // 
      // dbo_MoveTopicTestData
      // 
      this.dbo_MoveTopicTestData.PosttestAction = null;
      this.dbo_MoveTopicTestData.PretestAction = null;
      this.dbo_MoveTopicTestData.TestAction = dbo_MoveTopicTest_TestAction;
      // 
      // dbo_UpdateAttributesTestData
      // 
      this.dbo_UpdateAttributesTestData.PosttestAction = null;
      this.dbo_UpdateAttributesTestData.PretestAction = null;
      this.dbo_UpdateAttributesTestData.TestAction = dbo_UpdateAttributesTest_TestAction;
      // 
      // dbo_UpdateExtendedAttributesTestData
      // 
      this.dbo_UpdateExtendedAttributesTestData.PosttestAction = null;
      this.dbo_UpdateExtendedAttributesTestData.PretestAction = null;
      this.dbo_UpdateExtendedAttributesTestData.TestAction = dbo_UpdateExtendedAttributesTest_TestAction;
      // 
      // dbo_UpdateReferencesTestData
      // 
      this.dbo_UpdateReferencesTestData.PosttestAction = null;
      this.dbo_UpdateReferencesTestData.PretestAction = null;
      this.dbo_UpdateReferencesTestData.TestAction = dbo_UpdateReferencesTest_TestAction;
      // 
      // dbo_UpdateRelationshipsTestData
      // 
      this.dbo_UpdateRelationshipsTestData.PosttestAction = null;
      this.dbo_UpdateRelationshipsTestData.PretestAction = null;
      this.dbo_UpdateRelationshipsTestData.TestAction = dbo_UpdateRelationshipsTest_TestAction;
      // 
      // dbo_UpdateTopicTestData
      // 
      this.dbo_UpdateTopicTestData.PosttestAction = null;
      this.dbo_UpdateTopicTestData.PretestAction = null;
      this.dbo_UpdateTopicTestData.TestAction = dbo_UpdateTopicTest_TestAction;
      // 
      // dbo_DeleteTopicTest_PretestAction
      // 
      dbo_DeleteTopicTest_PretestAction.Conditions.Add(initializeTopicCount);
      dbo_DeleteTopicTest_PretestAction.Conditions.Add(initializeAttributeCount);
      dbo_DeleteTopicTest_PretestAction.Conditions.Add(initializeRelationshipCount);
      dbo_DeleteTopicTest_PretestAction.Conditions.Add(initializeReferenceCount);
      resources.ApplyResources(dbo_DeleteTopicTest_PretestAction, "dbo_DeleteTopicTest_PretestAction");
      // 
      // initializeTopicCount
      // 
      initializeTopicCount.Enabled = true;
      initializeTopicCount.Name = "initializeTopicCount";
      initializeTopicCount.ResultSet = 1;
      initializeTopicCount.RowCount = 2;
      // 
      // postDeleteTopicCount
      // 
      postDeleteTopicCount.Enabled = true;
      postDeleteTopicCount.Name = "postDeleteTopicCount";
      postDeleteTopicCount.ResultSet = 1;
      postDeleteTopicCount.RowCount = 0;
      // 
      // initializeAttributeCount
      // 
      initializeAttributeCount.Enabled = true;
      initializeAttributeCount.Name = "initializeAttributeCount";
      initializeAttributeCount.ResultSet = 2;
      initializeAttributeCount.RowCount = 4;
      // 
      // initializeRelationshipCount
      // 
      initializeRelationshipCount.Enabled = true;
      initializeRelationshipCount.Name = "initializeRelationshipCount";
      initializeRelationshipCount.ResultSet = 3;
      initializeRelationshipCount.RowCount = 1;
      // 
      // initializeReferenceCount
      // 
      initializeReferenceCount.Enabled = true;
      initializeReferenceCount.Name = "initializeReferenceCount";
      initializeReferenceCount.ResultSet = 4;
      initializeReferenceCount.RowCount = 1;
      // 
      // postDeleteAttributeCount
      // 
      postDeleteAttributeCount.Enabled = true;
      postDeleteAttributeCount.Name = "postDeleteAttributeCount";
      postDeleteAttributeCount.ResultSet = 2;
      postDeleteAttributeCount.RowCount = 0;
      // 
      // postDeleteRelationshipCount
      // 
      postDeleteRelationshipCount.Enabled = true;
      postDeleteRelationshipCount.Name = "postDeleteRelationshipCount";
      postDeleteRelationshipCount.ResultSet = 3;
      postDeleteRelationshipCount.RowCount = 0;
      // 
      // postDeleteReferenceCount
      // 
      postDeleteReferenceCount.Enabled = true;
      postDeleteReferenceCount.Name = "postDeleteReferenceCount";
      postDeleteReferenceCount.ResultSet = 4;
      postDeleteReferenceCount.RowCount = 0;
    }

    #endregion


    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    #endregion

    [TestMethod()]
    public void dbo_CreateTopicTest() {
      SqlDatabaseTestActions testActions = this.dbo_CreateTopicTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_DeleteTopicTest() {
      SqlDatabaseTestActions testActions = this.dbo_DeleteTopicTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_GetTopicVersionTest() {
      SqlDatabaseTestActions testActions = this.dbo_GetTopicVersionTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_GetTopicsTest() {
      SqlDatabaseTestActions testActions = this.dbo_GetTopicsTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_MoveTopicTest() {
      SqlDatabaseTestActions testActions = this.dbo_MoveTopicTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_UpdateAttributesTest() {
      SqlDatabaseTestActions testActions = this.dbo_UpdateAttributesTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_UpdateExtendedAttributesTest() {
      SqlDatabaseTestActions testActions = this.dbo_UpdateExtendedAttributesTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_UpdateReferencesTest() {
      SqlDatabaseTestActions testActions = this.dbo_UpdateReferencesTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_UpdateRelationshipsTest() {
      SqlDatabaseTestActions testActions = this.dbo_UpdateRelationshipsTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }

    [TestMethod()]
    public void dbo_UpdateTopicTest() {
      SqlDatabaseTestActions testActions = this.dbo_UpdateTopicTestData;
      // Execute the pre-test script
      // 
      System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
      SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
      try {
        // Execute the test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
        SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
      }
      finally {
        // Execute the post-test script
        // 
        System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
        SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
      }
    }
    private SqlDatabaseTestActions dbo_CreateTopicTestData;
    private SqlDatabaseTestActions dbo_DeleteTopicTestData;
    private SqlDatabaseTestActions dbo_GetTopicVersionTestData;
    private SqlDatabaseTestActions dbo_GetTopicsTestData;
    private SqlDatabaseTestActions dbo_MoveTopicTestData;
    private SqlDatabaseTestActions dbo_UpdateAttributesTestData;
    private SqlDatabaseTestActions dbo_UpdateExtendedAttributesTestData;
    private SqlDatabaseTestActions dbo_UpdateReferencesTestData;
    private SqlDatabaseTestActions dbo_UpdateRelationshipsTestData;
    private SqlDatabaseTestActions dbo_UpdateTopicTestData;
  }
}
