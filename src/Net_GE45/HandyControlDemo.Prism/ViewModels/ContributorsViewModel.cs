using HandyControlDemo.Data;
using HandyControlDemo.Properties.Langs;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ContributorsViewModel : BindableBase
    {
        private ObservableCollection<AvatarModel> _DataList;
        public ObservableCollection<AvatarModel> DataList
        {
            get { return _DataList; }
            set { SetProperty(ref _DataList, value); }
        }

        private bool _DataGot;
        public bool DataGot
        {
            get { return _DataGot; }
            set { SetProperty(ref _DataGot, value); }
        }

        public ContributorsViewModel()
        {
            Task.Run(async () =>
            {
                DataList = new ObservableCollection<AvatarModel>(await GetContributorDataList());
            }).ContinueWith(x => { DataGot = true; });
        }

        internal async Task<List<AvatarModel>> GetContributorDataList()
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "request");
            var list = new List<AvatarModel>();
            try
            {
                var json = await client.DownloadStringTaskAsync(new Uri("https://api.github.com/repos/nabian/handycontrol/contributors"));
                var objList = JsonConvert.DeserializeObject<List<dynamic>>(json);

                list.AddRange(objList.Select(item => new AvatarModel
                {
                    DisplayName = item.login,
                    AvatarUri = item.avatar_url,
                    Link = item.html_url
                }));
            }
            catch (Exception e)
            {
                HandyControl.Controls.MessageBox.Error(e.Message, Lang.Error);
            }

            return list;
        }
    }
}
