using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL
{
    public class EnrollmentNotificationDAL : IEnrollmentNotificationDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<EnrollmentNotification, EnrollmentNotificationModel> _enrollmentNotificationMapper;

        public EnrollmentNotificationDAL(DataAccess dataAccess, EnrollmentNotificationMapper enrollmentNotificationMapper)
        {
            _dataAccess = dataAccess;
            _enrollmentNotificationMapper = enrollmentNotificationMapper;
        }

        public int Add(EnrollmentNotificationModel notification)
        {
            string insertQuery = @"INSERT INTO EnrollmentNotification (NotificationMessage, RecipientId, SentAt)
                                   VALUES (@NotificationMessage, @RecipientId, GETDATE());";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NotificationMessage", notification.NotificationMessage),
                new SqlParameter("@RecipientId", notification.RecipientId)
            };
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows added");
            }
            return rowsAffected;
        }

        public int AddBatch(IEnumerable<EnrollmentNotificationModel> notifications)
        {
            StringBuilder insertQuery = new StringBuilder(@"INSERT INTO EnrollmentNotification (NotificationMessage, RecipientId, SentAt) VALUES");
            List<SqlParameter> parameters = new List<SqlParameter>();

            int parameterIndex = 0;
            foreach (EnrollmentNotificationModel notification in notifications)
            {
                string notificationValues = $"(@NotificationMessage{parameterIndex}, @RecipientId{parameterIndex}, GETDATE()), ";
                insertQuery.Append(notificationValues);

                parameters.Add(new SqlParameter($"@NotificationMessage{parameterIndex}", notification.NotificationMessage));
                parameters.Add(new SqlParameter($"@RecipientId{parameterIndex}", notification.RecipientId));

                parameterIndex++;
            }
            insertQuery.Length -= 2;
            insertQuery.Append(";");
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(insertQuery.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows added");
            }

            return rowsAffected;
        }

        public IEnumerable<EnrollmentNotificationModel> GetAllByRecipientIdAndSeenStatus(short recipientId, bool hasSeen)
        {
            string selectQuery = @"SELECT EnrollmentNotificationId, NotificationMessage, SentAt
                                   FROM EnrollmentNotification
                                   WHERE RecipientId = @RecipientId AND HasSeen = @HasSeen";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@RecipientId", recipientId),
                new SqlParameter("@HasSeen", hasSeen)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            return _enrollmentNotificationMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public int Update(EnrollmentNotificationModel model)
        {
            string updateQuery = @"UPDATE EnrollmentNotificationId SET 
                                   HasSeen =  @HasSeen, 
                                   SeenAt =  GETDATE()
                                   WHERE EnrollmentNotificationId = @EnrollmentNotificationId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EnrollmentNotificationId", model.EnrollmentNotificationId),
                new SqlParameter("@HasSeen", model.HasSeen)
            };
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(updateQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows updated");
            }
            return rowsAffected;
        }
    }
}
