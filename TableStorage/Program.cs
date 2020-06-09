using System;
using Microsoft.Azure.Cosmos.Table;

namespace m04l05
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageConnectionString = "";
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference("Measures");
            table.CreateIfNotExists();
            var deviceId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var measure = new Measure(deviceId, id)
            {
                MeasuredAt = DateTime.Now,
                ValueAt = 6,
            };

            TableOperation insertOperation = TableOperation.InsertOrMerge(measure);
            table.Execute(insertOperation);

            TableOperation queryOperation = TableOperation.Retrieve<Measure>(deviceId.ToString(), id.ToString());
            Measure result = table.Execute(queryOperation).Result as Measure;
            if (result != null)
            {
                Console.WriteLine("Result value: " + result.ValueAt);
            }
        }
    }
}
