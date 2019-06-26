namespace MyWebApi.Infrastructure
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System.Data;
    using System.Threading.Tasks;
    using System.Transactions;

    public class DBTransactionFilter : IAsyncActionFilter
    {
        private readonly IDbConnection _connection;

        private readonly ILogger<DBTransactionFilter> _logger;

        public DBTransactionFilter(IDbConnection connection, ILogger<DBTransactionFilter> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           
            var transactionScopeOptions = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionScopeOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                _connection.Open();
                var executedContext = await next().ConfigureAwait(false);
                if (executedContext.Exception == null)
                {
                    transactionScope.Complete();
                    _logger.Log(LogLevel.Information, "Transaction committed");
                }
                else
                {
                    _logger.Log(LogLevel.Error, $"Transaction aborted due to error: {executedContext.Exception.Message}");
                }
            }
        }
    }
}