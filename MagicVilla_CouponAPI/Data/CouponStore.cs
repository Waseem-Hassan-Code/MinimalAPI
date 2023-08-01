using MagicVilla_CouponAPI.Model;

namespace MagicVilla_CouponAPI.Data
{
    public static class CouponStore
    {
        public static List<Coupon> CouponList = new List<Coupon> {
        new Coupon{Id=1,Name="10off",Percent=10,IsActive=true},
        new Coupon{Id=2,Name="20off",Percent=20,IsActive=false}
        };
    }
}
