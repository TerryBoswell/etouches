﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Xml;
using Scribe.Connector.etouches;
using Scribe.Core.ConnectorApi.Actions;

namespace Scribe.Connector.Test
{
    [TestClass]
    public class DataServicesClient_Test
    {
        private const string AccountId = "3037";
        private const string EventId = "86972";
        private const string ApiKey = "0742cb104f8a632bcab90400a3d16b622715b70e";
        private const string BaseUrl = "https://ds-prod.eiseverywhere.com";

        private const string V2BaseUrl = "https://www.eiseverywhere.com";


        private const string TTL = "20";
        private const string PageSize = "1024";

        private string AccessToken = string.Empty;
        Scribe.Connector.etouches.Connector Connector;
        public DataServicesClient_Test()
        {
            Connector = createConnector();
        }

        private etouches.Connector createConnector()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("AccountId", AccountId);
            properties.Add("EventId", EventId);
            properties.Add("ApiKey", ApiKey);
            properties.Add("SubDomain", string.Empty);
            properties.Add("BaseUrl", BaseUrl);
            properties.Add("TTL", TTL);
            properties.Add("PageSize", PageSize);

            properties.Add("V2Url", V2BaseUrl);

            var connector = new Scribe.Connector.etouches.Connector();
            connector.Connect(properties);
            return connector;
        }

        private void VerifyAccessToken()
        {
            
        }

        [TestMethod]
        public void TestCanConnect()
        {
            var connector = createConnector();
            Assert.IsNotNull(connector);
            Assert.AreEqual(connector.IsConnected, true);
            if (Configuration.BidirectionalEnabled)
                Assert.AreEqual(connector.IsV2Connected, true);       
        }

