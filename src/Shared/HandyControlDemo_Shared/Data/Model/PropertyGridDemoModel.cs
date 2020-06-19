using System.ComponentModel;

namespace HandyControlDemo.Data
{
    public class PropertyGridDemoModel
    {
        [Category("Info")]
        public string Name { get; set; }

        [Category("Achievement")]
        public int Score { get; set; }

        [Category("Achievement")]
        public bool IsPassed { get; set; }

        [Category("Info")]
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}