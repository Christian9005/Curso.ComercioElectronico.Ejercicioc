using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;


public interface ITipoProductoAppService
{

    ICollection<TipoProductoDto> GetAll();

    Task<TipoProductoDto> CreateAsync(TipoProductoCrearActualizarDto marca);

    Task UpdateAsync (int id, TipoProductoCrearActualizarDto marca);

    Task<bool> DeleteAsync(int marcaId);
}
 
 