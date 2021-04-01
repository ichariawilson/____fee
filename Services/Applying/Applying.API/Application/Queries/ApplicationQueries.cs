namespace Microsoft.Fee.Services.Applying.API.Application.Queries
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class ApplicationQueries : IApplicationQueries
    {
        private string _connectionString = string.Empty;

        public ApplicationQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }


        public async Task<Application> GetApplicationAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                   @"select a.[Id] as applicationnumber,a.ApplicationDate as date, a.Description as description,
                        a.Profile_IDNumber as idnumber, a.Profile_Request as request,
                        s.Name as status, 
                        ai.ScholarshipItemName as scholarshipitemname, ai.Slots as slots, ai.SlotAmount as slotamount, ai.PictureUrl as pictureurl
                        FROM applying.Applications a
                        LEFT JOIN applying.Applicationitems ai ON a.Id = ai.applicationid 
                        LEFT JOIN applying.applicationstatus s on a.ApplicationStatusId = s.Id
                        WHERE a.Id=@id"
                        , new { id }
                    );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapApplicationItems(result);
            }
        }

        public async Task<IEnumerable<ApplicationSummary>> GetApplicationsFromUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryAsync<ApplicationSummary>(@"SELECT a.[Id] as applicationnumber,a.[ApplicationDate] as [date],s.[Name] as [status], SUM(ai.slots*ai.slotamount) as total
                     FROM [applying].[Applications] a
                     LEFT JOIN[applying].[applicationitems] ai ON  a.Id = ai.applicationid 
                     LEFT JOIN[applying].[applicationstatus] s on a.ApplicationStatusId = s.Id                     
                     LEFT JOIN[applying].[students] sa on a.StudentId = sa.Id
                     WHERE sa.IdentityGuid = @userId
                     GROUP BY a.[Id], a.[ApplicationDate], s.[Name] 
                     ORDER BY a.[Id]", new { userId });
            }
        }

        public async Task<IEnumerable<PaymentType>> GetPaymentTypesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryAsync<PaymentType>("SELECT * FROM applying.paymenttypes");
            }
        }

        private Application MapApplicationItems(dynamic result)
        {
            var application = new Application
            {
                applicationnumber = result[0].applicationnumber,
                date = result[0].date,
                status = result[0].status,
                description = result[0].description,
                applicationitems = new List<Applicationitem>(),
                total = 0
            };

            foreach (dynamic item in result)
            {
                var applicationitem = new Applicationitem
                {
                    scholarshipitemname = item.scholarshipitemname,
                    slots = item.slots,
                    slotamount = (double)item.slotamount,
                    pictureurl = item.pictureurl
                };

                application.total += item.slots * item.slotamount;
                application.applicationitems.Add(applicationitem);
            }

            return application;
        }
    }
}
