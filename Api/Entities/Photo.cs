using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities
{
    //-informar ao entity framework que é para criar uma tabela com nome "Photos" no banco.
     [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        //-estas propriedades foram criadas para que classe Photo saiba da existência da classe AppUser,
        //-com isso o migrations irá criar uma chave estrangeira não nulável.
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}