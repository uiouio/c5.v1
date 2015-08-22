using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Models;
using CTPPV5.Models.Api;
using CTPPV5.Client.Winform.Api;

namespace CTPPV5.Client.Winform
{
    public class LocalApiMockModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new RemoteApiMock()).As<IRemoteApi>();
        }
    }

    public class RemoteApiMock : CTPPV5.Client.Winform.Api.IRemoteApi
    {

        public Models.Api.LoginRS Login(LoginRQ loginRQ)
        {
            return new LoginRS
            {
                Ok = true,
                Message = string.Empty,
                User = new Models.User
                {
                    AuthorizedModules = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 100 },
                    Group = UserGroup.Administrator,
                    LastLoginTime = DateTime.Now,
                    Name = "田志良",
                    OrganizationID = 1,
                    OrganizationName = "创智",
                    Token = "aaaa"
                }
            };
        }

        public Models.Api.SchoolListClassifiedRS GetSchoolListClassified(ApiRequest apiRQ)
        {
            var schoolCollection1 = new SchoolCollection();
            schoolCollection1.Add(new School { ID = 1, Name = "上海海富幼儿园龙阳校区" });
            schoolCollection1.Add(new School { ID = 2, Name = "上海私立蒙特梭利幼儿园" });
            schoolCollection1.Add(new School { ID = 3, Name = "上海市建青实验学校幼儿部" });
            schoolCollection1.Add(new School { ID = 4, Name = "上海中国福利会幼儿园" });
            schoolCollection1.Add(new School { ID = 5, Name = "上海市浦东新区冰厂田幼儿园（云山路寄宿部）" });
            var area1 = new Area { ID = 1, Name = "黄浦区", Schools = schoolCollection1 };
            var schoolCollection2 = new SchoolCollection();
            schoolCollection2.Add(new School { ID = 6, Name = "上海市实验幼儿园（杏山园）" });
            schoolCollection2.Add(new School { ID = 7, Name = "孔祥东音乐幼儿园（巨野路园区）" });
            schoolCollection2.Add(new School { ID = 8, Name = "上海市徐汇区科技幼儿园" });
            var area2 = new Area { ID = 2, Name = "徐汇区", Schools = schoolCollection2 };
            var schoolCollection3 = new SchoolCollection();
            schoolCollection3.Add(new School { ID = 9, Name = "上海乌南幼儿园" });
            schoolCollection3.Add(new School { ID = 10, Name = "上海市本溪路幼儿园" });
            schoolCollection3.Add(new School { ID = 11, Name = "上海市宝山区行知实验幼儿园" });
            var area3 = new Area { ID = 3, Name = "长宁区", Schools = schoolCollection3 };
            var areaCollection1 = new AreaCollection();
            areaCollection1.Add(area1);
            areaCollection1.Add(area2);
            areaCollection1.Add(area3);
            var schoolCollection4 = new SchoolCollection();
            schoolCollection4.Add(new School { ID = 12, Name = "宁波市第一幼儿园" });
            schoolCollection4.Add(new School { ID = 13, Name = "宁波市宝韵音乐幼儿园" });
            schoolCollection4.Add(new School { ID = 14, Name = "宁波市华光幼儿园" });
            var schoolCollection5 = new SchoolCollection();
            schoolCollection5.Add(new School { ID = 15, Name = "杭州市行知幼儿园" });
            schoolCollection5.Add(new School { ID = 16, Name = "杭州市清波幼儿园" });
            schoolCollection5.Add(new School { ID = 17, Name = "杭州市娃哈哈幼儿园" });
            var area4 = new Area { ID = 4, Name = "杭州市", Schools = schoolCollection5 };
            var area5 = new Area { ID = 5, Name = "宁波市", Schools = schoolCollection4 };
            var areaCollection2 = new AreaCollection();
            areaCollection2.Add(area4);
            areaCollection2.Add(area5);
            var sh = new Province { ID = 1, Areas = areaCollection1, Name = "上海市" };
            var zj = new Province { ID = 2, Areas = areaCollection2, Name = "浙江省" };

            return new SchoolListClassifiedRS { Ok = true, Provinces = new List<Province> { sh, zj } };
        }
    }
}
