using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
/// <summary>
/// Summary description for ImportacaoInteligente
/// </summary>
/// 
namespace ImportacaoInteligente
{
    public class Importacao
    {
        private string _nomeArquivo;
        private List<ColunaCabecalho> _cabecalhos = new List<ColunaCabecalho>();
        private List<string> _criticas = new List<string>();
        private List<string> _consultas = new List<string>();
        private bool _removerAtuais;
        private static string diretorioTemporario = "Imports/";
        private static string[] contentTypes = new string[] { "text/x-csv", "application/vnd.ms-excel" };
        private string _stringConexao;
        private List<string> _tabelas;
        private Conexao conexao;

        public List<string> criticas
        {
            get { return _criticas; }
            set { _criticas = value; }
        }

        public List<string> consultas
        {
            get { return _consultas; }
            set { _consultas = value; }
        }

        public bool removerAtuais
        {
            get { return _removerAtuais; }
            set { _removerAtuais = value; }
        }

        public static List<Campos> campos = new List<Campos>(new Campos[]{ 
            new Campos("NOME", true, new List<string>(new string[] {"CAD_EMPRESA"}), false, "NOME_RAZAO_SOCIAL"),
            new Campos("RAZAO_SOCIAL", true, new List<string>(new string[] {"CAD_EMPRESA"}), false, "NOME_RAZAO_SOCIAL"),
            new Campos("TIPO", true, new List<string>(new string[] {"CAD_EMPRESA"}), true, "TIPO"),
            new Campos("CPF", true, new List<string>(new string[] {"CAD_EMPRESA"}), true, "CPF"),
            new Campos("CNPJ", true, new List<string>(new string[] {"CAD_EMPRESA"}), true, "CNPJ")
        });

        public List<ColunaCabecalho> cabecalhos
        {
            get { return _cabecalhos; }
            set { _cabecalhos = value; }
        }

        public string nomeArquivo
        {
            get { return _nomeArquivo; }
            set { _nomeArquivo = value; }
        }

        public Importacao(string stringConexao, List<string> tabelas)
        {
            _stringConexao = stringConexao;
            _tabelas = tabelas;
        }

        public bool uploadArquivo(HttpPostedFile post)
        {
            bool status = false;
            if (post.ContentLength == 0)
                throw new Exception("Arquivo está vazio ou corrompido.");

            bool existeTipo = false;
            for (int i = 0; i < Importacao.contentTypes.Length; i++)
            {
                if (post.ContentType == Importacao.contentTypes[i])
                {
                    existeTipo = true;
                    break;
                }
            }

            if (!existeTipo)
                throw new Exception("Tipo de arquivo inválido.");

            nomeArquivo = DateTime.Now.ToString("ddMMyyyyHms") + Path.GetExtension(post.FileName);
            DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(Importacao.diretorioTemporario));
            if (!dir.Exists)
                dir.Create();

            post.SaveAs(System.Web.HttpContext.Current.Server.MapPath(Importacao.diretorioTemporario + nomeArquivo));
            status = true;

            return status;
        }

