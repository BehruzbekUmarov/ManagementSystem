namespace InnerSystem.Identity.Abstract;

public interface IMappingService
{
	T Map<T, TSource>(TSource source);
	T Map<T, TSource>(TSource source, T destination);
}
