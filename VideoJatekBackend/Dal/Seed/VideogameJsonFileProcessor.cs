using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoJatekBackend.Models;

namespace VideoJatekBackend.Dal.Seed
{
    public static class VideogameJsonFileProcessor
    {
        public static void Deserialize(IServiceProvider serviceProvider)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string fileName = "videogame.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            string jsonString = File.ReadAllText(path);
            var videogameList = JsonSerializer.Deserialize<ICollection<Videogame>>(jsonString,options);

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<VideogameDbContext>();
                foreach (var videogame in videogameList)
                {
                    var vid = context.Videogames.SingleOrDefault(a => a.Id == videogame.Id);
                    if (vid != null)
                    {
                        vid.PublisherId = videogame.PublisherId;
                        vid.Category = videogame.Category;
                        vid.Developer = videogame.Developer;
                        vid.Name = videogame.Name;
                    } else
                    {
                        context.Add(videogame);
                    }
                    
                }
                context.SaveChanges();
                SerializeList(context.Videogames.ToList()); // Konzisztens maradjon a db tartalmával.
            }
        }

        public static void DeserializeDb(VideogameDbContext context)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string fileName = "videogame.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            string jsonString = File.ReadAllText(path);
            var videogameList = JsonSerializer.Deserialize<ICollection<Videogame>>(jsonString, options);

            foreach (var videogame in videogameList)
            {
                var vid = context.Videogames.SingleOrDefault(a => a.Id == videogame.Id);
                if (vid != null)
                {
                    vid.PublisherId = videogame.PublisherId;
                    vid.Category = videogame.Category;
                    vid.Developer = videogame.Developer;
                    vid.Name = videogame.Name;
                }
                else
                {
                    context.Add(videogame);
                }
            }
            context.SaveChanges();
            SerializeList(context.Videogames.ToList()); // Konzisztens maradjon a db tartalmával.
        }

        public static List<Videogame> DeserializedList()
        {
            string fileName = "videogame.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            string jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Videogame>>(jsonString);
        }

        public static void SerializeList(List<Videogame> list)
        {
            string fileName = "videogame.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            var jsonString = JsonSerializer.Serialize(list);
            File.WriteAllText(path, jsonString);
        }
    }
}
