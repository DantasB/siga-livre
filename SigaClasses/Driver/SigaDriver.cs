using System;
using System.Net;
using System.Web;
using System.Text;
using System.IO;
using SigaClasses.Models;
using System.Collections.Generic;
using HtmlAgilityPack;
using SigaClasses.Utils;

namespace SigaClasses
{
    public class SigaDriver
    {
        private static readonly Dictionary<string, string> Courses = new Dictionary<string, string>
            {
                { "Engenharia de Computação e Informação - Fundão", "A46B41C8-92A4-F79C-7383-BE627D84214E" },
                { "Engenharia Civil - Fundão", "EB1804EA-92A4-F79D-5F51-DA5503AB533D"},
                { "Engenharia Ambiental - Fundão ", "C32301EF-92A4-FAFE-00E7-DF90826388FA"},
                { "Engenharia de Controle e Automação - Fundão", "DA307421-92A4-F799-6747-B5C24C9E41F2"},
                { "Engenharia de Materiais - Fundão", "3E100A5A-92A4-F79C-051F-6C994AA25736"},
                { "Engenharia de Petróleo - Fundão", "24C6A6A4-92A4-F79C-4038-E6D8B161E716"},
                { "Engenharia de Produção - Fundão", "333C3B46-92A4-F799-3D93-3783890745AC"},
                { "Engenharia Elétrica - Fundão", "F5F60FF8-92A4-F79C-2256-5EE81016A543"},
                { "Engenharia Eletrônica e de Computação - Fundão", "AD403453-92A4-F799-108D-AD9C754FD6A7"},
                { "Engenharia Mecânica - Fundão", "C4C63E6F-92A4-F79B-5BD7-89E4D53BE14A"},
                { "Engenharia Metalúrgica - Fundão", "3E231718-92A4-F79B-1A27-5A2E5FF58D69"},
                { "Engenharia Naval e Oceânica - Fundão", "D431D60F-92A4-F79D-78B9-9F12584902F9"},
                { "Engenharia Nuclear - Fundão", "BFAED97A-92A4-F79C-30D1-28262E56DB1B"},
                { "Engenharia Química Integral - Fundão", "158DBD47-92A4-F799-0F21-2168C111D3CC"},
                { "Engenharia Química Noturno - Fundão", "106A11A1-92A4-F79C-430F-766F2001DB88"},
                { "Engenharia de Bioprocessos - Fundão", "9F4C900B-92A4-FB5F-002A-C92F15792CA9"},
                { "Engenharia de Alimentos - Fundão", "9F4BA9F7-92A4-FB5F-002A-C92FECF233B9"},
                { "Química Industrial Noturno - Fundão", "1FDD32EC-92A4-F79C-430F-766F717D814E"},
                { "Química Industrial Integral - Fundão", "20120240-92A4-F799-0F21-21684A448794"},
                { "Arquitetura e Urbanismo - Fundão", "D99CB5BA-92A4-F713-0041-1509CB806191"},
                { "Ciência da Computação - Fundão", "FB170486-92A4-F79C-13B5-B562AF729978"},
                { "Farmácia Integral - Fundão", "3D93C4E2-92A4-F79A-1828-B8269DED73D1"},
                { "Farmácia Noturno - Fundão", "C03E5428-92A4-F799-241D-5EFC1FA9D1E7"},
                { "Serviço Social Integral - Fundão", "D7E0A446-92A4-F79C-0323-49D8A03CC72A"},
                { "Serviço Social Noturno - Fundão", "C03E5428-92A4-F799-241D-5EFC1FA9D1E7"},
                { "Saúde Coletiva - Fundão", "190569CF-92A4-F799-54BE-04529D312DA2"},
                { "Pintura - Fundão", "14373839-92A4-F79C-1CA2-4FE7964814AF"},
                { "Odontologia - Fundão", "4F3A17FE-3D9B-4CD3-B3F0-E819522A2E9E"},
                { "Nutrição - Fundão", "5D60818B-2D21-4F8A-82EE-5C56321CCAFA"},
                { "Meteorologia - Fundão", "3705396B-92A4-F79D-59E6-DEF32526AE58"},
                { "Geologia - Fundão", "654C42EA-92A4-F79C-39AE-9171EC4971ED"},
                { "Medicina - Fundão", "4CE43D18-92A4-F79C-20CA-3F2DEE9B9C2C"},
                { "Fonoaudiologia - Fundão", "BC2EBF38-B0BE-426C-BCF5-E0E61440193D"},
                { "Fisioterapia - Fundão", "FE6A5A16-92A4-F799-0D3F-4809B3D52912"},
                { "Direito - Centro", "A3905265-92A4-F79B-1703-64F2CC271B41"},
                { "Medicina - Macaé", "CD5A520B-92A4-F79A-06D1-EF0369EEC0EA"},
                { "Nutrição - Macaé", "FCE436FD-92A4-F79C-2107-2F13E9B10AF6"},
                { "Farmácia Integral - Macaé", "D8D58E48-92A4-F79C-0323-49D84014AF3D"},
                { "Astronomia - Observatório do Valongo", "757E21B0-92A4-F799-41C2-11B35155AE05"},
                { "Administração - Praia Vermelha", "FD804294-92A4-F799-634F-A707C86F4A33"},

            };

