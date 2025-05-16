namespace InnerSystem.Api.Helper;

public interface IPostImageSaveHelper
{
	Task<string> SaveImageAsync(IFormFile image);
}
