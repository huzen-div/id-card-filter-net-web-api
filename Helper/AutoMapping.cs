using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BroadcastAPI.Entities;
using BroadcastAPI.Models;

namespace BroadcastAPI.Helper
{
   
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //CreateMap<M_Address, AddressModel>();
        }
    }
}
