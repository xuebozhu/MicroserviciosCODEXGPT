# MicroserviciosCODEXGPT

Este repositorio contiene un ejemplo didáctico de microservicios en .NET 8.
Consta de cuatro aplicaciones web que se comunican mediante HTTP:

- **ProductService** expone una API para consultar y crear productos.
- **OrderService** registra pedidos y consulta el servicio de productos para obtener información de cada producto.
- **ShippingService** permite registrar envíos y consulta el servicio de pedidos para mostrar los detalles del pedido enviado.
- **Frontend** ofrece una interfaz web que consume los tres servicios anteriores.

## Ejecución

Para ejecutar cada microservicio utilice los siguientes comandos en terminales
separadas desde la raíz del repositorio:

```bash
# Servicio de productos (puerto 5000)
dotnet run --project ProductService

# Servicio de pedidos (puerto 5001)
dotnet run --project OrderService

# Servicio de envíos (puerto 5002)
dotnet run --project ShippingService

# Frontend (puerto 5003)
dotnet run --project Frontend
```

Una vez iniciados, podrá realizar peticiones HTTP a los endpoints expuestos,
o bien acceder a `http://localhost:5003` para utilizar la interfaz web.
