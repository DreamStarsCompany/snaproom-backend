using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using SnapRoom.Common.Base;
using SnapRoom.Common.Enum;

namespace SnapRoom.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		[HttpPost("upload")]
		public async Task<IActionResult> UploadImage(IFormFile imageFile)
		{

			string connectionString = "DefaultEndpointsProtocol=https;AccountName=dataimage;AccountKey=EculdS+KG3G0NWHbDqcJAzjmR/6xeXXawureAr8j/SEOdvyv2tjdEvckOuaHYeRFh73MDoixrLHD+ASts0Eudw==;EndpointSuffix=core.windows.net";
			string containerName = "snaproom";  // Your blob container name

			var blobServiceClient = new BlobServiceClient(connectionString);
			var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

			// Optional: create container if it doesn't exist
			await containerClient.CreateIfNotExistsAsync();

			// Use original filename or generate a unique one
			string blobName = imageFile.FileName;

			// Get a BlobClient representing the blob to upload to
			var blobClient = containerClient.GetBlobClient(blobName);

			// Upload the image file stream to the blob with Content-Type set
			using (var stream = imageFile.OpenReadStream())
			{
				var options = new BlobUploadOptions
				{
					HttpHeaders = new BlobHttpHeaders { ContentType = imageFile.ContentType }
				};

				await blobClient.UploadAsync(stream, options);
			}

			string baseUrl = "https://dataimage.blob.core.windows.net/snaproom/";

			return Ok(new BaseResponse<object>(
				statusCode: StatusCodeEnum.OK,
				message: "Tải hình thành công",
				data: baseUrl + imageFile.FileName
			));
		}

	}
}
