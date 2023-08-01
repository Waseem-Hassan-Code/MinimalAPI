using AutoMapper;
using MagicVilla_CouponAPI.Model;
using MagicVilla_CouponAPI.Model.DTO;

namespace MagicVilla_CouponAPI
{
    public class MappingConfiguration:Profile
    {
        public MappingConfiguration() {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();
        }
    }
}
