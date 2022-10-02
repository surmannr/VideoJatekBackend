using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VideoJatekBackend.Dto
{
    public class VideogameDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public string Developer { get; set; }
        public int PublisherId { get; set; }
    }
}
