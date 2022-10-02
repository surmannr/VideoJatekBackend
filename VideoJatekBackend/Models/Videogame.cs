using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VideoJatekBackend.Models
{
    [Table("Videojatek")]
    public class Videogame
    {
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("Nev")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [Column("Kategoria")]
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [Column("Fejleszto")]
        [JsonPropertyName("developer")]
        public string Developer { get; set; }

        [Column("KiadoId")]
        [JsonPropertyName("publisherId")]
        public int PublisherId { get; set; }
        [JsonIgnore]
        public Publisher Publisher { get; set; }

    }
}
