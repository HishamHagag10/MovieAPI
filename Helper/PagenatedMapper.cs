
namespace MovieAPI.Helper
{
    public class PagenatedMapper
    {
        private readonly IMapper _mapper;

        public PagenatedMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public PagenatedResponse<T2> Map<T1,T2>(ICollection<T1> data,int pageIndex,int pageSize,int totalCount)
        {
            var pr = new PagenatedResponse<T2>
            {
                Data = _mapper.Map<IEnumerable<T1>, IEnumerable<T2>>(data),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount=totalCount,
                TotalPages = (totalCount+pageSize-1)/pageSize,
            };
            return pr;
        }
    }
}
