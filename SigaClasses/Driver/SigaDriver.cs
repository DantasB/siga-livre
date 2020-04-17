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
            { "Artes Cênicas Cenografia - Fundão", "810B1869-92A4-F79A-7EF3-0FCE4A954AC1"},
            { "Artes Cênicas Indumentária - Fundão", "81045C0A-92A4-F79C-7BF7-911B399907CD"},
            { "Artes Visuais Gravura - Fundão", "94031F33-92A4-F799-77BD-9CA93F36BBA8"},
            { "Astronomia - Ênfase: Astrofísica - Observatório do Valongo", "7613365E-92A4-F79F-6268-CA7155B029B5" },
            { "Astronomia - Ênfase: Astronomia Computacional - Observatório do Valongo", "761658BA-92A4-F79F-6268-CA716A32D813" },
            { "Astronomia - Ênfase: Astronomia Instrumental - Observatório do Valongo", "7AC10A59-92A4-F79B-1E8E-D192798C6B4B" },
            { "Astronomia - Ênfase: Astronomia Matemática - Observatório do Valongo", "7A99D464-92A4-F79C-4BFC-2F8C1F81AE86" },
            { "Astronomia - Ênfase: Difusão da Astronomia - Observatório do Valongo", "7AA1894A-92A4-F79C-4BFC-2F8C433EF3D5" },
            { "Bacharelado em Ciências Matemáticas e da Terra - Fundão", "96821A16-92A4-F79B-6A3A-191EB7AE79D3"},
            { "Bacharelado em Ciências Matemáticas e da Terra Análise de Suporte à Decisão - Fundão", "A67BE5CD-92A4-F79D-4AFC-818CA3D6E5F3"},
            { "Bacharelado em Ciências Matemáticas e da Terra Ciência da Terra e Patrimônio Natural - Fundão", "13E0139A-92A4-F79B-5BE4-E5C620311D77"},
            { "Bacharelado em Ciências Matemáticas e da Terra Sensoriamento Remoto e Geoprocessamento - Fundão", "8C768E10-92A4-F79B-6A3A-191E92752891"},
            { "Bacharelado em Letras: Libras Noturno - Fundão", "6C5E1BC5-92A4-F79C-7BF7-911B6569D542"},
            { "Ciência da Computação - Fundão", "FB170486-92A4-F79C-13B5-B562AF729978"},
            { "Ciências Atuariais - Fundão", "8291DE00-92A4-F716-0036-96D0129B07AE" },
            { "Composição Paisagística - Fundão", "378DAA4D-92A4-F799-136A-F14DDFB299B5"},
            { "Composição de Interior - Fundão", "A24C0BBB-D6D0-42C1-9966-6A80AB66EEEF"},
            { "Comunicação Visual Design - Fundão", "BED7B95D-92A4-F79D-77AC-F1C034C21FFF"},
            { "Conservação e Restauração - Fundão", "E763726D-92A4-F799-4F11-241E18418366"},
            { "Desenho Industrial Projeto do Produto - Fundão", "B81433DC-2DE2-4C8F-A411-0F4C2B388AC5"},
            { "Direito - Centro", "A3905265-92A4-F79B-1703-64F2CC271B41"},
            { "Engenharia Ambiental - Fundão ", "C32301EF-92A4-FAFE-00E7-DF90826388FA"},
            { "Engenharia Civil - Fundão", "EB1804EA-92A4-F79D-5F51-DA5503AB533D"},
            { "Engenharia Eletrônica e de Computação - Fundão", "AD403453-92A4-F799-108D-AD9C754FD6A7"},
            { "Engenharia Elétrica - Fundão", "F5F60FF8-92A4-F79C-2256-5EE81016A543"},
            { "Engenharia Matemática - Fundão", "14BA7CE6-92A4-F799-1F31-15AB6442F8A8"},
            { "Engenharia Mecânica - Fundão", "C4C63E6F-92A4-F79B-5BD7-89E4D53BE14A"},
            { "Engenharia Metalúrgica - Fundão", "3E231718-92A4-F79B-1A27-5A2E5FF58D69"},
            { "Engenharia Naval e Oceânica - Fundão", "D431D60F-92A4-F79D-78B9-9F12584902F9"},
            { "Engenharia Nuclear - Fundão", "BFAED97A-92A4-F79C-30D1-28262E56DB1B"},
            { "Engenharia Química Integral - Fundão", "158DBD47-92A4-F799-0F21-2168C111D3CC"},
            { "Engenharia Química Noturno - Fundão", "106A11A1-92A4-F79C-430F-766F2001DB88"},
            { "Engenharia de Alimentos - Fundão", "9F4BA9F7-92A4-FB5F-002A-C92FECF233B9"},
            { "Engenharia de Bioprocessos - Fundão", "9F4C900B-92A4-FB5F-002A-C92F15792CA9"},
            { "Engenharia de Computação e Informação - Fundão", "A46B41C8-92A4-F79C-7383-BE627D84214E" },
            { "Engenharia de Controle e Automação - Fundão", "DA307421-92A4-F799-6747-B5C24C9E41F2"},
            { "Engenharia de Materiais - Fundão", "3E100A5A-92A4-F79C-051F-6C994AA25736"},
            { "Engenharia de Petróleo - Fundão", "24C6A6A4-92A4-F79C-4038-E6D8B161E716"},
            { "Engenharia de Produção - Fundão", "333C3B46-92A4-F799-3D93-3783890745AC"},
            { "Estatística - Fundão", "243658D9-92A4-F79A-27F6-39968D2F8BF5" },
            { "Farmácia Integral - Fundão", "3D93C4E2-92A4-F79A-1828-B8269DED73D1"},
            { "Farmácia Noturno - Fundão", "C03E5428-92A4-F799-241D-5EFC1FA9D1E7"},
            { "Fisioterapia - Fundão", "FE6A5A16-92A4-F799-0D3F-4809B3D52912"},
            { "Fonoaudiologia - Fundão", "BC2EBF38-B0BE-426C-BCF5-E0E61440193D"},
            { "Física - Fundão", "1BB12920-92A4-F799-32E7-93CEA3D09843"},
            { "Física Médica - Fundão", "6684EB75-92A4-F799-6FA2-7E7CBE506055"},
            { "Geografia Integral - Fundão", "BB18B459-EFDB-453D-B1C2-37C5C15C713D"},
            { "Geografia Noturno - Fundão", "D7C872F2-9965-4279-A6D4-BA9CFB37C8C4"},
            { "Geologia - Fundão", "654C42EA-92A4-F79C-39AE-9171EC4971ED"},
            { "Gravura - Fundão", "523B2D0D-453C-4EEF-8710-A3DA7CB3C9E6"},
            { "História da Arte - Fundão", "B3EAFFE1-92A4-F799-77BD-9CA99B5C020E"},
            { "Letras: Português-Alemão - Fundão", "E43EA678-92A4-F79C-7445-AE4ECE48A675"},
            { "Letras: Português-Francês - Fundão", "E4475ECC-92A4-F79C-7445-AE4E191548CE"},
            { "Letras: Português-Grego - Fundão", "F84B1150-92A4-F799-136A-F14DEC71A221"},
            { "Letras: Português-Hebraico - Fundão", "F1C62AED-92A4-F79B-0232-AE98E9F1B350"},
            { "Letras: Português-Inglês - Fundão", "F1C84B57-92A4-F79B-0232-AE986ED055CF"},
            { "Letras: Português-Italiano - Fundão", "F8787C29-92A4-F79A-4D80-948F50151C11"},
            { "Letras: Português-Japonês - Fundão", "F1CAA13F-92A4-F79B-0232-AE98C3F0B5E2"},
            { "Letras: Português-Latim - Fundão", "F1CB92DC-92A4-F79B-0232-AE9801D30DA3"},
            { "Letras: Português-Literaturas Integral - Fundão", "F1CC7D4D-92A4-F79B-0232-AE984EA53CE5"},
            { "Letras: Português-Literaturas Noturno - Fundão", "A16E4429-92A4-F79B-5556-D74F742EB37C"},
            { "Letras: Português-Russo - Fundão", "F1CD662A-92A4-F79B-0232-AE9807CA3957"},
            { "Letras: Português-Árabe - Fundão", "E441F77B-92A4-F79C-7445-AE4E6BFF0035"},
            { "Licenciatura em Educação Artística Artes Plásticas - Fundão", "8FE2D2CC-C04A-4274-8DF3-45102CDFA32F"},
            { "Licenciatura em Educação Artística Desenho - Fundão", "85724BFD-8D2A-48BF-86AA-5749D7EE5759"},
            { "Licenciatura em Física Integral - Fundão", "E67DAE9A-D8A1-4CF4-BD8A-607B96B0D116"},
            { "Licenciatura em Física Noturno - Fundão", "19752C45-92A4-F79A-0B10-6AA3DC70319A"},
            { "Licenciatura em Letras: Libras Noturno - Fundão", "717D6B88-92A4-F79B-3A19-6232A8F2D31C"},
            { "Licenciatura em Letras: Português-Alemão - Fundão", "236BFEBD-92A4-F79B-7092-FB4167B430DC"},
            { "Licenciatura em Letras: Português-Francês - Fundão", "06FFCE55-92A4-F799-11F1-C7D0F1BFCCC9"},
            { "Licenciatura em Letras: Português-Grego - Fundão", "E94A09E6-92A4-F79D-11C1-497A4ECC1F08"},
            { "Licenciatura em Letras: Português-Hebraico - Fundão", "0C8F2EE6-92A4-F79D-11C1-497AA701ED1A"},
            { "Licenciatura em Letras: Português-Inglês - Fundão", "2DF29E3A-92A4-F79A-1F23-8D326C53D0AE"},
            { "Licenciatura em Letras: Português-Italiano - Fundão", "2369D4DC-92A4-F79B-7092-FB416514DA83"},
            { "Licenciatura em Letras: Português-Japonês - Fundão", "08449B9E-92A4-F79C-576D-51554481ECDB"},
            { "Licenciatura em Letras: Português-Latim - Fundão", "DF17FFBB-92A4-F79B-609E-E01B12F7DC57"},
            { "Licenciatura em Letras: Português-Literaturas Integral - Fundão", "BA7DA06B-92A4-F799-3C43-14FEDDFC9320"},
            { "Licenciatura em Letras: Português-Literaturas Noturno - Fundão", "BA83357A-92A4-F799-3C43-14FE60627438"},
            { "Licenciatura em Letras: Português-Russo - Fundão", "EE9B8339-92A4-F79B-609E-E01BA9D88781"},
            { "Licenciatura em Letras: Português-Árabe - Fundão", "0C88BCDB-92A4-F79D-11C1-497A21779458"},
            { "Licenciatura em Matemática Integral - Fundão", "A28E3C7D-92A4-F799-2D72-BC2413894D9C" },
            { "Licenciatura em Matemática Noturno - Fundão", "A298CF1E-92A4-F799-2D72-BC240ADD01E9" },
            { "Licenciatura em Música - Escola de Música", "8AD52198-92A4-F79D-342B-E5BC7B0A7BC5"},
            { "Licenciatura em Química Integral - Fundão", "F377ECA8-8C1B-4FE1-9F62-93989955E8C6"},
            { "Licenciatura em Química Noturno - Fundão", "B0B38E9A-92A4-F79A-5843-0CB23B379573"},
            { "Matemática - Fundão", "A78A8FDB-92A4-F799-2D72-BC24C5B74786" },
            { "Matemática - Ênfase: Matemática - Fundão", "5CD3127C-92A4-F79A-4D39-A0059BDAE0A3" },
            { "Matemática - Ênfase: Matemática Computacional - Fundão", "A78D4D72-92A4-F79D-1E72-2083F1F6BD1F" },
            { "Matemática - Ênfase: Matemática Estatística - Fundão", "B97BCCC1-92A4-F799-1134-FADA9D2E66C5" },
            { "Matemática Aplicada - Fundão", "818CB277-AD26-4DC5-A033-F5B28338704C" },
            { "Matemática Aplicada - Ênfase: Computação Científica - Fundão", "337F8BBC-DEE7-48E1-83C1-4040FE8A4EC4" },
            { "Matemática Aplicada - Ênfase: Matemática Ciências Biológicas - Fundão", "66FE8FFE-756C-40F6-854A-A9129BBD948E" },
            { "Matemática Aplicada - Ênfase: Matemática de Negócios - Fundão", "66FE8FFE-756C-40F6-854A-A9129BBD948E" },
            { "Medicina - Fundão", "4CE43D18-92A4-F79C-20CA-3F2DEE9B9C2C"},
            { "Medicina - Macaé", "CD5A520B-92A4-F79A-06D1-EF0369EEC0EA"},
            { "Meteorologia - Fundão", "3705396B-92A4-F79D-59E6-DEF32526AE58"},
            { "Musicoterapia Noturno - PV", "EF67C44E-92A4-F799-241D-5EFCD06246B6"},
            { "Música Bandolim - Escola de Música", "9D19434F-92A4-F79D-6560-DE7CDB9AAD11"},
            { "Música Canto - Escola de Música", "B750CADE-92A4-F79E-1D3E-4F820A5CF5DC"},
            { "Música Cavaquinho - Escola de Música", "8A4FFAE8-92A4-F79C-1B6D-99797F6125B0"},
            { "Música Clarineta - Escola de Música", "0470E49E-92A4-F79B-1C98-5BEF1E5A123E"},
            { "Música Composição - Escola de Música", "13408F82-92A4-F79A-2FD0-98088E23C4F2"},
            { "Música Contrabaixo - Escola de Música", "13EB1B87-92A4-F79B-1C98-5BEFEAAB818A"},
            { "Música Cravo - Escola de Música", "18DE71C9-92A4-F79D-3849-3622A150AD8B"},
            { "Música Fagote - Escola de Música", "19472482-92A4-F79D-3849-362245EF94BE"},
            { "Música Flauta - Escola de Música", "1420781D-92A4-F79D-3849-36229D5FF8FE"},
            { "Música Harpa - Escola de Música", "46978312-92A4-F79E-2B57-8C6193E09FFD"},
            { "Música Oboé - Escola de Música", "190DF990-92A4-F79D-3849-362242123193"},
            { "Música Percussão - Escola de Música", "134BE0DD-92A4-F79D-3849-362273136F27"},
            { "Música Regência - Escola de Música", "EEE6EF0E-8869-4CC2-A7C9-757204745FE5"},
            { "Música Regência Coral - Escola de Música", "225E98FA-92A4-F79B-1C98-5BEF7F5C9D03"},
            { "Música Regência Orquestral - Escola de Música", "1E809FC1-92A4-F799-41DD-3F8A36C762F1"},
            { "Música Regência de Banda - Escola de Música", "045177B9-92A4-F79C-6516-D64046A53EC5"},
            { "Música Saxofone - Escola de Música", "1DC93E35-92A4-F79D-3849-36221776B7E8"},
            { "Música Trombone - Escola de Música", "1D75B4CF-92A4-F79D-3849-3622C1FC9263"},
            { "Música Trompa - Escola de Música", "1DA636AE-92A4-F79D-3849-3622E7A462E5"},
            { "Música Trompete - Escola de Música", "1D4B9D66-92A4-F79D-3849-362297DEC2C9"},
            { "Música Tuba - Escola de Música", "18D72927-92A4-F79C-6516-D640D9496EB4"},
            { "Música Viola - Escola de Música", "187ED101-92A4-F799-41DD-3F8AB47C4082"},
            { "Música Violino - Escola de Música", "1D63A231-92A4-F79D-3849-3622A2A29C28"},
            { "Música Violão - Escola de Música", "037E716C-92A4-F79E-7F24-3900BA12CD6F"},
            { "Música Órgão - Escola de Música", "045177B9-92A4-F79C-6516-D64046A53EC5"},
            { "Nutrição - Fundão", "5D60818B-2D21-4F8A-82EE-5C56321CCAFA"},
            { "Nutrição - Macaé", "FCE436FD-92A4-F79C-2107-2F13E9B10AF6"},
            { "Odontologia - Fundão", "4F3A17FE-3D9B-4CD3-B3F0-E819522A2E9E"},
            { "Paisagismo - Fundão", "AF6F875F-92A4-F79C-7203-E3B50910CAF4"},
            { "Pintura - Fundão", "14373839-92A4-F79C-1CA2-4FE7964814AF"},
            { "Química - Atribuições Tecnológicas - Fundão", "2D75E4B0-92A4-F79C-6405-62485F9051FB"},
            { "Química Industrial Integral - Fundão", "20120240-92A4-F799-0F21-21684A448794"},
            { "Química Industrial Noturno - Fundão", "1FDD32EC-92A4-F79C-430F-766F717D814E"},
            { "Química Noturno - Fundão", "F1E64377-92A4-F79C-2576-734D37CC92A3"},
            { "Saúde Coletiva - Fundão", "190569CF-92A4-F799-54BE-04529D312DA2"},
            { "Serviço Social Integral - Fundão", "D7E0A446-92A4-F79C-0323-49D8A03CC72A"},
            { "Serviço Social Noturno - Fundão", "D7E142B7-92A4-F79C-0323-49D8600587D2"},
            { "Arquitetura e Urbanismo - Fundão", "D99CB5BA-92A4-F713-0041-1509CB806191"},
        };

        public static Disciplinas GetCurrentClassesInfo(string input)
        {
            input           = HttpUtility.UrlDecode(input).ToLower();
            string classUrl = "https://siga.ufrj.br/sira/gradeHoraria/{0}";

            Disciplinas response = new Disciplinas
            {
                Cursos = new Dictionary<string, Dictionary<string, List<Disciplina>>>()
            };

            foreach (string cursos in Courses.Keys)
            {
                if (cursos.ToLower().IndexOf(input, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    Result storedData = MongoHandler.IsOnMongo(Courses[cursos]);
                    if (storedData != null)
                    {
                        response.Cursos.Add(Courses[cursos], storedData.Disciplinas);
                        continue;
                    }

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

                Result storedData  = MongoHandler.IsOnMongo(cursos);
                if (storedData != null)
                {
                    response.Cursos.Add(cursos, storedData.Disciplinas);
                    continue;
                }

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
            Result data = new Result
            {
                Curso = curso,
                Disciplinas = response
            };

            MongoHandler.SaveOnMongo(data);
        }

    }
}