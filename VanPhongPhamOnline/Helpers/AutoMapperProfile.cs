using AutoMapper;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Helpers
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, KhachHang>();
            //.ForMember(kh => kh.HoTen, option => option
            //.MapFrom(RegisterVM => RegisterVM.HoTen))
            //.ReverseMap();
        }
    }
}
