using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TipoFactory
/// </summary>
/// 
namespace ImportacaoInteligente
{
    internal sealed class TipoFactory
    {
        private static volatile TipoFactory _instancia;
        private static object syncRoot = new Object();

        private TipoFactory()
        {
        }

        public static TipoFactory Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (syncRoot)
                    {
                        if (_instancia == null)
                            _instancia = new TipoFactory();
                    }
                }

                return _instancia;
            }
        }

        public TipoColunaAbstract monta(string tipo)
        {
            TipoColunaAbstract dados;
            switch (tipo)
            {
                case "NOME":
                    dados = new TipoTexto();
                    break;
                case "NOME_FANTASIA":
                    dados = new TipoTexto();
                    break;
                case "CNPJ":
                    dados = new TipoTexto();
                    break;
                case "CPF":
                    dados = new ColunaCpf();
                    break;
                case "ENDERECO":
                    dados = new TipoTexto();
                    break;
                case "BAIRRO":
                    dados = new TipoTexto();
                    break;
                case "CEP":
                    dados = new TipoTexto();
                    break;
                case "MUNICIPIO":
                    dados = new TipoTexto();
                    break;
                case "TELEFONE":
                    dados = new TipoTexto();
                    break;
                case "IE":
                    dados = new TipoTexto();
                    break;
                case "RG":
                    dados = new TipoTexto();
                    break;
                case "UF":
                    dados = new TipoTexto();
                    break;

                default:
                    dados = new TipoTexto();
                    break;
            }

            return dados;
        }
    }
}