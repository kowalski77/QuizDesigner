using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;
using Dapper;
using Microsoft.Data.SqlClient;
using QuizDesigner.Outbox;

namespace QuizDesigner.OutboxSender
{
    public class OutboxData
    {
        private const string PublishFailedOutboxMessagesSql = "Select * from OutboxMessages where EventState = 2";
        private const string UpdateOutboxMessageEventStateSql = "Update OutboxMessages set EventState = 2 where Id = {0}";

        private readonly string connectionString;

        public OutboxData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Maybe<IReadOnlyList<OutboxMessage>>> GetNotPublishedAsync()
        {
            await using var db = new SqlConnection(this.connectionString);

            var outboxMessageCollection = (await db.QueryAsync<OutboxMessage>(PublishFailedOutboxMessagesSql)
                    .ConfigureAwait(false))
                .ToList();

            return outboxMessageCollection;
        }

        public async Task SetMessageAsPublishedAsync(Guid id)
        {
            await using var db = new SqlConnection(this.connectionString);

            await db.ExecuteAsync(string.Format(CultureInfo.InvariantCulture, UpdateOutboxMessageEventStateSql, id))
                .ConfigureAwait(false);
        }
    }
}