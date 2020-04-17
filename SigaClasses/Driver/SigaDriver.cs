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
            { "Administração Tarde e Noite - Praia Vermelha", "FD804294-92A4-F799-634F-A707C86F4A33"},
            { "Arquitetura e Urbanismo - Fundão", "D99CB5BA-92A4-F713-0041-1509CB806191"},
            { "Artes Cênicas Cenografia - Fundão", "810B1869-92A4-F79A-7EF3-0FCE4A954AC1"},
            { "Artes Cênicas Indumentária - Fundão", "81045C0A-92A4-F79C-7BF7-911B399907CD"},
            { "Artes Cênicas: Direção Teatral Noturno - Praia Vermelha", "268A5846-E2FF-41D8-98AF-E8F76E315F8B"},
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
            { "Bacharelado em Psicologia - Praia Vermelha", "29524E6E-CE75-4743-888A-F5BEFE93E191"},
            { "Bacharelado em Química Noturno - Macaé", "FC0DC803-92A4-F799-167B-C91453328264"},
            { "Biblioteconomia e Gestão de Unidade Informação Integral - Praia Vermelha", "0333BD69-92A4-F799-18C3-2A5D2C787AD9"},
            { "Biblioteconomia e Gestão de Unidade Informação Noturno - Fundão", "03DFF3F9-92A4-F79C-1C11-651ED2F4C995"},
            { "Ciência da Computação - Fundão", "FB170486-92A4-F79C-13B5-B562AF729978"},
            { "Ciências Atuariais - Fundão", "8291DE00-92A4-F716-0036-96D0129B07AE" },
            { "Ciências Biológicas - Macaré", "2658AB28-92A4-F79C-019D-17982F25B6DD"},
            { "Ciências Biológicas Biofísica - Fundão", "B3CB2FDA-92A4-F79C-0323-49D8AE8A5526"},
            { "Ciências Biológicas Biofísica Ênfase: Biofísica Ambiental - Fundão", "B402202B-92A4-F79C-0323-49D85BDCD8E8"},
            { "Ciências Biológicas Biofísica Ênfase: Biofísica Molecular Bioinformática - Fundão", "B411C01F-92A4-F79C-0323-49D819CE78C7"},
            { "Ciências Biológicas Biofísica Ênfase: Biofísica Sistemas Biotecnológicos - Fundão", "B4AAA402-92A4-F79C-0323-49D80A9B9A04"},
            { "Ciências Biológicas Biofísica Ênfase: Bioinformática - Fundão", "627202AA-92A4-F79C-1E7E-23D1910B6192"},
            { "Ciências Biológicas Biofísica Ênfase: Biologia Estrutural - Fundão", "621FE733-92A4-F79F-6807-85F2EB01146C"},
            { "Ciências Biológicas Biofísica Ênfase: Biologia de Sistemas - Fundão", "627C4409-92A4-F79C-1E7E-23D135A4090D"},
            { "Ciências Biológicas Biofísica Ênfase: Biologia de Sistemas - Polo de Xerém", "9A6DE585-92A4-F79C-1B6D-997950B59887"},
            { "Ciências Biológicas Biofísica Ênfase: Biologia estrutural - Polo de Xerém", "9A7016CB-92A4-F79C-1B6D-9979E9532242"},
            { "Ciências Biológicas Biofísica Ênfase: Biotecnologia - Fundão", "7DF2282E-92A4-F713-01DF-BC7F88D86AD8"},
            { "Ciências Biológicas Biofísica Ênfase: Biotecnologia - Polo de Xerém", "9A71F22D-92A4-F79C-1B6D-9979CF2C4C78"},
            { "Ciências Biológicas Biofísica Ênfase: Toxicologia Ambiental - Fundão", "627A070E-92A4-F79C-1E7E-23D19F3C5120"},
            { "Ciências Biológicas Biofísica Ênfase: Toxicologia Ambiental - Polo de Xerém", "9A74373B-92A4-F79C-1B6D-9979FA50FC46"},
            { "Ciências Biológicas Bioinformática Ênfase: Biotecnologia - Polo de Xerém", "9A6B9AC3-92A4-F79C-1B6D-99798A098F87"},
            { "Ciências Biológicas Biologia Marinha - Fundão", "13491F7F-92A4-F79B-7AE6-B24F9246803C"},
            { "Ciências Biológicas Biologia Vegetal - Fundão", "5B79C9FD-92A4-F799-1E95-6567BBC64689"},
            { "Ciências Biológicas Biotecnologia - Polo de Xerém", "56EB312E-92A4-F799-269F-4025B7AD1D09"},
            { "Ciências Biológicas Básico - Fundão", "12622FFE-92A4-F79B-7AE6-B24FDBB909CC"},
            { "Ciências Biológicas Ecologia - Fundão", "12A64E2B-92A4-F79B-7AE6-B24F92ECCC7E"},
            { "Ciências Biológicas Genética - Fundão", "1780842F-92A4-F79A-2835-61B96D248CE9"},
            { "Ciências Biológicas Microbiologia e Imunologia - Fundão", "7B0FB779-92A4-F79C-7BF7-911B04551099"},
            { "Ciências Biológicas Modalidade Médica - Fundão", "5AB7CB7E-92A4-F79F-08DD-FA31B0D8749A"},
            { "Ciências Biológicas Modalidade Médica Ênfase: Análises Clínicas - Fundão", "F9B23319-92A4-F79C-087D-4CB2E506E00D"},
            { "Ciências Biológicas Modalidade Médica Ênfase: Biociência Legal - Fundão", "F9B4289E-92A4-F79C-087D-4CB2AD6F0F81"},
            { "Ciências Biológicas Modalidade Médica Ênfase: Ciência e Tecnologia - Fundão", "F9B9A587-92A4-F79C-087D-4CB2608F6843"},
            { "Ciências Biológicas Zoologia - Fundão", "5A9DE4AC-92A4-F79C-0833-4CEE41834AE1"},
            { "Ciências Biológicas Ênfase: Biotecnologia - Macaé", "266FB367-92A4-F79C-019D-1798ACEDE5A5"},
            { "Ciências Biológicas Ênfase: Meio Ambiente - Macaé", "266872CA-92A4-F79C-019D-17988BD7F679"},
            { "Ciências Biológicas: Biofísica - Polo de Xerém", "99ED6AEE-92A4-F79C-1B6D-9979D427A7EB"},
            { "Ciências Contábeis Noturno - Fundão", "16F3B26A-92A4-F79D-3FE1-7EF5D2B16FC1"},
            { "Ciências Contábeis Noturno - Praia Vermelha", "680C676F-92A4-F799-25C3-0F7EE98FBFC5"},
            { "Ciências Contábeis Ênfase: Contábeis Financeira - Fundão", "24C40DED-92A4-F799-2737-5710881821F6"},
            { "Ciências Contábeis Ênfase: Contábeis Financeira - Praia Vermelha", "F7034E0B-92A4-F799-3F3E-B56C9318DDD5"},
            { "Ciências Contábeis Ênfase: Contábeis Gestão Negócios - Fundão", "24B6D187-92A4-F799-2737-5710B38FCABE"},
            { "Ciências Contábeis Ênfase: Contábeis Gestão Negócios - Praia Vermelha", "F6FD47AA-92A4-F799-3F3E-B56C0ABE90CD"},
            { "Ciências Econômicas Integral - Praia Vermelha", "F5B59D5F-92A4-F79D-7004-C040610D7AD2"},
            { "Ciências Econômicas Noturno - Praia Vermelha", "BCD51358-92A4-F79D-6220-85B6B5FC13D9"},
            { "Ciências Sociais - Praia Vermelha", "226EF236-92A4-F79B-0232-AE98803D22DF"},
            { "Composição Paisagística - Fundão", "378DAA4D-92A4-F799-136A-F14DDFB299B5"},
            { "Composição de Interior - Fundão", "A24C0BBB-D6D0-42C1-9966-6A80AB66EEEF"},
            { "Comunicação Social: Básico Integral - Praia Vermelha", "37DE066C-92A4-F799-36A0-A46C504B5D24"},
            { "Comunicação Social: Básico Noturno - Praia Vermelha", "4D3C747C-92A4-F79C-1099-99F11BA80F19"},
            { "Comunicação Social: Jornalismo - Praia Vermelha", "51D24AE9-92A4-F799-31BD-13B6ED6B6D3F"},
            { "Comunicação Social: Produção Editorial - Praia Vermelha", "51B87CDD-92A4-F799-31BD-13B6F2CD257B"},
            { "Comunicação Social: Publicidade e Propaganda - Praia Vermelha", "4D7C67C8-92A4-F799-31BD-13B6606F700E"},
            { "Comunicação Social: Radialismo Integral - Praia Vermelha", "55B9A131-DC47-42A9-9EE4-FA8A254C7BA2"},
            { "Comunicação Social: Radialismo Noturno - Praia Vermelha", "4D704101-92A4-F79C-1099-99F135270E63"},
            { "Comunicação Visual Design - Fundão", "BED7B95D-92A4-F79D-77AC-F1C034C21FFF"},
            { "Conservação e Restauração - Fundão", "E763726D-92A4-F799-4F11-241E18418366"},
            { "Dança Integral - Fundão", "172652D9-BAA2-49B9-A2DE-008C51728698"},
            { "Dança Noturno - Fundão", "28A422AB-92A4-F79A-0A97-8D36D1EEA070"},
            { "Defesa e Gestão Estratégica Internacional Noturno - Fundão", "BA645FDA-92A4-F79C-7203-E3B5DB668130"},
            { "Desenho Industrial Projeto do Produto - Fundão", "B81433DC-2DE2-4C8F-A411-0F4C2B388AC5"},
            { "Direito Integral - Faculdade de Direito", "A38E0507-92A4-F79B-1703-64F2F1BC8943"},
            { "Direito Noturno - Faculdade de Direito", "A3905265-92A4-F79B-1703-64F2CC271B41"},
            { "Educação Fìsica Integral - Fundão", "7C525F7D-92A4-F7A0-66EC-6406F3DA6C5F"},
            { "Educação Fìsica Noturno - Fundão", "026EECB6-92A4-F79C-77D2-1A9BA3202B44"},
            { "Enfermagem - Fundão", "48B020A7-92A4-F79C-3B74-80C52F3D2444"},
            { "Enfermagem e Obstetrícia - Macaé", "CD4E3A4F-92A4-F79A-06D1-EF033E819101"},
            { "Engenharia - Macaé", "647BE4D0-92A4-F79A-617E-32C7B80EDEFC"},
            { "Engenharia Ambiental - Fundão ", "C32301EF-92A4-FAFE-00E7-DF90826388FA"},
            { "Engenharia Civil - Fundão", "EB1804EA-92A4-F79D-5F51-DA5503AB533D"},
            { "Engenharia Civil - Macaé", "86BAB1F4-92A4-F79D-45D8-342910413991"},
            { "Engenharia Eletrônica e de Computação - Fundão", "AD403453-92A4-F799-108D-AD9C754FD6A7"},
            { "Engenharia Elétrica - Fundão", "F5F60FF8-92A4-F79C-2256-5EE81016A543"},
            { "Engenharia Matemática - Fundão", "14BA7CE6-92A4-F799-1F31-15AB6442F8A8"},
            { "Engenharia Mecânica - Fundão", "C4C63E6F-92A4-F79B-5BD7-89E4D53BE14A"},
            { "Engenharia Mecânica - Macaé", "86BE612F-92A4-F79D-45D8-3429EC87C0B5"},
            { "Engenharia Metalúrgica - Fundão", "3E231718-92A4-F79B-1A27-5A2E5FF58D69"},
            { "Engenharia Naval e Oceânica - Fundão", "D431D60F-92A4-F79D-78B9-9F12584902F9"},
            { "Engenharia Nuclear - Fundão", "BFAED97A-92A4-F79C-30D1-28262E56DB1B"},
            { "Engenharia Núcleo Comum - Macaé", "80BD4FF7-92A4-F79B-0ED7-52C392FA5F44"},
            { "Engenharia Química Integral - Fundão", "158DBD47-92A4-F799-0F21-2168C111D3CC"},
            { "Engenharia Química Noturno - Fundão", "106A11A1-92A4-F79C-430F-766F2001DB88"},
            { "Engenharia de Alimentos - Fundão", "9F4BA9F7-92A4-FB5F-002A-C92FECF233B9"},
            { "Engenharia de Bioprocessos - Fundão", "9F4C900B-92A4-FB5F-002A-C92F15792CA9"},
            { "Engenharia de Computação e Informação - Fundão", "A46B41C8-92A4-F79C-7383-BE627D84214E" },
            { "Engenharia de Controle e Automação - Fundão", "DA307421-92A4-F799-6747-B5C24C9E41F2"},
            { "Engenharia de Materiais - Fundão", "3E100A5A-92A4-F79C-051F-6C994AA25736"},
            { "Engenharia de Petróleo - Fundão", "24C6A6A4-92A4-F79C-4038-E6D8B161E716"},
            { "Engenharia de Produção - Fundão", "333C3B46-92A4-F799-3D93-3783890745AC"},
            { "Engenharia de Produção - Macaé", "86BD6BAE-92A4-F79D-45D8-34291F6C353F"},
            { "Estatística - Fundão", "243658D9-92A4-F79A-27F6-39968D2F8BF5" },
            { "Farmácia Integral - Fundão", "3D93C4E2-92A4-F79A-1828-B8269DED73D1"},
            { "Farmácia Integral - Macaé", "D8D58E48-92A4-F79C-0323-49D84014AF3D"},
            { "Farmácia Noturno - Fundão", "C03E5428-92A4-F799-241D-5EFC1FA9D1E7"},
            { "Farmácia Noturno - Macaé", "CD3AC890-92A4-F79A-06D1-EF03782CE81D"},
            { "Fisioterapia - Fundão", "FE6A5A16-92A4-F799-0D3F-4809B3D52912"},
            { "Fonoaudiologia - Fundão", "BC2EBF38-B0BE-426C-BCF5-E0E61440193D"},
            { "Física - Fundão", "1BB12920-92A4-F799-32E7-93CEA3D09843"},
            { "Física Médica - Fundão", "6684EB75-92A4-F799-6FA2-7E7CBE506055"},
            { "Gastronomia - Fundão", "20C9AA01-92A4-F79D-6657-968B550FCC64"},
            { "Geografia Integral - Fundão", "BB18B459-EFDB-453D-B1C2-37C5C15C713D"},
            { "Geografia Noturno - Fundão", "D7C872F2-9965-4279-A6D4-BA9CFB37C8C4"},
            { "Geologia - Fundão", "654C42EA-92A4-F79C-39AE-9171EC4971ED"},
            { "Gestão Pública Desenvolvimento Econômico e Social Tarde e Noite - Fundão", "21FA5564-92A4-F79A-72F1-D02E435E1CB9"},
            { "Gestão Pública Desenvolvimento Econômico e Social Ênfase: Gestão do Setor Público Tarde e Noite - Fundão", "26E4539E-92A4-F799-12EA-DA5DC7E23ADC"},
            { "Gestão Pública Desenvolvimento Econômico e Social Ênfase: Gestão do Terceiro Setor Tarde e Noite - Fundão", "270E9624-92A4-F799-12EA-DA5D4B69BE71"},
            { "Gravura - Fundão", "523B2D0D-453C-4EEF-8710-A3DA7CB3C9E6"},
            { "História Integral - Praia Vermelha", "0B764D67-92A4-F799-6879-596F0AB056E2"},
            { "História Noturno - Praia Vermelha", "D7E158AB-92A4-F79B-1558-0D4C3AF00EB7"},
            { "História da Arte - Fundão", "B3EAFFE1-92A4-F799-77BD-9CA99B5C020E"},
            { "Jornalismo - Praia Vermelha", "88F04D12-92A4-F79C-19B3-A8DB13A5E152"},
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
            { "Licenciatura em Ciências Biológicas - Macaé", "96BD6C42-92A4-F79C-58A6-06E339330FCC"},
            { "Licenciatura em Ciências Biológicas Integral - Fundão", "12693EEA-92A4-F79B-7AE6-B24FCD32B0C8"},
            { "Licenciatura em Ciências Biológicas Noturno - Fundão", "126B89F3-92A4-F79B-7AE6-B24F8469B966"},
            { "Licenciatura em Ciências Sociais - Praia Vermelha", "5B480A69-92A4-F79E-25D2-5F8DAB4BE6E7"},
            { "Licenciatura em Ciências Sociais Noturno - Praia Vermelha", "484CF394-92A4-F79B-0CBD-E4AC53B31C43"},
            { "Licenciatura em Dança Noturno - Fundão", "290B9431-92A4-F79A-0A97-8D363CDE9275"},
            { "Licenciatura em Educação Artística Artes Plásticas - Fundão", "8FE2D2CC-C04A-4274-8DF3-45102CDFA32F"},
            { "Licenciatura em Educação Artística Desenho - Fundão", "85724BFD-8D2A-48BF-86AA-5749D7EE5759"},
            { "Licenciatura em Educação Física Integral - Fundão", "FD563FE6-92A4-F799-3D56-CCE6543B7571"},
            { "Licenciatura em Filosofia - Praia Vermelha", "4D5253F6-92A4-F715-00B6-520B96B05B4E"},
            { "Licenciatura em Filosofia - Praia Vermelha", "8044A933-92A4-F79A-1770-0237BFFE4CF2"},
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
            { "Licenciatura em Psicologia - Praia Vermelha", "7AAC26D1-A142-4F63-B9BC-B7781D7C5A8A"},
            { "Licenciatura em Química Integral - Fundão", "F377ECA8-8C1B-4FE1-9F62-93989955E8C6"},
            { "Licenciatura em Química Noturno - Fundão", "B0B38E9A-92A4-F79A-5843-0CB23B379573"},
            { "Licenciatura em Química Noturno - Macaé", "F3AE2615-92A4-F79C-1E33-80C5B2783367"},
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
            { "Nanotecnologia - Fundão", "858FF1AC-92A4-F79C-2205-49D45A7754CB"},
            { "Nanotecnologia - Polo de Xerém", "33565B05-92A4-F79C-7F8B-F72317607C83"},
            { "Nanotecnologia Ênfase: Bionanotecnologia - Fundão", "A48E50A3-92A4-F79C-4FD2-AC36159A0056"},
            { "Nanotecnologia Ênfase: Bionanotecnologia - Polo de Xerém", "337137C4-92A4-F79C-7F8B-F723FD08DD0D"},
            { "Nanotecnologia Ênfase: Física - Fundão", "9A1BC3B8-92A4-F79B-30C7-AC0ACAE3DBEA"},
            { "Nanotecnologia Ênfase: Física - Polo de Xerém", "3390A222-92A4-F79C-7F8B-F723F862EF0E"},
            { "Nanotecnologia Ênfase: Materiais - Fundão", "A4624591-92A4-F79C-4FD2-AC367898A591"},
            { "Nutrição - Fundão", "5D60818B-2D21-4F8A-82EE-5C56321CCAFA"},
            { "Nutrição - Macaé", "FCE436FD-92A4-F79C-2107-2F13E9B10AF6"},
            { "Odontologia - Fundão", "4F3A17FE-3D9B-4CD3-B3F0-E819522A2E9E"},
            { "Paisagismo - Fundão", "AF6F875F-92A4-F79C-7203-E3B50910CAF4"},
            { "Pedadogia Vespertino - Praia Vermelha", "8C6C438C-92A4-F79A-553D-FD57A3FE308C"},
            { "Pedagogia Matutino - Praia Vermelha", "8C6E973C-92A4-F79A-553D-FD5710EC50DC"},
            { "Pedagogia Noturno - Praia Vermelha", "8C523598-92A4-F79A-553D-FD57B5965FB8"},
            { "Pintura - Fundão", "14373839-92A4-F79C-1CA2-4FE7964814AF"},
            { "Psicologia - Praia Vermelha", "832E2E20-92A4-F799-466E-6CB70777042"},
            { "Química - Atribuições Tecnológicas - Fundão", "2D75E4B0-92A4-F79C-6405-62485F9051FB"},
            { "Química Industrial Integral - Fundão", "20120240-92A4-F799-0F21-21684A448794"},
            { "Química Industrial Noturno - Fundão", "1FDD32EC-92A4-F79C-430F-766F717D814E"},
            { "Química Noturno - Fundão", "F1E64377-92A4-F79C-2576-734D37CC92A3"},
            { "Relações Internacionais Noturno - Fundão", "D3B0C7C5-92A4-F799-5FBF-C2EBC5C714BC"},
            { "Saúde Coletiva - Fundão", "190569CF-92A4-F799-54BE-04529D312DA2"},
            { "Serviço Social Integral - Fundão", "D7E0A446-92A4-F79C-0323-49D8A03CC72A"},
            { "Serviço Social Integral - Praia Vermelha", "D7E0A446-92A4-F79C-0323-49D8A03CC72A"},
            { "Serviço Social Noturno - Fundão", "D7E142B7-92A4-F79C-0323-49D8600587D2"},
            { "Serviço Social Noturno - Praia Vermelha", "D7E142B7-92A4-F79C-0323-49D8600587D2"},
            { "Teoria da Dança Noturno - Fundão", "28EBD161-92A4-F79A-0A97-8D3689661E61"},
            { "Terapia Ocupacional - Fundão", "FEA0A477-92A4-F799-54BC-9CB57821A4F0"},
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