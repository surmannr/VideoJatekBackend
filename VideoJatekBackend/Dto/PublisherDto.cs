using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using VideoJatekBackend.Models;

namespace VideoJatekBackend.Dto
{
    public class PublisherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime FoundationDate { get; set; }

        public List<VideogameDto> Videogames { get; set; }
    }
}
