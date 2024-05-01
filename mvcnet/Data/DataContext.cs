using MySqlConnector;
namespace mvc;

public class DataContext : IDataContext
{
    private readonly MySqlConnection _sqlConnetion;

    public DataContext(MySqlConnection mySqlConnection){
        _sqlConnetion = mySqlConnection;
    }


    public async Task<List<Producto>> ObtenProductosAsync()
    {
        await  _sqlConnetion.OpenAsync();
        List<Producto> productos = new();
        using var command = new MySqlCommand(@"SELECT producto.id, producto.nombre, producto.precio, fabricante.nombre as fa_nombre FROM 
        fabricante INNER JOIN producto ON fabricante.id = producto.id_fabricante", _sqlConnetion);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()){
            Producto item = new(){
                ProductoId = Convert.ToInt32(reader["id"]),
                Nombre = reader["nombre"].ToString(),
                Precio = Convert.ToDecimal(reader["precio"]),
                Fabricante = reader["fa_nombre"].ToString()
            };
            productos.Add(item);
        }
        return productos;
    }
}