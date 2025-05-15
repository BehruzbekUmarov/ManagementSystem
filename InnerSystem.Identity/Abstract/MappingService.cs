using AutoMapper;

namespace InnerSystem.Identity.Abstract;

public class MappingService(IMapper mapper) : IMappingService
{
	public T Map<T, TSourse>(TSourse source)
	{
		return mapper.Map<T>(source);
	}

	public T Map<T, TSource>(TSource source, T destination)
	{
		mapper.Map(source, destination);
		return destination;
	}
}
