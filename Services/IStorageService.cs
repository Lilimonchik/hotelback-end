using System;
using Hotel.Models;

namespace Hotel.Services
{
	public interface IStorageService
	{
        Task<S3ResponseDto> UploadFileAsync(S3Object obj, AwsCredentials awsCredentialsValues);
    }
}

