using System;

namespace HandyControlDemo.Data.Model
{
    public class CellGateRecord
    {
        public string CellGateId { get; set; }

        public string PersonId { get; set; }

        public DateTime CreateTime { get; set; }

        public string Direction { get; set; }

        public string DeviceId { get; set; }
    }
}