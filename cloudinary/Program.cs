using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudinary
{
    class Program
    {

        static bool WriteToFile(string filepath,List<string> lines)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }

            return true;
        }


        static ImageUploadResult UploadImages(Cloudinary cloudinary, string sourceImageUrl,string publicId)
        {
            var uploadParams = new ImageUploadParams()
            {
                //File = new FileDescription(@"C:\cartman.jpg"),
                File = new FileDescription(sourceImageUrl),
                PublicId = publicId
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult;
        }
        static void Main(string[] args)
        {
            string sourceAccountName = "[AccountName Of Source]",
                   sourceApiKey = "[ApiKey Of Source]",
                   sourceApiSecret = "[ApiSecret Of Source]";

            string destinationAccountName = "[AccountName Of Destination]",
                  destinatioApiKey = "[ApiKey Of Destination]",
                  destinatioApiSecret = "[ApiSecret Of Destination]";

            string successFilePath = @"C:\Users\Public\Documents\Sucess.txt", errorFilePath = @"C:\Users\Public\Documents\Error.txt", urlFilePath = @"C:\Users\Public\Documents\Urls.txt",exceptionsFilePath = @"C:\Users\Public\Documents\exceptionsFilePath.txt";
            //var sourceAccount = new Account(sourceAccountName, sourceApiKey, sourceApiSecret);
            var destinationAccount = new Account(destinationAccountName, destinatioApiKey, destinatioApiSecret);
            var cloudinary = new Cloudinary(destinationAccount);
            
            string imagesToUpload = "{CommaSepereatedValues of Public IDS of source images}";
            string sourceUri = "http://res.cloudinary.com/"+ sourceAccountName + "/image/upload/";
            string destinationUri = "http://res.cloudinary.com/"+ destinationAccountName + "/image/upload/";

            List<string> listImages = imagesToUpload.Split(',').ToList(),
                         successResult = new List<string>(), errorResult = new List<string>(), exceptions = new List<string>(), urlList = new List<string>();
            int count = 0;
            foreach (var image in listImages)
            {
                try
                {
                    count++;
                    Console.WriteLine(count);
                    string sourceImageUrl = sourceUri + image + ".png", publicId = image;
                    var result = UploadImages(cloudinary, sourceImageUrl, publicId);
                    Debug.WriteLine(result.SecureUri);
                    successResult.Add(image);
                    urlList.Add(result.SecureUri.ToString());
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    exceptions.Add(ex.Message);
                    errorResult.Add(image);
                }
            }

            WriteToFile(successFilePath, successResult);
            WriteToFile(errorFilePath, errorResult);
            WriteToFile(exceptionsFilePath, exceptions);
            WriteToFile(urlFilePath, urlList);
            Console.ReadLine();
        }


        
    }
}
