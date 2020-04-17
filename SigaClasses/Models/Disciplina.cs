using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;


namespace SigaClasses.Models
{

    public class Disciplina
    {
        public string Código { get; set; }
        public string Número { get; set; }
        public string Nome { get; set; }
        public string Professor { get; set; }
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

    public class Result
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Curso { get; set; }
        public Dictionary<string, List<Disciplina>> Disciplinas { get; set; }
        public string Ano { get; set; }
    }

}