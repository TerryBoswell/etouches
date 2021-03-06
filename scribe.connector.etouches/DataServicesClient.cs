﻿using EasyHttp.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scribe.Core.ConnectorApi.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections.Concurrent;
using Scribe.Connector.etouches.V2Results;

namespace Scribe.Connector.etouches
{
    
    public static class DataServicesClient
    {

        public static string Authorize(string baseUrl, string accountId, string apiKey)
        {
            var http = new HttpClient();
            var uri = new UriBuilder(baseUrl);
            uri.Path = "authenticate"; 

            var result = http.Get(uri.ToString(), new { accountid = accountId, key = apiKey });
            var json = JObject.Parse(result.RawText);
            if (((string)json["status"]).ToLower() == "error")
            {
                Logger.WriteError((string)json["msg"]);
                throw new ApplicationException((string)json["msg"]);
            }
            if (!String.IsNullOrEmpty((string)json["accesstoken"])) 
                return json["accesstoken"].ToString();
            throw new ApplicationException(result.RawText);
        }

        #region Meta Data
        ///eventmetadata/
        public static JObject GetEventMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.EventMeta, connection.AccountId);            
        }

        ///sessionmetadata/
        public static JObject GetSessionMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.SessionMeta, connection.AccountId, connection.EventId);            
        }

        ///attendeemetadata/
        public static JObject GetAttendeeMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.AttendeeMeta, connection.AccountId, connection.EventId);            
        }
        ///regsessionmetadata/        
        public static JObject GetRegSessionMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.RegSessionMeta, connection.AccountId, connection.EventId);            
        }

        ///speakermetadata/
        public static JObject GetSpeakerMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.SpeakerMeta, connection.AccountId, connection.EventId);
        }

        /*
        GET /sessiontrackmetadata/[accountid]/[eventid] accountid
        */
        public static JObject GetSessionTrackMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.SessionTrackMeta, connection.AccountId, connection.EventId);
        }


        /*
        GET /meetingmetadata/[accountid]/[eventid] accountid
        */
        public static JObject GetMeetingMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.MeetingMeta, connection.AccountId, connection.EventId);
        }

        /*
        GET /financialtransactionmetadata/[accountid]/[eventid] accountid
        */
        public static JObject GetFinancialTransactionMetaData(ScribeConnection connection)
        {
            return DataUtility.GetJObject(connection, Extensions.Actions.FinancialTransactionMeta, connection.AccountId, connection.EventId);
        }
        #endregion 

        #region List Data
        /*
        /eventlist/[accountid]*
        deleted (optional)
        lastmodified-gt
        (optional)
        lastmodified-lt (optional)
        attendees_modified-gt
        (optional)
        attendees_modified-lt
        (optional)
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListEvents(ScribeConnection connection, 
            DateTime? modifiedAfter = null, DateTime? modifiedBefore = null, DateTime? attendeesModifiedAfter = null,
            Dictionary<string, string> keypairs = null)
        {
            string aQuery = string.Empty;
            if (attendeesModifiedAfter.HasValue)
            {
                var d = attendeesModifiedAfter.Value.ToString("yyyy-MM-dd");
                aQuery = $"attendees_modified-gt={d}";
            }
            return DataUtility.GetDataset(connection, Extensions.Actions.Event, null, modifiedAfter, modifiedBefore, 
                aQuery, keypairs);            
        }

        /*
        /attendeelist/[accountid]/[eventid]*
        deleted (optional)
        lastmodified-gt
        (optional)
        lastmodified-lt (optional)
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListAttendees(ScribeConnection connection, DateTime? modifiedAfter = null, DateTime? modifiedBefore = null,
            Dictionary<string, string> keypairs = null)
        {
            return DataUtility.GetDataset(connection, Extensions.Actions.Attendee, connection.EventId, 
                modifiedAfter, modifiedBefore, null, keypairs);
        }
        /*
        /regsessionlist/[accountid]/[eventid]*
        deleted (optional)
        lastmodified-gt
        (optional)
        lastmodified-lt (optional)
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListRegSessions(ScribeConnection connection,
            Dictionary<string, string> keypairs = null)
        {
            return DataUtility.GetDataset(connection, Extensions.Actions.RegSession, connection.EventId, null, null, null, keypairs);
        }

        /*
        /speakerlist/[accountid]/[eventid] *
        deleted (optional)
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListSpeakers(ScribeConnection connection,
            Dictionary<string, string> keypairs = null)
        {
            return DataUtility.GetDataset(connection, Extensions.Actions.Speaker, connection.EventId, null, null, null, keypairs);
        }


        /*
        /sessionlist/[accountid]/[eventid] *
        deleted (optional)
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListSessions(ScribeConnection connection,
            Dictionary<string, string> keypairs = null)
        {
            return DataUtility.GetDataset(connection, Extensions.Actions.Session, connection.EventId, null, null, null, keypairs);
            
        }

        /*
        /sessiontracklist/[accountid]/[eventid] *
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListSessionTracks(ScribeConnection connection,
            Dictionary<string, string> keypairs = null)
        {
            return DataUtility.GetDataset(connection, Extensions.Actions.SessionTrack, connection.EventId, null, null, null, keypairs);
        }

        /*
        /meetinglist/[accountid]/[eventid] *
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListMeetings(ScribeConnection connection,
            Dictionary<string, string> keypairs = null)
        {
            return DataUtility.GetDataset(connection, Extensions.Actions.Meeting, connection.EventId, null, null, null, keypairs);
        }

        /*
        /financialtransactionlist/[accountid]/[eventid] accountid
        accesstoken
        deleted (optional)
        fields (optional)
        pageNumber (optional)
        pageSize (optional)
        */
        public static DataSet ListFinancialTransactions(ScribeConnection connection,
            Dictionary<string, string> keypairs = null)
        {
            return DataUtility.GetDataset(connection, Extensions.Actions.FinanacialTransaction, connection.EventId, null, null, null, keypairs);
        }

        #endregion


        #region Post/Put Events

        /*
        https://www.eiseverywhere.com/api/v2/ereg/createEvent.format
            Parameters

            accesstoken
            required	The access token assigned to you from the authorize function.
            Example: Ub57+fu2p/4KcaqIRNUNQD9HV8nWid+7flRBxbskZIv/DWkaZG0+e6H6ZcjPaAoAGqOtjSq4tHh9ChxUChZdZw==
            name
            required	The name to create the new event with.
            folder
            optional	The ID of folder to create the event in. Must be a numeric value.
            modules
            optional	Array containing the names of modules to turn on for the event. Valid values are (case sensitive): 'eRFP', 'eBudget', 'eProject', 'eScheduler', 'eWiki', 'eHome', 'eMobile', 'eSelect', 'eReg', 'eBooth', 'eConnect', 'eSocial', 'eSeating'. rReg is always on by default.
        */
        public static EventResult CreateEvent(ScribeConnection connection, string name)
        {
            Dictionary<string, string> keyPairs = new Dictionary<string, string>();
            keyPairs.Add("name", name);

            var e = DataUtility.DoPost<EventResult>(connection, Extensions.Actions.EventCreate, string.Empty, keyPairs);

            return e;
        }

        /*
         * https://www.eiseverywhere.com/api/v2/ereg/updateEvent.json
         accesstoken
            required	The access token assigned to you from the authorize function.
            Example: Ub57+fu2p/4KcaqIRNUNQD9HV8nWid+7flRBxbskZIv/DWkaZG0+e6H6ZcjPaAoAGqOtjSq4tHh9ChxUChZdZw==
            eventid
            required	The event ID to put the data into, must be a numeric value.
            name
            optional	Update the event’s name.
            eventid
            optional	Update the event’s code.
            startdate
            optional	Update the event’s start date, dates need to be in ISO format.
            Example: 2012-12-31
            enddate
            optional	Update the event’s end date, dates need to be in ISO format.
            Example: 2012-12-31
            locationname
            optional	Update the event’s location name.
            programmanager
            optional	Update the event’s program manager.
            status
            optional	Update event status, allowed values: Archived, Cancelled, Closed, Live, On-site, Pre-Event, Sold Out
            max_reg 
            optional	Update "Maximum registrations" value
         */
        public static EventResult UpdateEvent(ScribeConnection connection, int eventId, string name, string code,
            string startDate, string endDate, string locationName, string programManager, string status, 
            string max_reg)
        {
            Dictionary<string, string> keyPairs = new Dictionary<string, string>();
            keyPairs.Add("eventid", eventId.ToString());
            if (!name.IsNullOrEmpty())
                keyPairs.Add("name", name);
            if (!code.IsNullOrEmpty())
                keyPairs.Add("code", code);
            if (!startDate.IsNullOrEmpty())
                keyPairs.Add("startdate", startDate);
            if (!endDate.IsNullOrEmpty())
                keyPairs.Add("enddate", endDate);
            if (!locationName.IsNullOrEmpty())
                keyPairs.Add("locationname", locationName);
            if (!programManager.IsNullOrEmpty())
                keyPairs.Add("programmanager", programManager);
            if (!status.IsNullOrEmpty())
                keyPairs.Add("status", status);
            if (!max_reg.IsNullOrEmpty())
                keyPairs.Add("max_reg", max_reg);

            var e = DataUtility.DoPut<EventResult>(connection, Extensions.Actions.EventUpdate, string.Empty, keyPairs);

            return e;
        }
        #endregion
    }


}
