using AutoMapper;
using HtmlToCsv2.Dtos;

namespace HtmlToCsv2
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile(){
            CreateMap<Candidate,CandidateDto>();
            CreateMap<Area,AreaDto>();
                    }
    }
}