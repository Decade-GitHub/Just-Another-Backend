using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AccessControlAPI.Models
{
    public class CEP
    {
        [Key]
        [Column("cep")]
        public string? IdCEP { get; set; }

        [JsonProperty("bairro")]
        public string? Bairro { get; set; }

        [MaxLength(50)]
        [JsonProperty("cidade")]
        public string? Cidade { get; set; }

        [MaxLength(50)]
        [JsonProperty("logradouro")]
        public string? Logradouro { get; set; }

        public DateTime? DataInclusao { get; set; }
    }
}