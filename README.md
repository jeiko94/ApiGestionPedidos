# ApiGestionPedidos

Este proyecto es una API para la gestión de pedidos, desarrollada en C# con .NET 8.0. Proporciona funcionalidades para crear, obtener, listar, actualizar y eliminar productos.

## Requisitos

- .NET 8.0 SDK
- Visual Studio 2022
- SQL Server (o cualquier base de datos compatible con Entity Framework Core)

## Configuración del Proyecto

1. **Clonar el repositorio**
2. **Configurar la base de datos**

   Actualiza la cadena de conexión en el archivo `appsettings.json` con los detalles de tu base de datos.
3. **Aplicar las migraciones**

   Ejecuta los siguientes comandos en la consola del Administrador de paquetes de Visual Studio o en la terminal:
4. **Ejecutar el proyecto**

   Abre el proyecto en Visual Studio 2022 y presiona `F5` para ejecutar la aplicación.

## Endpoints

### Crear un nuevo producto

- **URL:** `POST /api/productos`
- **Body:**
### Obtener un producto por su ID

- **URL:** `GET /api/productos/{id}`

### Listar todos los productos

- **URL:** `GET /api/productos`

### Actualizar un producto existente

- **URL:** `PUT /api/productos/{id}`
- **Body:**
### Eliminar un producto por su ID

- **URL:** `DELETE /api/productos/{id}`

## Contribuir

Si deseas contribuir a este proyecto, por favor sigue los siguientes pasos:

1. Haz un fork del repositorio.
2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza tus cambios y haz commit (`git commit -am 'Añadir nueva funcionalidad'`).
4. Sube tus cambios a tu fork (`git push origin feature/nueva-funcionalidad`).
5. Abre un Pull Request en el repositorio original.

## Autorización con JWT
debes autenticarte para generar el token y consumir el endpoint, recuerda que es tu correo y contraseña, el mismo que crear al crear un usuario

---



  

  

   

   

   
