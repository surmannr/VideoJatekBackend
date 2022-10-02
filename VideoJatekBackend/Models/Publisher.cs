using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VideoJatekBackend.Models
{
    [Table("Kiado")]
    public class Publisher
    {
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("Nev")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [Column("Cim")]
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [Column("Alapitas_datuma")]
        [JsonPropertyName("foundationDate")]
        public DateTime FoundationDate { get; set; }

        [JsonIgnore]
        public ICollection<Videogame> Videogames { get; set; }

    }
}
