using AutoMapper;

namespace MovieAPI.Helper
{
    public class PagenatedMapper
    {
        private readonly IMapper _mapper;

        public PagenatedMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public PagenatedResponse<T2> Map<T1,T2>(PagenatedResponse<T1>r)
        {
            var re = new PagenatedResponse<T2>
            {
                Data = _mapper.Map<IEnumerable<T1>, IEnumerable<T2>>(r.Data),
                PageIndex = r.PageIndex,
                PageSize = r.PageSize,
                TotalPages = r.TotalPages,
            };
            return re;
        }
    }
}
