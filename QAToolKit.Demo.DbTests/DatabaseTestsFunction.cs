using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using QAToolKit.Engine.Database.Models;
using QAToolKit.Engine.Database.Generators;
using QAToolKit.Engine.Database.Runners;
using System.Linq;
using System;

namespace QAToolKit.Demo.DbTests
{
    public static class DatabaseTestsFunction
    {
        [FunctionName("DatabaseTestsFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Running Database tests.");

            var generator = new SqlServerTestGenerator(options =>
            {
                options.AddDatabaseObjectExitsRule(new string[]
                {
                    "Customer",
                    "ProductModel",
                    "ProductDescription",
                    "Product",
                    "ProductModelProductDescription",
                    "ProductCategory",
                    "BuildVersion",
                    "ErrorLog",
                    "Address",
                    "CustomerAddress",
                    "SalesOrderDetail",
                    "SalesOrderHeader"
                }, DatabaseObjectType.Table);

                options.AddDatabaseObjectExitsRule(new string[]
                {
                    "vProductModelCatalogDescription",
                    "vProductAndDescription",
                    "vGetAllCategories"
                }, DatabaseObjectType.View);

                options.AddDatabaseObjectExitsRule(new string[]
                {
                    "uspLogError",
                    "uspPrintError"
                }, DatabaseObjectType.StoredProcedure);

                options.AddDatabaseRecordsCountRule(new List<DatabaseRecordCountRule>() {
                    new DatabaseRecordCountRule()
                    {
                        TableName = "SalesLT.ProductCategory",
                        Count = 41,
                        Operator = "="
                    }
                });

                options.AddDatabaseRecordExitsRule(new List<DatabaseRecordExistRule>() { 
                    new DatabaseRecordExistRule()
                    {
                        ColumnName = "EmailAddress",
                        Operator = "=",
                        TableName = "SalesLT.Customer",
                        Value = "greg1@adventure-works.com"
                    }
                });
            });

            IEnumerable<DatabaseTest> scripts = await generator.Generate();

            var runner = new SqlServerTestRunner(scripts, options =>
            {
                options.AddSQLServerConnection(Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process));
            });

            IEnumerable<DatabaseTestResult> results = await runner.Run();

            if (results.ToList().Any(x => x.DatabaseResult == false))
            {
                return new BadRequestObjectResult(results.Where(x => x.DatabaseResult == false));
            }

            return new OkObjectResult(results);
        }
    }
}
