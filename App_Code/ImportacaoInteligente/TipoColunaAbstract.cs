using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TipoColunaAbstract
/// </summary>
/// 
namespace ImportacaoInteligente
{
    [Serializable]
    public abstract class TipoColunaAbstract : TipoColuna
    {
        private string _value;

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }

        public virtual string concatenador
        {
            get { return "'"; }
        }

        public virtual object nulo
        {
            get { return ""; }
        }

        public abstract bool valida();

        public abstract void limpa();
    }
}