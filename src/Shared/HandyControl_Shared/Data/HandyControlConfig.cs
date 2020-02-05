namespace HandyControl.Data
{
    public class HandyControlConfig
    {
        public SystemVersionInfo SystemVersionInfo { get; set; }

        public string Lang { get; set; }

        public int TimelineFrameRate { get; set; } = 60;
    }
}