        public static Disciplinas GetCurrentClassesInfo(string input)
        {
            input          = HttpUtility.UrlDecode(input).ToLower();
            string classUrl = "https://siga.ufrj.br/sira/gradeHoraria/{0}";

            Disciplinas response = new Disciplinas
            {
                Cursos = new Dictionary<string, Dictionary<string, List<Disciplina>>>()
            };

            foreach (string cursos in Courses.Keys)
            {
                if (cursos.ToLower().IndexOf(input, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    string formatedUrl    = string.Format(classUrl, Courses[cursos]);
                    HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(formatedUrl);

                    webReq.KeepAlive              = true;
                    webReq.UserAgent              = "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";
                    webReq.Accept                 = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    webReq.ContentType            = "text/html";
                    webReq.Host                   = "siga.ufrj.br";
                    webReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                    webReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");

                    string htmlResponse = String.Empty;

                    HttpWebResponse htmlContent = (HttpWebResponse)webReq.GetResponse();

                    using (StreamReader reader = new StreamReader(htmlContent.GetResponseStream(), Encoding.GetEncoding("iso-8859-1")))
                    {
                        htmlResponse = reader.ReadToEnd();
                    }

                    htmlResponse = HttpUtility.HtmlDecode(htmlResponse);

                    if (String.IsNullOrWhiteSpace(htmlResponse))
                    {
                        response.Erro = "Não foi possível obter as disciplinas. Tente novamente!";
                        return response;
                    }

                    ParseCourseInfo(htmlResponse, ref response, cursos);
                }

            }

            if(response.Cursos.Count == 0){
                response.Erro = "Curso não implementado.";
            }

            return response;
        }

        public static Disciplinas GetAllClassesInfo()
        {
            string classUrl = "https://siga.ufrj.br/sira/gradeHoraria/{0}";

            Disciplinas response = new Disciplinas
            {
                Cursos = new Dictionary<string, Dictionary<string, List<Disciplina>>>()
            };

            foreach (string cursos in Courses.Keys)
            {
                string formatedUrl = string.Format(classUrl, Courses[cursos]);
                HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(formatedUrl);

                webReq.KeepAlive = true;
                webReq.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";
                webReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                webReq.ContentType = "text/html";
                webReq.Host = "siga.ufrj.br";
                webReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                webReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");

                string htmlResponse = String.Empty;

                HttpWebResponse htmlContent = (HttpWebResponse)webReq.GetResponse();

                using (StreamReader reader = new StreamReader(htmlContent.GetResponseStream(), Encoding.GetEncoding("iso-8859-1")))
                {
                    htmlResponse = reader.ReadToEnd();
                }

                htmlResponse = HttpUtility.HtmlDecode(htmlResponse);

                if (String.IsNullOrWhiteSpace(htmlResponse))
                {
                    response.Erro = "Não foi possível obter as disciplinas. Tente novamente!";
                    return response;
                }

                ParseCourseInfo(htmlResponse, ref response, cursos);
                }

            if (response.Cursos.Count == 0)
            {
                response.Erro = "Curso não implementado.";
            }

            return response;
        }


        private static void ParseCourseInfo(string htmlResponse, ref Disciplinas resposta, string curso)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlResponse);

            Dictionary<string, List<Disciplina>> response = new Dictionary<string, List<Disciplina>>();

            HtmlNodeCollection numberPeriodosNode = htmlDoc.DocumentNode.SelectNodes("//*[@class='tableTitle']");

            List<string> periods = new List<string>();
            if (numberPeriodosNode != null)
            {
                foreach (HtmlNode node in numberPeriodosNode)
                {
                    string texto = Remover.ReplaceMultipleSpacesByPipe(Remover.RemoveSeparators(node.InnerText));
                    string[] splittedTexto = texto.Split('|');
                    if(splittedTexto.Length == 3)
                    {
                        if(splittedTexto[1].Length == 10)
                        {
                            periods.Add(splittedTexto[1]);
                        }
                        if (splittedTexto[1].Length == 11)
                        {
                            periods.Add(splittedTexto[1]);
                        }
                    }
                }
            }

            HtmlNodeCollection periodosNode = htmlDoc.DocumentNode.SelectNodes("//*[@class='table'][1]//table//table//tr");

