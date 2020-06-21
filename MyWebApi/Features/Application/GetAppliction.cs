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

    public class GetAppliction
    {
        public class GetApplictionQuery : IRequest<GetApplictionResponse>
        {
            public int Id { get; set; }
        }
        public class GetApplictionResponse: Models.Application
        {

        }
        public class Handler : IRequestHandler<GetApplictionQuery, GetApplictionResponse>
        {
            private readonly IDbConnection _dbConnection;
            public Handler(IDbConnection dbConnection)
            {
                _dbConnection = dbConnection;
            }
            public async Task<GetApplictionResponse> Handle(GetApplictionQuery request, CancellationToken cancellationToken)
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
                var result= await _dbConnection.QueryFirstAsync<Models.Application>(query, new { ApplicationId= request.Id});
                
                return result as GetApplictionResponse;
            }
        }
    }

}
