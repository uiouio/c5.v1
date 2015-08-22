using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Models
{
    public class ModelModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Mapper.CreateMap<IEnumerable<School>, SchoolCollection>()
                .AfterMap((src, dest) => src.ForEach(school => dest.Add(school)));
        }
    }
}
