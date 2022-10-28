using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;



public class TipoProductoAppService : ITipoProductoAppService
{
    private readonly ITipoProductoRepository repository;
    //private readonly IUnitOfWork unitOfWork;

    public TipoProductoAppService(ITipoProductoRepository repository)
    {
        this.repository = repository;
        //this.unitOfWork = unitOfWork;
    }

    public async Task<TipoProductoDto> CreateAsync(TipoProductoCrearActualizarDto tipoProductoDto)
    {
        
        //Reglas Validaciones... 
        var existeNombreMarca = await repository.ExisteNombre(tipoProductoDto.Nombre);
        if (existeNombreMarca){
            throw new ArgumentException($"Ya existe una marca con el nombre {tipoProductoDto.Nombre}");
        }
 
        //Mapeo Dto => Entidad
        var tipoProducto = new TipoProducto();
        tipoProducto.Nombre = tipoProductoDto.Nombre;
 
        //Persistencia objeto
        tipoProducto = await repository.AddAsync(tipoProducto);
        //await unitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        var tipoProductoCreada = new TipoProductoDto();
        tipoProductoCreada.Nombre = tipoProducto.Nombre;
        tipoProductoCreada.Id = tipoProducto.Id;

        //TODO: Enviar un correo electronica... 

        return tipoProductoCreada;
    }

    public async Task UpdateAsync(int id, TipoProductoCrearActualizarDto tipoProductoDto)
    {
        var tipoProducto = await repository.GetByIdAsync(id);
        if (tipoProducto == null){
            throw new ArgumentException($"La marca con el id: {id}, no existe");
        }
        
        var existeNombreTipoProducto = await repository.ExisteNombre(tipoProductoDto.Nombre,id);
        if (existeNombreTipoProducto){
            throw new ArgumentException($"Ya existe una marca con el nombre {tipoProductoDto.Nombre}");
        }

        //Mapeo Dto => Entidad
        tipoProducto.Nombre = tipoProductoDto.Nombre;

        //Persistencia objeto
        await repository.UpdateAsync(tipoProducto);
        //await unitOfWork.SaveChangesAsync();

        return;
    }

    public async Task<bool> DeleteAsync(int tipoProductoId)
    {
        //Reglas Validaciones... 
        var tipoProducto = await repository.GetByIdAsync(tipoProductoId);
        if (tipoProducto == null){
            throw new ArgumentException($"La marca con el id: {tipoProductoId}, no existe");
        }

        repository.Delete(tipoProducto);
        //await unitOfWork.SaveChangesAsync();

        return true;
    }

    public ICollection<TipoProductoDto> GetAll()
    {
        var tipoProductoList = repository.GetAll();

        var tipoProductoListDto =  from tp in tipoProductoList
                            select new TipoProductoDto(){
                                Id = tp.Id,
                                Nombre = tp.Nombre
                            };

        return tipoProductoListDto.ToList();
    }

    
}
 