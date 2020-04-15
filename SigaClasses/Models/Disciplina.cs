using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;


namespace SigaClasses.Models
{
    [BsonDiscriminator("Disciplina")]
    public class Disciplina
    {

        [BsonElement("Código")]
        public string Código { get; set; }
        [BsonElement("Número")]
        public string Número { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("Professor")]
        public string Professor { get; set; }

        [BsonElement("Dias")]
        public List<Dia> Dias { get; set; }
    }

    public class Dia
    {
        public string DiaDaSemana { get; set; }
        public string Horário { get; set; }
        public string SegundoProfessor { get; set; }
    }

    public class Disciplinas
    {
        public Dictionary<string, Dictionary<string, List<Disciplina>>> Cursos { get; set; }
        public string Erro { get; set; }
    }

}