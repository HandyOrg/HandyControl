
namespace HandyControlDemo.Data
{
    public class ContributorModel
    {
        public string UserName { get; set; }

        public string Link { get; set; }

        public string AvatarUri { get; set; }
    }

    public class ContributorWebModel
    {
        public string login { get; set; }

        public string html_url { get; set; }

        public string avatar_url { get; set; }
    }
}