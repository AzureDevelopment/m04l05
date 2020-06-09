using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Redis.Demo
{
    public static class RedisDemo
    {
        [FunctionName("RedisDemo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            if (Int32.TryParse(req.Query["orderId"], out int orderId))
            {
                bool withCache = req.Query["withCache"] == "true";
                var requestedOrder = new OrdersRepository().getOrderById(orderId, withCache);
                var response = JsonConvert.SerializeObject(requestedOrder);
                return new OkObjectResult(response);
            }
            else
            {
                return new BadRequestObjectResult("Provide order Id");
            }
        }
    }
}
