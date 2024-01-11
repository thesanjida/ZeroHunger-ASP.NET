using AutoMapper;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.Mapping
{
    public class AutoMapperConfig
    {
        public AutoMapperConfig() { }

        public List<TDestination> MakeList<TSource, TDestination>(List<TSource> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = new Mapper(config);
            return mapper.Map<List<TDestination>>(source);
        }

        public TDestination MakeSingleInstance<TSource, TDestination>(TSource source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = new Mapper(config);
            return mapper.Map<TDestination>(source);
        }


    }
}