        public void pegaColunas()
        {
            using (StreamReader leitor = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(Importacao.diretorioTemporario + nomeArquivo), System.Text.Encoding.Default))
            {
                while (!leitor.EndOfStream)
                {
                    string linha = leitor.ReadLine();
                    string[] colunas = linha.Split(';');
                    _cabecalhos = new List<ColunaCabecalho>();
                    for (int i = 0; i < colunas.Length; i++)
                    {
                        _cabecalhos.Add(new ColunaCabecalho(i, "", colunas[i]));
                    }
                    break;
                }
            }
        }

        public bool analisaDocumento()
        {
            _criticas = new List<string>();
            _consultas = new List<string>();

            var query = (from cab in _cabecalhos
                         from campos in Importacao.campos
                         where cab.cabecalho == campos.nome
                         && campos.obrigatorio == true
                         select campos).ToList();
            var obrigatorios = (from campos in Importacao.campos where campos.obrigatorio == true select campos).ToList();

            if (query.Count != obrigatorios.Count)
            {
                _criticas.Add("Você não escolheu todos os campos que são obrigatórios, por favor verifique.");
            }

            if (_criticas.Count == 0)
            {
                for (int i = 0; i < _cabecalhos.Count; i++)
                {
                    if (_cabecalhos[i].cabecalho != "0" && !string.IsNullOrEmpty(_cabecalhos[i].cabecalho))
                    {
                        _cabecalhos[i].tipo = TipoFactory.Instancia.monta(_cabecalhos[i].cabecalho);
                    }
                }

                //using (StreamReader leitor = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(ImportacaoInteligente.diretorioTemporario + nomeArquivo), System.Text.Encoding.Default))
                using (StreamReader leitor = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("c:" + nomeArquivo), System.Text.Encoding.Default))
                {
                    int contador = 1;
                    while (!leitor.EndOfStream)
                    {
                        string linha = leitor.ReadLine();
                        string[] colunas = linha.Split(';');

                        analisaLinha(colunas, contador);

                        contador++;
                    }
                }
            }

            return (_criticas.Count == 0);
        }

        public bool salvar()
        {
            int erros = 0;
            List<Exception> exceptions = new List<Exception>();

            conexao = new Conexao(_stringConexao);
            for (int i = 0; i < _consultas.Count; i++)
            {
                try
                {
                    conexao.execute(_consultas[i]);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    erros++;
                }
            }

            return (erros > 0);
        }

        private bool verificaExistencia(string destino, string[] linha)
        {
            string sql = "select isnull(count(*),0) from " + destino + " where ";
            for (int i = 0; i < _cabecalhos.Count; i++)
            {
                if (_cabecalhos[i].cabecalho != "0" && !string.IsNullOrEmpty(_cabecalhos[i].cabecalho))
                {
                    List<string> destinos = destinosColuna(_cabecalhos[i].cabecalho);
                    cabecalhos[i].tipo.value = linha[cabecalhos[i].posicao];
                    sql += campoDb(_cabecalhos[i].cabecalho) + " = " + _cabecalhos[i].tipo.concatenador + _cabecalhos[i].tipo.ToString() + _cabecalhos[i].tipo.concatenador;
                }
            }

            return (Convert.ToInt32(conexao.scalar(sql)) > 0);
        }

        private bool analisaLinha(string[] linha, int contador)
        {
            bool perfeito = true;
            for (int i = 0; i < _cabecalhos.Count; i++)
            {
                if (_cabecalhos[i].cabecalho != "0" && !string.IsNullOrEmpty(_cabecalhos[i].cabecalho))
                {
                    _cabecalhos[i].tipo.value = linha[_cabecalhos[i].posicao];
                    if (!_cabecalhos[i].tipo.valida())
                    {
                        if (verificaObrigacao(_cabecalhos[i].cabecalho))
                        {
                            _criticas.Add("Linha " + contador + ": O valor " + linha[_cabecalhos[i].posicao] + " é incompatível com a coluna " + _cabecalhos[i].cabecalho);
                            perfeito = false;
                            break;
                        }
                    }
                }
            }

            if (perfeito)
            {
                
                for (int i = 0; i < _tabelas.Count; i++)
                {
                    bool insert = true;
                    if (!removerAtuais)
                    {
                        if (verificaExistencia(_tabelas[i], linha))
                        {
                            insert = false;
                        }
                    }
                    _consultas.Add(montaConsulta(linha, _tabelas[i], insert));
                }
            }

            return perfeito;
        }

        private string montaConsulta(string[] linha, string destino, bool insert)
        {
            string sql = "";
            string colunas = "";
            string valores = "";
            string where = "";

            for (int i = 0; i < _tabelas.Count; i++)
            {
                if (insert)
                {
                    sql = "INSERT INTO " + _tabelas[i] + "(";
                }
                else
                {

                }
            }

            for (int i = 0; i < _cabecalhos.Count; i++)
            {
                if (_cabecalhos[i].cabecalho != "0" && !string.IsNullOrEmpty(_cabecalhos[i].cabecalho))
                {
                    bool faz = true;
                    if (faz)
                    {
                        List<string> destinos = destinosColuna(_cabecalhos[i].cabecalho);
                        if (destinos.Contains(destino))
                        {
                            if (insert)
                            {
                                cabecalhos[i].tipo.value = linha[cabecalhos[i].posicao];
                                colunas += (colunas.Length == 0 ? "" : ",") + campoDb(_cabecalhos[i].cabecalho);
                                valores += (valores.Length == 0 ? "" : ",") + _cabecalhos[i].tipo.concatenador + _cabecalhos[i].tipo.ToString() + _cabecalhos[i].tipo.concatenador;
                            }
                            else
                            {
                                cabecalhos[i].tipo.value = linha[cabecalhos[i].posicao];
                                valores += (valores.Length == 0 ? "" : ",") + campoDb(_cabecalhos[i].cabecalho) + " = " + _cabecalhos[i].tipo.concatenador + _cabecalhos[i].tipo.ToString() + _cabecalhos[i].tipo.concatenador;
                            }
                        }
                    }
                }
            }

            if (insert)
            {
                sql += colunas + ")values(" + valores + ")";
            }
            else
            {
                sql += valores + where;
            }

            return sql;
        }

        private bool verificaObrigacao(string cabecalho)
        {
            for (int i = 0; i < Importacao.campos.Count; i++)
            {
                if (cabecalho == Importacao.campos[i].nome)
                {
                    return Importacao.campos[i].obrigatorio;
                }
            }

            throw new Exception("Cabeçalho não encontrado.");
        }

        private string campoDb(string cabecalho)
        {
            for (int i = 0; i < Importacao.campos.Count; i++)
            {
                if (cabecalho == Importacao.campos[i].nome)
                {
                    return Importacao.campos[i].campoDb;
                }
            }

            throw new Exception("Cabeçalho não encontrado.");
        }

        private bool verificaChave(string cabecalho)
        {
            for (int i = 0; i < Importacao.campos.Count; i++)
            {
                if (cabecalho == Importacao.campos[i].nome)
                {
                    return Importacao.campos[i].chave;
                }
            }

            throw new Exception("Cabeçalho não encontrado.");
        }

        private List<string> destinosColuna(string cabecalho)
        {
            for (int i = 0; i < Importacao.campos.Count; i++)
            {
                if (cabecalho == Importacao.campos[i].nome)
                {
                    return Importacao.campos[i].destino;
                }
            }

            throw new Exception("Cabeçalho não encontrado.");
        }
    }
}