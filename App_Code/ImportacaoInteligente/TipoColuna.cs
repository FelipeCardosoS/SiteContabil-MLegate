using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TipoColuna
/// </summary>
/// 
namespace ImportacaoInteligente
{
    public interface TipoColuna
    {
        bool valida();
        void limpa();
    }
}