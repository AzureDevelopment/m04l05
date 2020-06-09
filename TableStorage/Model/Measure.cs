using System;
using Microsoft.Azure.Cosmos.Table;

namespace m04l05
{
    internal class Measure : TableEntity
    {

        public Measure()
        {
        }

        public Measure(Guid DeviceId, Guid Id)
        {
            this.PartitionKey = DeviceId.ToString(); this.RowKey = Id.ToString();
        }

        public int ValueAt { get; set; }
        public DateTime MeasuredAt { get; internal set; }
    }
}