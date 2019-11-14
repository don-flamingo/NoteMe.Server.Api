using AutoMapper;

namespace NoteMe.Server.Infrastructure.Framework.Mappers
{
    public interface INoteMeMapper
    {
        TDesc Map<TSource, TDesc>(TSource source);
        TDesc Map<TDesc>(object source);
    }
    
    public class NoteMeMapper : INoteMeMapper
    {
        private readonly IMapper _mapper;

        public NoteMeMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDesc Map<TSource, TDesc>(TSource source)
            => _mapper.Map<TSource, TDesc>(source);

        public TDesc Map<TDesc>(object source)
            => _mapper.Map<TDesc>(source);
    }
}