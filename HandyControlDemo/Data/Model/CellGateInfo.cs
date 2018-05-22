using System.Collections.Generic;

namespace HandyControlDemo.Data.Model
{
    public class CellGateInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Parent { get; set; }

        public List<CellGateInfo> InfoList { get; set; }
    }
}