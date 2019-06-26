using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using System;
using FluentValidation;

namespace MyWebApi.Features.Customers
{
    public class CustomerUpsert
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator() {
                RuleFor(x => x.Name).Null().Empty().WithMessage("Customer Name should not be empty.");
                
            }
        }

        public class Command : IRequest<Response>
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Address1 { get; set; }

            public string Address2 { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string Zip { get; set; }

            public string ContactNo { get; set; }

            public string ContactEmailId { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            public Handler(IDbConnection dbConnection)
            {
                _dbConnection = dbConnection;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, new { request.Id,
                    request.Name,
                    request.Address1,
                    request.Address2,
                    request.City,
                    request.ContactNo,
                    request.ContactEmailId,
                    request.State,
                    request.Zip,
                    Active=1,
                    CreatedOn= DateTime.Now,
                    ModifiedOn = DateTime.Now
                });
                return new Response {  Id = result};
            }

            private string sql = @"
                                MERGE Customer AS MyTarget
                                USING ( SELECT @Id
                                          ,@Name
                                          ,@Address1
                                          ,@Address2
                                          ,@City
                                          ,@State
                                          ,@Zip
                                          ,@ContactNo
                                          ,@ContactEmailId
                                          ,@Active
                                          ,@CreatedOn
                                          ,@ModifiedOn) as MySource
                                        (   Id,
                                            Name
                                            ,Address1
                                            ,Address2
                                            ,City
                                            ,State
                                            ,Zip
                                            ,ContactNo
                                            ,ContactEmailId
                                            ,Active
                                            ,CreatedOn
                                            ,ModifiedOn
                                        )
                                     ON MySource.Id = MyTarget.Id
                                WHEN MATCHED THEN UPDATE SET 
                                           [Name] = MySource.[Name]
                                          ,[Address1] = MySource.[Address1]
                                          ,[Address2] = MySource.[Address2]
                                          ,[City] = MySource.[City]
                                          ,[State] = MySource.[State]
                                          ,[Zip] = MySource.[Zip]
                                          ,[ContactNo] = MySource.[ContactNo]
                                          ,[ContactEmailId] = MySource.[ContactEmailId]
                                          ,[Active] = MySource.[Active]
                                          ,[CreatedOn] = MySource.[CreatedOn]
                                          ,[ModifiedOn] = MySource.[ModifiedOn]
                                WHEN NOT MATCHED THEN INSERT
                                    (
                                        [Name] 
                                        ,[Address1] 
                                        ,[Address2] 
                                        ,[City] 
                                        ,[State] 
                                        ,[Zip] 
                                        ,[ContactNo] 
                                        ,[ContactEmailId] 
                                        ,[Active] 
                                        ,[CreatedOn] 
                                        ,[ModifiedOn]
                                    )
                                    VALUES (
                                        MySource.[Name], 
                                        MySource.[Address1], 
                                        MySource.[Address2], 
                                        MySource.[City], 
                                        MySource.[State], 
                                        MySource.[Zip], 
                                        MySource.[ContactNo], 
                                        MySource.[ContactEmailId], 
                                        MySource.[Active], 
                                        MySource.[CreatedOn], 
                                        MySource.[ModifiedOn]
                                    )
                                    OUTPUT
                                    inserted.Id;";
            private readonly IDbConnection _dbConnection;
        }
    }
}