        #region Meta Data Tests
        [TestMethod]
        public void TestGetAttendeeMetaData()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.GetAttendeeMetaData(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestGetRegSessionMetaData()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.GetRegSessionMetaData(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestGetSessionMetaData()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.GetSessionMetaData(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestGetSpeakerMetaData()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.GetSpeakerMetaData(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestGetSessionTrackMetaData()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.GetSessionTrackMetaData(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestGetMeetingMetaData()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.GetMeetingMetaData(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestGetFinancialTransactionMetaData()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.GetMeetingMetaData(Connector.Connection);
            Assert.IsNotNull(data);
        }

        #endregion

        #region List Tests
        [TestMethod]
        public void TestListAttendees()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListAttendees(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListAttendeesWithKeyPairs()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListAttendees(Connector.Connection);
            var key = "category_reference";
            Dictionary<string, string> keyPairs = getTestKeyPairs(data, key);
            data = etouches.DataServicesClient.ListAttendees(Connector.Connection, null, null, keyPairs);
            var table = data.Tables[0];
            Assert.IsTrue(table.Rows.Count > 0);
            var val = table.Rows[0][key].ToString();
            //have to loop so not to take into account data type
            foreach (var row in table.Rows)
            {
                var curVal = ((DataRow)row)[key].ToString();
                Assert.AreEqual(val, curVal);
            }
        }

        [TestMethod]
        public void TestListEvents()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListEvents(Connector.Connection);
            Assert.IsNotNull(data);
            sw.Stop();

        }

        //[Ignore]
        //[TestMethod]
        //public void TestListEvents_TestPaging()
        //{
        //    VerifyAccessToken();
        //    var data = etouches.DataServicesClient.ListEvents(Connector.Connection);
        //    Assert.IsNotNull(data);
        //    var firstCount = data.Tables[0].Rows.Count;
        //    Connector.SetPageSize(1);
        //    //we'll exist in this case because it is no longer a valid test
        //    if (firstCount == 1)
        //        return;
        //    etouches.ConnectorCache.Clear();
        //    data = etouches.DataServicesClient.ListEvents(Connector.Connection);
        //    Assert.IsNotNull(data);
        //    var secondCount = data.Tables[0].Rows.Count;
        //    Assert.AreNotEqual(firstCount, secondCount);
        //}


        [TestMethod]
        public void TestListEventsWithKeyPairs()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListEvents(Connector.Connection);
            var key = "enddate";
            Dictionary<string, string> keyPairs = getTestKeyPairs(data, key);
            data = etouches.DataServicesClient.ListEvents(Connector.Connection, null, null, null, keyPairs);
            var table = data.Tables[0];
            Assert.IsTrue(table.Rows.Count > 0);
            var val = table.Rows[0][key].ToString();
            //have to loop so not to take into account data type
            foreach (var row in table.Rows)
            {
                var curVal = ((DataRow)row)[key].ToString();
                Assert.AreEqual(val, curVal);
            }
        }


        [TestMethod]
        public void TestListEventsWithLessThanDate()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListEvents(Connector.Connection, null, DateTime.Now.AddDays(-30));
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListEventsWithAttendeeDate()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListEvents(Connector.Connection, null, null, DateTime.Now.AddDays(-30));
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListRegSessions()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListRegSessions(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListRegSessionsWithKeyPairs()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListRegSessions(Connector.Connection);
            Assert.IsNotNull(data);
            var key = "fname";
            Dictionary<string, string> keyPairs = getTestKeyPairs(data, key);
            data = etouches.DataServicesClient.ListRegSessions(Connector.Connection, keyPairs);
            var table = data.Tables[0];
            Assert.IsTrue(table.Rows.Count > 0);
            var val = table.Rows[0][key].ToString();
            //have to loop so not to take into account data type
            foreach (var row in table.Rows)
            {
                var curVal = ((DataRow)row)[key].ToString();
                Assert.AreEqual(val, curVal);
            }
        }

        [TestMethod]
        public void TestListSpeakers()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListSpeakers(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListSpeakersWithKeyPairs()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListSpeakers(Connector.Connection);
            Assert.IsNotNull(data);
            var key = "speaker_fname";
            Dictionary<string, string> keyPairs = getTestKeyPairs(data, key);
            data = etouches.DataServicesClient.ListSpeakers(Connector.Connection, keyPairs);
            var table = data.Tables[0];
            Assert.IsTrue(table.Rows.Count > 0);
            var val = table.Rows[0][key].ToString();
            //have to loop so not to take into account data type
            foreach (var row in table.Rows)
            {
                var curVal = ((DataRow)row)[key].ToString();
                Assert.AreEqual(val, curVal); 
            }
        }

        [TestMethod]
        public void TestListSessions()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListSessions(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListSessionsWithKeyPairs()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListSessions(Connector.Connection);
            Assert.IsNotNull(data);
            var key = "sessiondate";
            Dictionary<string, string> keyPairs = getTestKeyPairs(data, key);
            data =  etouches.DataServicesClient.ListSessions(Connector.Connection, keyPairs);            
            var table = data.Tables[0];
            Assert.IsTrue(table.Rows.Count > 0);
            var val = table.Rows[0][key].ToString();
            //have to loop so not to take into account data type
            foreach (var row in table.Rows)
            {
                var curVal = ((DataRow)row)[key].ToString();
                Assert.AreEqual(val, curVal);
            }
        }

        [TestMethod]
        public void TestListSessionTracks()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListSessionTracks(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListSessionTracksWithKeyPairs()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListSessionTracks(Connector.Connection);
            Assert.IsNotNull(data);
            var key = "eventid";
            Dictionary<string, string> keyPairs = getTestKeyPairs(data, key);
            data = etouches.DataServicesClient.ListSessionTracks(Connector.Connection);
            var table = data.Tables[0];
            Assert.IsTrue(table.Rows.Count > 0);
            var val = table.Rows[0][key].ToString();
            //have to loop so not to take into account data type
            foreach (var row in table.Rows)
            {
                var curVal = ((DataRow)row)[key].ToString();
                Assert.AreEqual(val, curVal);
            }
        }


        [TestMethod]
        public void TestListMeetings()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListMeetings(Connector.Connection);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestListMeetingsWithKeyPairs()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListMeetings(Connector.Connection);
            Assert.IsNotNull(data);
            var key = "eventid";
            Dictionary<string, string> keyPairs = getTestKeyPairs(data, key);
            data = etouches.DataServicesClient.ListMeetings(Connector.Connection);
            if (data.Tables.Count == 0)
                return;
            var table = data.Tables[0];
            Assert.IsTrue(table.Rows.Count > 0);
            var val = table.Rows[0][key].ToString();
            //have to loop so not to take into account data type
            foreach (var row in table.Rows)
            {
                var curVal = ((DataRow)row)[key].ToString();
                Assert.AreEqual(val, curVal);
            }
        }


        private Dictionary<string, string> getTestKeyPairs(DataSet data, string colName)
        {
            Dictionary<string, string> keyPairs = new Dictionary<string, string>();
            if (data.Tables.Count > 0 && data.Tables[0].Columns.Count > 1 && data.Tables[0].Rows.Count > 1)
            {
                string value = string.Empty;
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    value = data.Tables[0].Rows[i][colName].ToString();
                    if (!String.IsNullOrEmpty(value))
                        break;
                }
                keyPairs.Add(colName, value);
            }
            return keyPairs;
        }

        [TestMethod]
        public void TestListFinancialTransactionss()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.ListFinancialTransactions(Connector.Connection);
            Assert.IsNotNull(data);
        }
        #endregion

        #region Post Tests

        [TestMethod]
        public void TestCreateEvent()
        {
            VerifyAccessToken();
            var data = etouches.DataServicesClient.CreateEvent(Connector.V2Connection, Guid.NewGuid().ToString());
            Assert.IsNotNull(data);
            Assert.IsTrue(data.EventId > 0);
        }

        [TestMethod]
        public void TestCreateAndUpdateEvent()
        {
            VerifyAccessToken();
            var name = Guid.NewGuid().ToString();
            var data = etouches.DataServicesClient.CreateEvent(Connector.V2Connection, name);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.EventId > 0);
            data = etouches.DataServicesClient.UpdateEvent(Connector.V2Connection, data.EventId, name, name, "2016-01-01", "2017-12-12", "Test", "Some Guy", string.Empty, string.Empty);
            Assert.IsFalse(data.HasError);
        }


        [TestMethod]
        public void TestCreateEvent_ViaConnector()
        {
            var evtObjec = JsonDataFileHelper.GetObjectFromDisk<Core.ConnectorApi.DataEntity>("Event_OperationInput_.json");
            MethodInput input = new MethodInput();
            input.Name = "Create";
            input.IsTestMethod = false;
            input.Input = evtObjec;
            Connector.ExecuteMethod((MethodInput)input);

        }

        #endregion
    }
}
