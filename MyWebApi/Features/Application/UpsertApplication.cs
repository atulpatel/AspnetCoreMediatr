using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MyWebApi.Features.Application
{
    public class UpsertApplication
    {
        public class InsertApplicationCommand : Models.Application, IRequest<InsertApplicationResponse> 
        {
            //Any extra fiels can go hear.
        }
        public class InsertApplicationResponse
        {
            public int Saved { get; set; }
        }
        public class Handler : IRequestHandler<InsertApplicationCommand, InsertApplicationResponse>
        {
            private readonly IDbConnection _dbConnection;

            public Handler(IDbConnection dbConnection)
            {
                _dbConnection = dbConnection;
            }
            public async Task<InsertApplicationResponse> Handle(InsertApplicationCommand request, CancellationToken cancellationToken)
            {
                var sql = @"SET NOCOUNT ON

                    SET IDENTITY_INSERT [dbo].[Application] ON

                    MERGE INTO [dbo].[Application] AS [Target]
                    USING (SELECT [ApplicationId],[ApplicationType],[ApplicantName],[ApplicantDateOfBirth],[ApplicationAddress],[ApplicationCity],[ApplicationState],[ApplicationZip],[ApplicantEmailId],[ApplicationCreationDate],[ApplicationLastModifiedDate] FROM [dbo].[Application] WHERE 1 = 0 -- Empty dataset (source table contained no rows at time of MERGE generation) 
                    ) AS [Source] ([ApplicationId],[ApplicationType],[ApplicantName],[ApplicantDateOfBirth],[ApplicationAddress],[ApplicationCity],[ApplicationState],[ApplicationZip],[ApplicantEmailId],[ApplicationCreationDate],[ApplicationLastModifiedDate])
                    ON ([Target].[ApplicationId] = [Source].[ApplicationId])
                    WHEN MATCHED AND (
	                    NULLIF([Source].[ApplicationType], [Target].[ApplicationType]) IS NOT NULL OR NULLIF([Target].[ApplicationType], [Source].[ApplicationType]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicantName], [Target].[ApplicantName]) IS NOT NULL OR NULLIF([Target].[ApplicantName], [Source].[ApplicantName]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicantDateOfBirth], [Target].[ApplicantDateOfBirth]) IS NOT NULL OR NULLIF([Target].[ApplicantDateOfBirth], [Source].[ApplicantDateOfBirth]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicationAddress], [Target].[ApplicationAddress]) IS NOT NULL OR NULLIF([Target].[ApplicationAddress], [Source].[ApplicationAddress]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicationCity], [Target].[ApplicationCity]) IS NOT NULL OR NULLIF([Target].[ApplicationCity], [Source].[ApplicationCity]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicationState], [Target].[ApplicationState]) IS NOT NULL OR NULLIF([Target].[ApplicationState], [Source].[ApplicationState]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicationZip], [Target].[ApplicationZip]) IS NOT NULL OR NULLIF([Target].[ApplicationZip], [Source].[ApplicationZip]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicantEmailId], [Target].[ApplicantEmailId]) IS NOT NULL OR NULLIF([Target].[ApplicantEmailId], [Source].[ApplicantEmailId]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicationCreationDate], [Target].[ApplicationCreationDate]) IS NOT NULL OR NULLIF([Target].[ApplicationCreationDate], [Source].[ApplicationCreationDate]) IS NOT NULL OR 
	                    NULLIF([Source].[ApplicationLastModifiedDate], [Target].[ApplicationLastModifiedDate]) IS NOT NULL OR NULLIF([Target].[ApplicationLastModifiedDate], [Source].[ApplicationLastModifiedDate]) IS NOT NULL) THEN
                     UPDATE SET
                      [Target].[ApplicationType] = [Source].[ApplicationType], 
                      [Target].[ApplicantName] = [Source].[ApplicantName], 
                      [Target].[ApplicantDateOfBirth] = [Source].[ApplicantDateOfBirth], 
                      [Target].[ApplicationAddress] = [Source].[ApplicationAddress], 
                      [Target].[ApplicationCity] = [Source].[ApplicationCity], 
                      [Target].[ApplicationState] = [Source].[ApplicationState], 
                      [Target].[ApplicationZip] = [Source].[ApplicationZip], 
                      [Target].[ApplicantEmailId] = [Source].[ApplicantEmailId], 
                      [Target].[ApplicationCreationDate] = [Source].[ApplicationCreationDate], 
                      [Target].[ApplicationLastModifiedDate] = [Source].[ApplicationLastModifiedDate]
                    WHEN NOT MATCHED BY TARGET THEN
                     INSERT([ApplicationId],[ApplicationType],[ApplicantName],[ApplicantDateOfBirth],[ApplicationAddress],[ApplicationCity],[ApplicationState],[ApplicationZip],[ApplicantEmailId],[ApplicationCreationDate],[ApplicationLastModifiedDate])
                     VALUES([Source].[ApplicationId],[Source].[ApplicationType],[Source].[ApplicantName],[Source].[ApplicantDateOfBirth],[Source].[ApplicationAddress],[Source].[ApplicationCity],[Source].[ApplicationState],[Source].[ApplicationZip],[Source].[ApplicantEmailId],[Source].[ApplicationCreationDate],[Source].[ApplicationLastModifiedDate])
                    WHEN NOT MATCHED BY SOURCE THEN 
                     DELETE;

                    DECLARE @mergeError int
                     , @mergeCount int
                    SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
                    IF @mergeError != 0
                     BEGIN
                     PRINT 'ERROR OCCURRED IN MERGE FOR [dbo].[Application]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
                     END
                    ELSE
                     BEGIN
                     PRINT '[dbo].[Application] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
                     END
                    GO";
                var result = await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, request);
                return new InsertApplicationResponse()
                {
                    Saved = result
                };
            }
        }
    }
}
