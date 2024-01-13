using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
    public class EnrollmentNotificationDAL : IUserNotificationDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<UserNotification, UserNotificationModel> _userNotificationMapper;

        public EnrollmentNotificationDAL(DataAccess dataAccess, UserNotificationMapper userNotificationMapper)
        {
            _dataAccess = dataAccess;
            _userNotificationMapper = userNotificationMapper;
        }

        public async Task<int> AddAsync(UserNotificationModel notification)
        {
            string insertQuery = @"INSERT INTO UserNotification (NotificationMessage, RecipientId, SentAt, Title)
                                   VALUES (@NotificationMessage, @RecipientId, GETDATE(), @Title);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NotificationMessage", notification.NotificationMessage),
                new SqlParameter("@RecipientId", notification.RecipientId),
                new SqlParameter("@Title", notification.Title),
            };
            int rowsAffected;

            try
            {
                rowsAffected = await _dataAccess.ExecuteNonQuery(insertQuery, parameters);
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

        public async Task<int> AddBatchAsync(IEnumerable<UserNotificationModel> notifications)
        {
            StringBuilder insertQuery = new StringBuilder(@"INSERT INTO UserNotification (NotificationMessage, RecipientId, SentAt, Title) VALUES");
            List<SqlParameter> parameters = new List<SqlParameter>();

            int parameterIndex = 0;
            foreach (UserNotificationModel notification in notifications)
            {
                string notificationValues = $"(@NotificationMessage{parameterIndex}, @RecipientId{parameterIndex}, GETDATE(), @Title{parameterIndex}), ";
                insertQuery.Append(notificationValues);

                parameters.Add(new SqlParameter($"@NotificationMessage{parameterIndex}", notification.NotificationMessage));
                parameters.Add(new SqlParameter($"@RecipientId{parameterIndex}", notification.RecipientId));
                parameters.Add(new SqlParameter($"@@Title{parameterIndex}", notification.Title));

                parameterIndex++;
            }
            insertQuery.Length -= 2;
            insertQuery.Append(";");
            int rowsAffected;

            try
            {
                rowsAffected = await _dataAccess.ExecuteNonQuery(insertQuery.ToString(), parameters);
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

        public async Task<IEnumerable<UserNotificationModel>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen)
        {
            string selectQuery = @"SELECT UserNotificationId, NotificationMessage, SentAt, TItle
                                   FROM UserNotification
                                   WHERE RecipientId = @RecipientId AND HasSeen = @HasSeen";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@RecipientId", recipientId),
                new SqlParameter("@HasSeen", hasSeen)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _userNotificationMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<int> UpdateAsync(UserNotificationModel model)
        {
            string updateQuery = @"UPDATE UserNotificationId SET 
                                   HasSeen =  @HasSeen, 
                                   SeenAt =  GETDATE()
                                   WHERE UserNotificationId = @UserNotificationId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserNotificationId", model.UserNotificationId),
                new SqlParameter("@HasSeen", model.HasSeen)
            };
            int rowsAffected;

            try
            {
                rowsAffected = await _dataAccess.ExecuteNonQuery(updateQuery, parameters);
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
