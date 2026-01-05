using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TipoImpostoClass
/// </summary>
public class TipoImpostoClass
{
    private tiposImpostoDAO _tipoImpostoDAO = null;
    private aliquotasTipoImpostoDAO _aliquotaDAO = null;
	public TipoImpostoClass(tiposImpostoDAO tipoImpostoDAO, aliquotasTipoImpostoDAO aliquotaDAO)
	{
        _tipoImpostoDAO = tipoImpostoDAO;
        _aliquotaDAO = aliquotaDAO;
	}

    public void novo(STipoImposto tipo, List<SAliquotaImposto> aliquotas)
    {
        _tipoImpostoDAO.insert(tipo);
        for (int i = 0; i < aliquotas.Count; i++)
        {
            aliquotas[i].tipoImposto = tipo.tipoImposto;
            _aliquotaDAO.insert(aliquotas[i]);
        }
    }

    public void editar(STipoImposto tipo, List<SAliquotaImposto> aliquotas)
    {
        _tipoImpostoDAO.update(tipo);
        _aliquotaDAO.delete(tipo.tipoImposto, SessionView.EmpresaSession);

        for (int i = 0; i < aliquotas.Count; i++)
        {
            aliquotas[i].tipoImposto = tipo.tipoImposto;
            _aliquotaDAO.insert(aliquotas[i]);
        }
    }
}