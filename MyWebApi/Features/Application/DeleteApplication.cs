using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace MyWebApi.Features.Application
{

    public class DeleteApplication
    {
        public class DeleteApplicationCommand : IRequest<DeleteApplicationResponse>
        {
            public int Id { get; set; }
        }
        public class DeleteApplicationResponse
        {
            public int Deleted { get; set; }
            public string Reason { get; internal set; }
        }
        public class Handler : IRequestHandler<DeleteApplicationCommand, DeleteApplicationResponse>
        {
            private readonly IDbConnection _dbConnection;
            public Handler(IDbConnection dbConnection)
            {
                _dbConnection = dbConnection;
            }
            public async Task<DeleteApplicationResponse> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
            {

                string query = @$"SELECT 
                                   [{nameof(Models.Application.ApplicationId)}]
                                  ,[{nameof(Models.Application.ApplicationType)}]
                                  ,[{nameof(Models.Application.ApplicantName)}]
                                  ,[{nameof(Models.Application.ApplicantDateOfBirth)}]
                                  ,[{nameof(Models.Application.ApplicationAddress)}]
                                  ,[{nameof(Models.Application.ApplicationCity)}]
                                  ,[{nameof(Models.Application.ApplicationState)}]
                                  ,[{nameof(Models.Application.ApplicationZip)}]
                                  ,[{nameof(Models.Application.ApplicantEmailId)}]
                                  ,[{nameof(Models.Application.ApplicationCreationDate)}]
                                  ,[{nameof(Models.Application.ApplicationLastModifiedDate)}]
                              FROM[dbo].[Application]
                              WHERE ApplicationId = @ApplicationId";
                var resultapplication = await _dbConnection.QueryFirstAsync<Models.Application>(query, new { ApplicationId= request.Id });

                if (resultapplication == null)
                {
                    return new DeleteApplicationResponse()
                    {
                        Deleted = 0,
                        Reason=$"Application with id {request.Id} Not Found"
                    };
                }
                string sql = @"DELETE FROM [dbo].[Application]
                                   WHERE ApplicationId=@ApplicationId";
                var result = await _dbConnection.ExecuteAsync(sql, new { ApplicationId= request.Id });
                
                return new DeleteApplicationResponse()
                {
                    Deleted = result,
                    Reason=$"Successfully deleted Application {request.Id}"
                };
            }
        }
    }
}
