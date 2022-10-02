using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoJatekBackend.Models;

namespace VideoJatekBackend.Dal.Seed
{
    public static class PublisherJsonFileProcessor
    {
        public static void Deserialize(IServiceProvider serviceProvider)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string fileName = "publisher.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            string jsonString = File.ReadAllText(path);
            var publisherList = JsonSerializer.Deserialize<ICollection<Publisher>>(jsonString, options);

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<VideogameDbContext>();
                foreach (var publisher in publisherList)
                {
                    var publ = context.Publishers.SingleOrDefault(a => a.Id == publisher.Id);
                    if (publ == null)
                    {
                        context.Add(publisher);
                    } else
                    {
                        publ.Name = publisher.Name;
                        publ.Address = publisher.Address;
                        publ.FoundationDate = publisher.FoundationDate;
                    }
                   
                }
                context.SaveChanges();
                SerializeList(context.Publishers.ToList()); // Konzisztens maradjon a db tartalmával.
            }
        }

        public static void DeserializeDb(VideogameDbContext context)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string fileName = "publisher.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            string jsonString = File.ReadAllText(path);
            var publisherList = JsonSerializer.Deserialize<ICollection<Publisher>>(jsonString, options);

            foreach (var publisher in publisherList)
            {
                var publ = context.Publishers.SingleOrDefault(a => a.Id == publisher.Id);
                if (publ == null)
                {
                    context.Add(publisher);
                }
                else
                {
                    publ.Name = publisher.Name;
                    publ.Address = publisher.Address;
                    publ.FoundationDate = publisher.FoundationDate;
                }
            }
            context.SaveChanges();
            SerializeList(context.Publishers.ToList()); // Konzisztens maradjon a db tartalmával.
        }

        public static List<Publisher> DeserializedList()
        {
            string fileName = "publisher.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            string jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Publisher>>(jsonString);
        }

        public static void SerializeList(List<Publisher> list)
        {
            string fileName = "publisher.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            var jsonString = JsonSerializer.Serialize(list);
            File.WriteAllText(path, jsonString);
        }
    }
}
