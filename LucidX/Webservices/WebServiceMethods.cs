using System;
using System.Threading.Tasks;
using LucidX.RequestModels;
using LucidX.ResponseModels;
using LucidX.Constants;
using System.Net.Http;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LucidX.Webservices
{
    public class WebServiceMethods
    {
        public async static Task<LoginResponse> Login(string username, string password)
        {
            try
            {

                LoginAPIParams param = new LoginAPIParams
                {
                    userID = username,
                    strPwd = password,
                    connectionName = WebserviceConstants.CONNECTION_NAME

                };

                FinalResponse response = await WebServiceHandler.GetWebserviceResult(WebserviceConstants.LOGIN_URL,
                    HttpMethod.Post, param) as FinalResponse;
                var responseLst = response.ResultDoc as XmlNode[];
                bool isAuthenticate = false;
                isAuthenticate = Convert.ToBoolean(responseLst.FirstOrDefault(x => x.Name == "isAuthenticate").InnerText);

                LoginResponse loginInfo = null;
                if (isAuthenticate)
                {
                    loginInfo = new LoginResponse();

                    loginInfo.IsAuthenticate = isAuthenticate;
                    XmlNode node = responseLst[4];

                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        XmlNode childNode = node.ChildNodes[i];
                        if (i == 0)
                        {
                            loginInfo.UserId = childNode.InnerText.ToString();
                        }
                        else if (i == 1)
                        {
                            loginInfo.Name = childNode.InnerText.ToString();
                        }
                        else if (i == 2)
                        {
                            loginInfo.UserCompCode = childNode.InnerText.ToString();
                        }
                        else if (i == 6)
                        {
                            loginInfo.UserEmail = childNode.InnerText.ToString();
                        }
                    }
                }

                return loginInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async static Task<EmailCountResponse> EmailCount(string userId)
        {
            try
            {
                EmailCountsAPIParams param = new EmailCountsAPIParams
                {
                    intUserID = userId,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };

                var response = await WebServiceHandler.GetWebserviceResult(WebserviceConstants.EMAIL_COUNT_URL,
                    HttpMethod.Post, param) as FinalResponse;

                EmailCountResponse emailCount = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;

                    Dictionary<string, int> countDictionary = new Dictionary<string, int>();

                    string key = null;
                    int value = 0;

                    foreach (DataTable dt in resultIds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.ColumnName == "eMailTypeName")
                                {
                                    key = dr[dc].ToString();
                                }
                                if (dc.ColumnName == "emailcount")
                                {
                                    value = Convert.ToInt32(dr[dc].ToString());
                                }
                            }
                            countDictionary.Add(key, value);
                        }
                    }

                    emailCount = new EmailCountResponse();
                    emailCount.inboxCount = countDictionary["InBox"];
                    emailCount.draftCount = countDictionary["Drafts"];
                    emailCount.sentItemCount = countDictionary["Sent Items"];
                    emailCount.trashCount = countDictionary["Trash"];


                }

                return emailCount;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async static Task<bool> MarkReadEmail(string mailId)
        {
            try
            {

                MarkReadEmailApiParams param = new MarkReadEmailApiParams
                {
                    mailId = mailId,
                    read = true,
                    connectionName = WebserviceConstants.CONNECTION_NAME

                };

                FinalResponse response = await WebServiceHandler.GetWebserviceResult(WebserviceConstants.MARK_READ_EMAIL_URL,
                    HttpMethod.Post, param) as FinalResponse;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async static Task<List<EmailResponse>> InboxEmails(string userId, int mailTypeId)
        {
            List<EmailResponse> emailList = new List<EmailResponse>();
            try
            {
                InboxEmailApiParams param = new InboxEmailApiParams
                {
                    mailCount = 10,
                    mailTypeId = mailTypeId,
                    filterIndex = 0,
                    filterEmail = "",
                    blnShowblank = false,
                    intUserID = userId,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(WebserviceConstants.SHOW_INBOX_EMAILS_URL,
                    HttpMethod.Post, param) as FinalResponse;



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            EmailResponse emailResponse = new EmailResponse();
                            emailResponse.MailId = dr["MailId"] != DBNull.Value ? dr["MailId"].ToString() : "";
                            emailResponse.DisplayDate = dr["DisplayDate"] != DBNull.Value ? dr["DisplayDate"].ToString() : "";
                            emailResponse.myGroup = dr["myGroup"] != DBNull.Value ? dr["myGroup"].ToString() : "";
                            emailResponse.Received = dr["Received"] != DBNull.Value ? dr["Received"].ToString() : "";
                            emailResponse.FolderName = dr["FolderName"] != DBNull.Value ? dr["FolderName"].ToString() : "";
                            emailResponse.AccountCode = dr["AccountCode"] != DBNull.Value ? dr["AccountCode"].ToString() : "";
                            emailResponse.Sender = dr["Sender"] != DBNull.Value ? dr["Sender"].ToString() : "";
                            emailResponse.SenderName = dr["SenderName"] != DBNull.Value ? dr["SenderName"].ToString() : "";
                            emailResponse.Subject = dr["Subject"] != DBNull.Value ? dr["Subject"].ToString() : "";
                            emailResponse.eMailTypeId = dr["eMailTypeId"] != DBNull.Value ? Convert.ToInt32(dr["eMailTypeId"]) : 0;
                            emailResponse.Unread = dr["Unread"] != DBNull.Value ? Convert.ToBoolean(dr["Unread"]) : false;
                            emailResponse.Important = dr["Important"] != DBNull.Value ? Convert.ToBoolean(dr["Important"]) : false;
                            emailResponse.Attachment = dr["Attachment"] != DBNull.Value ? Convert.ToInt32(dr["Attachment"]) : 0;
                            emailResponse.SenderEmail = dr["SenderEmail"] != DBNull.Value ? dr["SenderEmail"].ToString() : "";
                            emailList.Add(emailResponse);
                        }
                    }
                }

                return emailList;
            }
            catch (Exception ex)
            {
                return emailList;
            }
        }


        public async static Task<string> EmailDetail(string mailId, string userId)
        {
            string emailDetail = null;
            try
            {

                EmailDetailsAPIParams param = new EmailDetailsAPIParams
                {
                    MailID = mailId,
                    uid = userId,
                    connectionName = WebserviceConstants.CONNECTION_NAME

                };

                var response = await WebServiceHandler.GetWebserviceResult(WebserviceConstants.EMAIL_DETAILS_URL,
                    HttpMethod.Post, param) as FinalResponse;

                var responseLst = response.ResultDoc as XmlNode[];
                emailDetail = responseLst.FirstOrDefault(x => x.Name == "retValue").InnerText;


                return emailDetail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete Email
        /// </summary>
        /// <param name="delete Email ApiParams"></param>
        /// <returns></returns>
        public async static Task<bool> DeleteEmail(string emailId, string uId)
        {
            try
            {
                EmailDetailsAPIParams deleteEmailApiParam = new EmailDetailsAPIParams
                {
                    MailID = emailId,
                    uid = uId,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };

                FinalResponse response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.DELETE_EMAIL_URL,
                    HttpMethod.Post, deleteEmailApiParam) as FinalResponse;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Returns list of ShowNotes
        /// </summary>
        /// <param name="entityCode"></param>
        /// <param name="accountCode"></param>
        /// <param name="blnShowblank"></param>
        /// <returns></returns>
        public async static Task<List<CrmNotesResponse>> ShowNotes(string entityCode,
            string accountCode, string startDate, string endDate)
        {
            try
            {
                CrmNotesAPIParams param = new CrmNotesAPIParams
                {
                    entityCode = entityCode,
                    accountCode = accountCode,
                    startDate = startDate,
                    endDate = endDate,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.SHOW_NOTES_LIST_URL, HttpMethod.Post, param)
                    as FinalResponse;

                List<CrmNotesResponse> notesList = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        notesList = (from DataRow dr in dt.Rows
                                     select new CrmNotesResponse()
                                     {
                                         NotesId = dr["NotesId"].ToString(),
                                         CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString()),
                                         NotesSubject = dr["NotesSubject"].ToString(),
                                         NotesDetail = dr["NotesDetail"].ToString(),
                                         CreatedBy = dr["CreatedBy"].ToString(),
                                         UserName = dr["UserName"].ToString(),
                                         ImageType = dr["ImageType"].ToString(),
                                         ActionTypeId = dr["ActionTypeId"].ToString(),
                                         SendTo = dr["SendTo"].ToString(),
                                         PrivacyId = dr["PrivacyId"].ToString(),
                                         CreatedBy1 = dr["CreatedBy"].ToString()

                                     }).ToList();
                    }
                }

                return notesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns list of Entity codes
        /// </summary>
        /// <returns></returns>
        public async static Task<List<EntityCodesResponse>> GetEntityCode(int _userId, string _compCode)
        {
            try
            {
                EntityCodeAPIParams param = new EntityCodeAPIParams
                {
                    userId = _userId,
                    compCode = _compCode,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.GET_ENTITY_CODES_URL, HttpMethod.Post, param)
                    as FinalResponse;

                List<EntityCodesResponse> entityCodesList = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        entityCodesList = (from DataRow dr in dt.Rows
                                           select new EntityCodesResponse()
                                           {
                                               CompCode = dr["CompCode"].ToString(),
                                               CompName = dr["CompName"].ToString()
                                           }).ToList();
                    }
                }

                return entityCodesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Returns list of Account codes
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns></returns>
        public async static Task<List<AccountCodesResponse>> GetAccountCodes(string compCode)
        {
            try
            {
                AccountCodeAPIParams param = new AccountCodeAPIParams
                {
                    compCode = compCode,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.GET_ACCOUNT_CODES_URL, HttpMethod.Post, param)
                    as FinalResponse;

                List<AccountCodesResponse> accountCodesList = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        accountCodesList = (from DataRow dr in dt.Rows
                                            select new AccountCodesResponse()
                                            {
                                                AccountCode = dr["AccountCode"].ToString(),
                                                AccountId = dr["AccountId"].ToString(),
                                                AccountName = dr["AccountName"].ToString()

                                            }).ToList();
                    }
                }

                return accountCodesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Save Add notes
        /// </summary>
        /// <param name="addNotesApiParams"></param>
        /// <returns></returns>
        public async static Task<int> AddCrmNotes(AddNotesAPIParams addNotesApiParams)
        {
            try
            {
                FinalResponse response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.ADD_NOTES_URL,
                    HttpMethod.Post, addNotesApiParams) as FinalResponse;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //int notesId = response.ResultDoc;
                    return 1;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="deleteNotesApiParams"></param>
        /// <returns></returns>
        public async static Task<bool> DeleteCrmNotes(string notesId)
        {
            try
            {
                DeleteNotesAPIParams deleteNotesApiParam = new DeleteNotesAPIParams
                {
                    notesId = notesId,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };

                FinalResponse response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.DELETE_NOTES_URL,
                    HttpMethod.Post, deleteNotesApiParam) as FinalResponse;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns list of Reference Users
        /// </summary>

        /// <returns></returns>
        public async static Task<List<RefUsersResponse>> ShowRefUsers(string userCompCode)
        {
            try
            {
                RefUsersAPIParams param = new RefUsersAPIParams
                {
                    connectionName = WebserviceConstants.CONNECTION_NAME,
                    compcode = userCompCode
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.SHOW_REF_USERS_URL, HttpMethod.Post, param)
                    as FinalResponse;

                List<RefUsersResponse> refUsersList = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        refUsersList = (from DataRow dr in dt.Rows
                                        select new RefUsersResponse()
                                        {
                                            UserID = dr["UserID"].ToString(),
                                            UserName = dr["UserName"].ToString(),
                                            UserTitle = dr["UserTitle"].ToString(),
                                            UserEmail = dr["UserEmail"].ToString(),
                                            UserPassword = dr["UserPassword"].ToString(),
                                            RoleId = dr["RoleId"].ToString(),
                                            UserStartPage = dr["UserStartPage"].ToString(),
                                            AccountExpiry = dr["AccountExpiry"].ToString(),
                                            CultureInfo = dr["CultureInfo"].ToString(),
                                            TimeZoneUTC = dr["TimeZoneUTC"].ToString()

                                        }).ToList();
                    }
                }

                return refUsersList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Returns list of Notes type
        /// </summary>

        /// <returns></returns>
        public async static Task<List<NotesTypeResponse>> ShowNotesType()
        {
            try
            {
                ShowNotesTypeAPIParams param = new ShowNotesTypeAPIParams
                {
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.SHOW_NOTES_TYPES, HttpMethod.Post, param)
                    as FinalResponse;

                List<NotesTypeResponse> notesTypeList = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        notesTypeList = (from DataRow dr in dt.Rows
                                         select new NotesTypeResponse()
                                         {
                                             NotesTypeId = Convert.ToInt32(dr["NotesTypeId"].ToString()),
                                             NotesTypeName = dr["NotesTypeName"].ToString().Trim(),
                                             IsSystem = Convert.ToBoolean(dr["IsSystem"].ToString()),
                                             eResourceNo = dr["eResourceNo"].ToString(),
                                             IsSelected = dr["NotesTypeName"].ToString().Trim().
                                                Equals("Systems Calendar") ? false : true
                                         }).ToList();
                    }
                }

                return notesTypeList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Returns list of Notes type
        /// </summary>

        /// <returns></returns>
        public async static Task<List<CalendarEventResponse>> GetCalendarEvents(
            string assignedTo,
            string calendarType,
            string startDate, string endDate)
        {
            try
            {
                CalendarEventsAPIParams param = new CalendarEventsAPIParams
                {
                    assignedTo = assignedTo,
                    startDate = startDate,
                    endDate = endDate,
                    calendarType = calendarType,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.GET_CALENDAR_EVENTS, HttpMethod.Post, param)
                    as FinalResponse;

                List<CalendarEventResponse> eventResponseList = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        eventResponseList = (from DataRow dr in dt.Rows
                                             select new CalendarEventResponse()
                                             {
                                                 EntryId = dr["EntryId"].ToString(),
                                                 CompCode = dr["CompCode"].ToString(),
                                                 AccountCode = dr["AccountCode"].ToString(),
                                                 NotesTypeId = dr["NotesTypeId"].ToString(),
                                                 EntryTypeId = Convert.ToInt32(dr["EntryTypeId"].ToString()),
                                                 DateStart = Convert.ToDateTime(dr["DateStart"].ToString()),
                                                 DateEnd = Convert.ToDateTime(dr["DateEnd"].ToString()),
                                                 Subject = dr["Subject"].ToString(),
                                                 Details = dr["Details"].ToString(),
                                                 AssignedTo = dr["AssignedTo"].ToString(),
                                                 Completed = dr["Completed"].ToString(),
                                                 IsPublic = Convert.ToBoolean(dr["IsPublic"].ToString()),
                                                 AccountId = dr["AccountId"].ToString()

                                             }).ToList();
                    }
                }

                return eventResponseList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Save Add Calendar Events
        /// </summary>
        /// <param name="addEventsApiParams"></param>
        /// <returns></returns>
        public async static Task<bool> AddCalendarEvents(AddCalendarEventsAPIParams addEventsApiParams)
        {
            try
            {
                FinalResponse response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.SAVE_CALENDAR_EVENTS,
                    HttpMethod.Post, addEventsApiParams) as FinalResponse;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //int notesId = response.ResultDoc;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="deleteEventsApiParams"></param>
        /// <returns></returns>
        public async static Task<bool> DeleteEvents(string eventId)
        {
            try
            {
                DeleteEventsAPIParams deleteEventsApiParam = new DeleteEventsAPIParams
                {
                    entryId = eventId,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };

                FinalResponse response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.DELETE_EVENTS_URL,
                    HttpMethod.Post, deleteEventsApiParam) as FinalResponse;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// Returns list of Orders
        /// </summary>

        /// <returns></returns>
        public async static Task<List<LedgerOrder>> GetOrders(
            string processedBy,
            string startDate,
            string endDate)
        {
            try
            {
                OrdersAPIParams param = new OrdersAPIParams
                {
                    startDate = startDate,
                    endDate = endDate,
                    processedBy = processedBy,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.GET_LEDGER_ORDERS, HttpMethod.Post, param)
                    as FinalResponse;

                List<LedgerOrder> ledgerOrderResponseList = new List<LedgerOrder>();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            LedgerOrder ledgerOrderResponse = new LedgerOrder();

                            ledgerOrderResponse.CompCode = dr["CompCode"] != DBNull.Value ?
                                Convert.ToInt32(dr["CompCode"].ToString()) : 0;
                            ledgerOrderResponse.AccountCode = dr["AccountCode"] != DBNull.Value ?
                                dr["AccountCode"].ToString() : "";
                            ledgerOrderResponse.AccountId = dr["AccountID"] != DBNull.Value ?
                                Convert.ToInt32(dr["AccountID"].ToString()) : 0;
                            ledgerOrderResponse.AccountName = dr["AccountName"] != DBNull.Value ?
                                dr["AccountName"].ToString() : "";
                            ledgerOrderResponse.LineDescription = dr["LineDescription"] != DBNull.Value ?
                                dr["LineDescription"].ToString() : "";
                            ledgerOrderResponse.JournalNo = dr["JournalNo"] != DBNull.Value ?
                                Convert.ToInt32(dr["JournalNo"].ToString()) : 0;
                            ledgerOrderResponse.JournalLine = dr["JournalLine"] != DBNull.Value ?
                                Convert.ToInt32(dr["JournalLine"].ToString()) : 0;
                            ledgerOrderResponse.TransactionReference = dr["OrderName"] != DBNull.Value ?
                                dr["OrderName"].ToString() : "";
                            ledgerOrderResponse.TransDate = dr["TransDate"] != DBNull.Value ?
                                dr["TransDate"].ToString() : "";
                            ledgerOrderResponse.ISForexRecord = dr["ISForexRecord"] != DBNull.Value ?
                             Convert.ToBoolean(dr["ISForexRecord"].ToString()) : false;
                            ledgerOrderResponse.CompleteTotal = dr["CompleteTotal"] != DBNull.Value ?
                                Convert.ToDecimal(dr["CompleteTotal"].ToString()) : 0;

                            ledgerOrderResponseList.Add(ledgerOrderResponse);

                        }
                    }
                }

                return ledgerOrderResponseList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Returns list of account orders
        /// </summary>

        /// <returns></returns>
        public async static Task<List<AccountOrdersResponse>> GetAccountForOrders()
        {
            try
            {
                AccountOrdersAPIParams param = new AccountOrdersAPIParams
                {
                    accountTypeID = 6,
                    compCode = 0,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.GET_ACCOUNT_FOR_ORDERS, HttpMethod.Post, param)
                    as FinalResponse;

                List<AccountOrdersResponse> accountOrderResponseList = new List<AccountOrdersResponse>(); ;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                AccountOrdersResponse accountOrderResponse = new AccountOrdersResponse();
                                accountOrderResponse.CompCode = dr["CompCode"] != DBNull.Value ?
                                    Convert.ToInt32(dr["CompCode"].ToString()) : 0;
                                accountOrderResponse.AccountCode = dr["AccountCode"] != DBNull.Value ?
                                    dr["AccountCode"].ToString() : "";
                                accountOrderResponse.AccountId = dr["AccountID"] != DBNull.Value ?
                                    Convert.ToInt32(dr["AccountID"].ToString()) : 0;
                                accountOrderResponse.AccountName = dr["AccountName"] != DBNull.Value ?
                                    dr["AccountName"].ToString() : "";
                                accountOrderResponse.StateID = dr["StateID"] != DBNull.Value ? Convert.ToInt32(dr["StateID"].ToString()) : 0;
                                accountOrderResponse.CountryCode = dr["CountryCode"] != DBNull.Value ?
                                    dr["CountryCode"].ToString() : "";
                                accountOrderResponse.City = dr["City"] != DBNull.Value ?
                                    dr["City"].ToString() : "";
                                accountOrderResponse.ContactPerson = dr["ContactPerson"] != DBNull.Value ?
                                    dr["ContactPerson"].ToString() : "";
                                accountOrderResponse.Telephone = dr["Telephone"] != DBNull.Value ?
                                    dr["Telephone"].ToString() : "";


                                accountOrderResponseList.Add(accountOrderResponse);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }

                return accountOrderResponseList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Returns list of user currency
        /// </summary>
        /// <returns></returns>
        public async static Task<ShowUserCurrencyResponse> GetUserCurrencyFromCountryCode(string countryCode)
        {
            try
            {
                ShowUserCurrencyAPIParams param = new ShowUserCurrencyAPIParams
                {
                    countryCode = countryCode,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.GET_USER_CURRENCY_FROM_COUNTRY_CODE, HttpMethod.Post, param)
                    as FinalResponse;

                List<ShowUserCurrencyResponse> userCurrencyResponseList = new List<ShowUserCurrencyResponse>(); ;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                ShowUserCurrencyResponse userCurrencyResponse = new ShowUserCurrencyResponse();
                                userCurrencyResponse.CountryCode = dr["CountryCode"] != DBNull.Value ?
                                    dr["CountryCode"].ToString() : "";
                                userCurrencyResponse.CurrencyCode = dr["CurrencyCode"] != DBNull.Value ?
                                    dr["CurrencyCode"].ToString() : "";
                                userCurrencyResponse.CurrencyCodeID = dr["CurrencyCodeID"] != DBNull.Value ?
                                    Convert.ToInt32(dr["CurrencyCodeID"].ToString()) : 0;
                                userCurrencyResponse.CurrencyUnit = dr["CurrencyUnit"] != DBNull.Value ?
                                    dr["CurrencyUnit"].ToString() : "";
                                userCurrencyResponse.RSS_Data = dr["RSS_Data"] != DBNull.Value ?
                                    Convert.ToBoolean(dr["RSS_Data"].ToString()) : false;
                                userCurrencyResponse.CurrencyName = dr["CurrencyName"] != DBNull.Value ?
                                    dr["CurrencyName"].ToString() : "";

                                userCurrencyResponseList.Add(userCurrencyResponse);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }

                return userCurrencyResponseList[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Returns list of Ledger Order Items
        /// </summary>

        /// <returns></returns>
        public async static Task<List<LedgerOrderItem>> GetLedgerOrderItems(int compCode, int journalNo)
        {
            try
            {
                LedgerOrderItemsAPIParams param = new LedgerOrderItemsAPIParams
                {
                    compCode = compCode,
                    journalNo = journalNo,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                FinalResponse response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.FETCH_LEDGER_ORDER_ITEMS, HttpMethod.Post, param)
                    as FinalResponse;
                List<LedgerOrderItem> result = null;
                string xml = "<LedgerOrderItems>";
                for (int i = 1; i < ((XmlNode[])response.ResultDoc).Length; i++)
                {
                    xml = xml + ((XmlNode[])response.ResultDoc)[1].OuterXml;
                }

                xml = xml + "</LedgerOrderItems>";

                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "LedgerOrderItems";
                xRoot.IsNullable = true;
                using (StringReader reader = new StringReader(xml))
                {
                    result = (List<LedgerOrderItem>)(new XmlSerializer(typeof(List<LedgerOrderItem>),
                        xRoot)).Deserialize(reader);
                    int numOfPersons = result.Count;
                }


                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Returns list of account orders
        /// </summary>

        /// <returns></returns>
        public async static Task<List<AccountOrdersResponse>> GetRevenueOrders(int compCode)
        {
            try
            {
                AccountOrdersAPIParams param = new AccountOrdersAPIParams
                {
                    accountTypeID = 8,
                    compCode = compCode,
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.GET_ACCOUNT_FOR_ORDERS, HttpMethod.Post, param)
                    as FinalResponse;

                List<AccountOrdersResponse> accountOrderResponseList = new List<AccountOrdersResponse>(); ;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                AccountOrdersResponse accountOrderResponse = new AccountOrdersResponse();
                                accountOrderResponse.CompCode = dr["CompCode"] != DBNull.Value ? Convert.ToInt32(dr["CompCode"].ToString()) : 0;
                                accountOrderResponse.AccountCode = dr["AccountCode"] != DBNull.Value ? dr["AccountCode"].ToString() : "";
                                accountOrderResponse.AccountId = dr["AccountID"] != DBNull.Value ? Convert.ToInt32(dr["AccountID"].ToString()) : 0;
                                accountOrderResponse.AccountName = dr["AccountName"] != DBNull.Value ? dr["AccountName"].ToString() : "";
                                accountOrderResponse.StateID = dr["StateID"] != DBNull.Value ? Convert.ToInt32(dr["StateID"].ToString()) : 0;
                                accountOrderResponse.CountryCode = dr["CountryCode"] != DBNull.Value ? dr["CountryCode"].ToString() : "";
                                accountOrderResponse.City = dr["City"] != DBNull.Value ? dr["City"].ToString() : "";
                                accountOrderResponse.ContactPerson = dr["ContactPerson"] != DBNull.Value ? dr["ContactPerson"].ToString() : "";
                                accountOrderResponse.Telephone = dr["Telephone"] != DBNull.Value ? dr["Telephone"].ToString() : "";

                                accountOrderResponseList.Add(accountOrderResponse);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }

                return accountOrderResponseList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// Returns list of tax rates
        /// </summary>

        /// <returns></returns>
        public async static Task<List<ShowTaxRatesResponse>> ShowTaxRates(string countryCode)
        {
            try
            {
                ShowTaxRatesAPIParams param = new ShowTaxRatesAPIParams
                {
                    countryCode = countryCode,
                    taxTypeID = "2",
                    connectionName = WebserviceConstants.CONNECTION_NAME
                };
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.SHOW_TAX_RATES, HttpMethod.Post, param)
                    as FinalResponse;

                List<ShowTaxRatesResponse> taxRatesResponseList = new List<ShowTaxRatesResponse>(); ;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DataSet resultIds = response.ResultDs;
                    foreach (DataTable dt in resultIds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                ShowTaxRatesResponse taxRateResponse = new ShowTaxRatesResponse();

                                taxRateResponse.StateID = dr["StateID"] != DBNull.Value ?
                                    Convert.ToInt32(dr["StateID"].ToString()) : 0;
                                taxRateResponse.CountryCode = dr["CountryCode"] != DBNull.Value ?
                                    dr["CountryCode"].ToString() : "";
                                taxRateResponse.CityId = dr["CityId"] != DBNull.Value ?
                                    Convert.ToInt32(dr["CityId"].ToString()) : 0;
                                taxRateResponse.TaxTypeID = dr["TaxTypeID"] != DBNull.Value ?
                                    Convert.ToInt32(dr["TaxTypeID"].ToString()) : 0;
                                taxRateResponse.FinancialYear = dr["FinancialYear"] != DBNull.Value ?
                                 dr["FinancialYear"].ToString() : "";
                                taxRateResponse.DateFrom = dr["DateFrom"] != DBNull.Value ?
                                    dr["DateFrom"].ToString() : "";
                                taxRateResponse.TaxCode = dr["TaxCode"] != DBNull.Value ?
                                 dr["TaxCode"].ToString() : "";
                                taxRateResponse.TaxName = dr["TaxName"] != DBNull.Value ?
                                 dr["TaxName"].ToString() : "";
                                taxRateResponse.TaxRatePercent = dr["TaxRatePercent"] != DBNull.Value ?
                                    Convert.ToDecimal(dr["TaxRatePercent"].ToString()) : 0;
                                taxRateResponse.IsDefaultCode = dr["IsDefaultCode"] != DBNull.Value ?
                                     Convert.ToBoolean(dr["IsDefaultCode"].ToString()) : false;
                                taxRateResponse.TaxID = dr["TaxID"] != DBNull.Value ?
                                    Convert.ToInt32(dr["TaxID"].ToString()) : 0;

                                taxRatesResponseList.Add(taxRateResponse);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }

                return taxRatesResponseList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Save ledger orders
        /// </summary>
        /// <returns></returns>
        public async static Task<bool> SaveLedgerOrdersNew(LedgerOrder ledgerOrder)
        {
            try
            {
                var str = Utils.Utilities.ToXML(ledgerOrder);

                XmlDocument doc = new XmlDocument();
                doc.Load(new StringReader(str));
                string innerXml = ((System.Xml.XmlElement)(doc.ChildNodes[1])).InnerXml;
                string requestXml = "<?xml version = \"1.0\" encoding = \"utf-8\" ?><ElucidateAPIParams><LedgerOrder>"
                + innerXml + "</LedgerOrder><connectionName>DEMOConneection</connectionName></ElucidateAPIParams>";
                var response = await WebServiceHandler.GetWebserviceResult(
                    WebserviceConstants.SAVE_LEDGER_ORDER_NEW, HttpMethod.Post, requestXml, true)
                    as FinalResponse;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

    }
}
