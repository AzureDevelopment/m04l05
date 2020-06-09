using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using StackExchange.Redis;
namespace Redis.Demo
{
    public class OrdersRepository
    {
        public Order getOrderById(int orderId, bool withCache)
        {
            var redisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString");
            var redisConnect = ConnectionMultiplexer.Connect(redisConnectionString);

            try
            {
                var Rediscache = redisConnect.GetDatabase();
                var redisValue = withCache ? Rediscache.StringGet("orderDetail-" + orderId) : RedisValue.Null;
                var connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                if (!redisValue.HasValue)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        IEnumerable<Order> queryResult = connection.Query<Order>($"SELECT * FROM SalesLT.SalesOrderDetail s INNER JOIN SalesLT.Product p ON s.ProductID = p.ProductID WHERE s.SalesOrderID = ${orderId}; WAITFOR DELAY '00:00:03'");
                        var result = queryResult.ToList().FirstOrDefault();
                        Rediscache.StringSetAsync("orderDetail-" + orderId, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(1));
                        return result;
                    }
                }
                else
                {
                    return JsonConvert.DeserializeObject<Order>(redisValue.ToString());
                }
            }
            catch (Exception e)
            {
                return new Order();
            }
        }
    }



}











