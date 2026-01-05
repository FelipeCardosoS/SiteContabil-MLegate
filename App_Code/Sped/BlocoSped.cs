using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BlocoSped
/// </summary>
/// 
namespace Sped
{
    public class BlocoSped
    {
        private string _nome;
        private int _totalLinhas;

        public string nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public int totalLinhas
        {
            get { return _totalLinhas; }
            set { _totalLinhas = value; }
        }

        public BlocoSped(string nome, int totalLinhas)
        {
            _nome = nome;
            _totalLinhas = totalLinhas;
        }
    }
}