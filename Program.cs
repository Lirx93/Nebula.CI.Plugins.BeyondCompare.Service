using System;
using System.IO;
using System.Net.Http;

namespace Nebula.CI.Plugins.BeyondCompare.Service
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string url = args[0];
            string filepath = args[1];
            string cmd = args[2];
            string resultPath = args[3];
            string savePath = args[4];

            string boundary = DateTime.Now.Ticks.ToString("X");
            var formData = new MultipartFormDataContent(boundary);
            formData.Add(new StringContent(cmd), "CMD"); 
            formData.Add(new StringContent(resultPath), "ResultPath"); 
            
            if(filepath != " ")
            {
                var stream = await System.IO.File.ReadAllBytesAsync(filepath);

                ByteArrayContent imageContent = new ByteArrayContent(stream);
                formData.Add(imageContent, "imgs", "source.zip");
            }
            
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(url, formData);
            response.EnsureSuccessStatusCode();
            var file = await response.Content.ReadAsStreamAsync();
            
            using (var fs = new FileStream(savePath, FileMode.Create)) {
                await file.CopyToAsync(fs);
            }
        }
    }
}
