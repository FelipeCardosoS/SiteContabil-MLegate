using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ColunaCabecalho
/// </summary>

namespace ImportacaoInteligente
{
    [Serializable]
    public class ColunaCabecalho
    {
        private int _posicao;
        private string _cabecalho;
        private string _referencia;
        private TipoColunaAbstract _tipo;

        public int posicao
        {
            get { return _posicao; }
            set { _posicao = value; }
        }

        public string cabecalho
        {
            get { return _cabecalho; }
            set { _cabecalho = value; }
        }

        public string referencia
        {
            get { return _referencia; }
            set { _referencia = value; }
        }

        public TipoColunaAbstract tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }


        public ColunaCabecalho(int posicao, string cabecalho, string referencia)
        {
            _posicao = posicao;
            _cabecalho = cabecalho;
            _referencia = referencia;
        }
    }
}