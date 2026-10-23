using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

// RRHH_FuncFam se lee de la vista vw_RRHH_FuncFam (una fila por empleado con las 9 respuestas)
// y se guarda como filas de RRHH_FuncFamRespuesta, una por pregunta.
public class RRHH_FuncFamData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_FuncFamData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_FuncFam>> Listar() => await _contexto.Set<RRHH_FuncFam>().AsNoTracking().ToListAsync();

    public async Task<RRHH_FuncFam?> Obtener(int IdEmpleadoParam)
    {
        return await _contexto.Set<RRHH_FuncFam>().AsNoTracking().FirstOrDefaultAsync(f => f.IdEmpleado == IdEmpleadoParam);
    }

    public async Task Crear(RRHH_FuncFam registro) => await GuardarRespuestas(registro);

    public async Task Actualizar(RRHH_FuncFam registro) => await GuardarRespuestas(registro);

    public async Task Eliminar(RRHH_FuncFam registro)
    {
        var respuestas = _contexto.Set<RRHH_FuncFamRespuesta>().Where(r => r.IdEmpleado == registro.IdEmpleado);
        _contexto.Set<RRHH_FuncFamRespuesta>().RemoveRange(await respuestas.ToListAsync());
        await _contexto.SaveChangesAsync();
    }

    private async Task GuardarRespuestas(RRHH_FuncFam registro)
    {
        var valores = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            ["Com1"] = registro.Com1, ["Com2"] = registro.Com2,
            ["Afec1"] = registro.Afec1, ["Afec2"] = registro.Afec2,
            ["Decis1"] = registro.Decis1, ["Decis2"] = registro.Decis2,
            ["Soc1"] = registro.Soc1, ["Soc2"] = registro.Soc2, ["Soc3"] = registro.Soc3
        };
        var preguntas = await _contexto.Set<RRHH_FuncFamPregunta>().AsNoTracking().ToListAsync();
        var actuales = await _contexto.Set<RRHH_FuncFamRespuesta>()
            .Where(r => r.IdEmpleado == registro.IdEmpleado).ToListAsync();

        foreach (var pregunta in preguntas)
        {
            var valor = valores.GetValueOrDefault(pregunta.Codigo)?.Trim();
            var actual = actuales.FirstOrDefault(r => r.IdPregunta == pregunta.IdPregunta);
            if (string.IsNullOrEmpty(valor))
            {
                if (actual is not null)
                    _contexto.Remove(actual);
            }
            else if (actual is null)
            {
                _contexto.Add(new RRHH_FuncFamRespuesta { IdEmpleado = registro.IdEmpleado, IdPregunta = pregunta.IdPregunta, Respuesta = valor });
            }
            else
            {
                actual.Respuesta = valor;
            }
        }
        await _contexto.SaveChangesAsync();
    }
}
