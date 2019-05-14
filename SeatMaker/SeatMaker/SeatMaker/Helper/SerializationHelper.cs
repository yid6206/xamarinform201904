using Newtonsoft.Json;
using SeatMaker.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SeatMaker.Helper
{
    public static class SerializationHelper
    {
        private static readonly string MyDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SeatMaker");
        private static readonly string ImageName = "img.png";

        public static void SaveMembers(Member[] memberList)
        {
            if (!Directory.Exists(MyDirectory))
                Directory.CreateDirectory(MyDirectory);

            string filePath = Path.Combine(MyDirectory, "members.json");
            Serialize(filePath, memberList);
        }

        public static Member[] LoadMembers()
        {
            var members = new List<Member>();
            string filePath = Path.Combine(MyDirectory, "members.json");
            if (!File.Exists(filePath))
                return members.ToArray();

            string text = File.ReadAllText(filePath);
            members = Deserialize<List<Member>>(text);
            return members.ToArray();
        }

        public static void SaveImage(SKBitmap bitmap)
        {
            if (!Directory.Exists(MyDirectory))
                Directory.CreateDirectory(MyDirectory);

            var filePath = Path.Combine(MyDirectory, ImageName);
            using (var stream = File.Create(filePath))
            {
                var image = SKImage.FromBitmap(bitmap);
                var data = image.Encode(SKEncodedImageFormat.Png, 100);
                data.SaveTo(stream);
            }
        }

        public static SKBitmap LoadImage()
        {
            var filePath = Path.Combine(MyDirectory, ImageName);
            if (!File.Exists(filePath))
                return null;
            return SKBitmap.Decode(File.ReadAllBytes(filePath));
        }

        public static async Task SendImageToTeamsAsync(string url)
        {
            var filePath = Path.Combine(MyDirectory, ImageName);
            if (!File.Exists(filePath))
                return;
            Debug.WriteLine("end load image");

            var imageUri = await AzureStorageHelper.UpdateFileAsync(filePath);
            if (string.IsNullOrEmpty(imageUri))
            {
                Debug.WriteLine("faild upload to azure ");
                return;
            }
            Debug.WriteLine("sucess upload to azure ");

            var sender = new TeamsMessageSender(url);
            if (await sender.SendImageAsync("テスト", imageUri))
                Debug.WriteLine("sucess to teams");
            else
                Debug.WriteLine("faild to teams");
        }

        private static void Serialize<T>(string filePath, T entity)
        {
            var serializer = new JsonSerializer();

            using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(jsonWriter, entity);
            }
        }

        private static T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
