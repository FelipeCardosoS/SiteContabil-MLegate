using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Campos
/// </summary>
/// 
namespace ImportacaoInteligente
{
    public class Campos
    {
        private string _nome;
        private bool _obrigatorio;
        private List<string> _destino;
        private bool _chave;
        private string _campoDb;

        public string nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public bool obrigatorio
        {
            get { return _obrigatorio; }
            set { _obrigatorio = value; }
        }

        public List<string> destino
        {
            get { return _destino; }
            set { _destino = value; }
        }

        public bool chave
        {
            get { return _chave; }
            set { _chave = value; }
        }

        public string campoDb
        {
            get { return _campoDb; }
            set { _campoDb = value; }
        }

        public Campos(string nome, bool obrigatorio)
        {
            _nome = nome;
            _obrigatorio = obrigatorio;
        }

        public Campos(string nome, bool obrigatorio, List<string> destino, bool chave)
            : this(nome, obrigatorio)
        {
            _destino = destino;
            _chave = chave;
        }

        public Campos(string nome, bool obrigatorio, List<string> destino, bool chave, string campoDb)
            : this(nome, obrigatorio, destino, chave)
        {
            _campoDb = campoDb;
        }
    }
}