            if(periodosNode!= null)
            {
                List<Disciplina> disciplinas = new List<Disciplina>();
                Disciplina disciplina = new Disciplina();
                List<Dia> dias = new List<Dia>();
                Dia dia = new Dia();
                int periodo = 0;
                foreach (HtmlNode node in periodosNode)
                {
                    string texto = Remover.ReplaceMultipleSpacesByPipe(Remover.RemoveSeparators(node.InnerText));

                    if (texto.Contains("Professor"))
                    {
                        if (!String.IsNullOrWhiteSpace(disciplina.Código))
                        {
                            disciplina.Dias = dias;
                            disciplinas.Add(disciplina);

                            response.Add(periods[periodo], disciplinas);

                            periodo++;
                            dias = new List<Dia>();
                            disciplina = new Disciplina();
                            disciplinas = new List<Disciplina>();
                        }
                        continue;
                    }
                    string[] splittedText = texto.Split('|');

                    if (String.IsNullOrWhiteSpace(splittedText[1]) && dias.Count != 0)
                    {
                        dia = new Dia
                        {
                            DiaDaSemana = splittedText[4],
                            Horário = splittedText[5]
                        };

                        if (!String.IsNullOrWhiteSpace(splittedText[6]))
                        {
                            dia.SegundoProfessor = splittedText[6];
                        }
                        dias.Add(dia);
                        dia = new Dia();
                    }

                    if (!String.IsNullOrWhiteSpace(splittedText[1]) && dias.Count != 0)
                    {
                        disciplina.Dias = dias;
                        dias = new List<Dia>();

                        disciplinas.Add(disciplina);
                        disciplina = new Disciplina();
                    }

                    if (splittedText.Length == 8 && !String.IsNullOrWhiteSpace(splittedText[1]))
                    {
                        disciplina.Código = splittedText[1];
                        disciplina.Número = splittedText[2];
                        disciplina.Nome = splittedText[3];
                        disciplina.Professor = splittedText[6];

                        dia.DiaDaSemana = splittedText[4];
                        dia.Horário = splittedText[5];

                        dias.Add(dia);
                        dia = new Dia();
                        continue;
                    }
                    if (splittedText.Length == 9 && !String.IsNullOrWhiteSpace(splittedText[1]))
                    {
                        disciplina.Código = splittedText[1];
                        disciplina.Número = splittedText[2];
                        disciplina.Nome = splittedText[3] + " " + splittedText[4];
                        disciplina.Professor = splittedText[7];

                        dia.DiaDaSemana = splittedText[5];
                        dia.Horário = splittedText[6];

                        dias.Add(dia);
                        dia = new Dia();
                        continue;
                    }

                }

                if (dias.Count != 0)
                {
                    disciplina.Dias = dias;
                    disciplinas.Add(disciplina);
                    response.Add(periods[periodo], disciplinas);
                }

            }

            HtmlNodeCollection ComplementaresNode = htmlDoc.DocumentNode.SelectNodes("//*[@class='table'][2]//table//table//tr");

            if (ComplementaresNode != null)
            {
                List<Disciplina> disciplinas = new List<Disciplina>();
                Disciplina disciplina = new Disciplina();
                List<Dia> dias = new List<Dia>();
                Dia dia = new Dia();

                foreach (HtmlNode node in ComplementaresNode)
                {
                    string texto = Remover.ReplaceMultipleSpacesByPipe(Remover.RemoveSeparators(node.InnerText));

                    if (texto.Contains("Professor"))
                    {
                        continue;
                    }
                    string[] splittedText = texto.Split('|');

                    if (String.IsNullOrWhiteSpace(splittedText[1]) && dias.Count != 0)
                    {
                        dia = new Dia
                        {
                            DiaDaSemana = splittedText[4],
                            Horário = splittedText[5]
                        };

                        if (!String.IsNullOrWhiteSpace(splittedText[6]))
                        {
                            dia.SegundoProfessor = splittedText[6];
                        }
                        dias.Add(dia);
                        dia = new Dia();
                    }

                    if (!String.IsNullOrWhiteSpace(splittedText[1]) && dias.Count != 0)
                    {
                        disciplina.Dias = dias;
                        dias = new List<Dia>();

                        disciplinas.Add(disciplina);
                        disciplina = new Disciplina();
                    }

                    if (splittedText.Length == 8 && !String.IsNullOrWhiteSpace(splittedText[1]))
                    {
                        disciplina.Código = splittedText[1];
                        disciplina.Número = splittedText[2];
                        disciplina.Nome = splittedText[3];
                        disciplina.Professor = splittedText[6];

                        dia.DiaDaSemana = splittedText[4];
                        dia.Horário = splittedText[5];

                        dias.Add(dia);
                        dia = new Dia();
                        continue;
                    }
                    if (splittedText.Length == 9 && !String.IsNullOrWhiteSpace(splittedText[1]))
                    {
                        disciplina.Código = splittedText[1];
                        disciplina.Número = splittedText[2];
                        disciplina.Nome = splittedText[3] + " " + splittedText[4];
                        disciplina.Professor = splittedText[7];

                        dia.DiaDaSemana = splittedText[5];
                        dia.Horário = splittedText[6];

                        dias.Add(dia);
                        dia = new Dia();
                        continue;
                    }
                }

                if (dias.Count != 0)
                {
                    disciplina.Dias = dias;
                    disciplinas.Add(disciplina);
                }

                response.Add("Complementares", disciplinas);
            }

            resposta.Cursos.Add(curso, response);

        }

    }
}