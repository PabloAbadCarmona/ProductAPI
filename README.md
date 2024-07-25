# ProductAPI

## Arquitectura y patrones

Se ha empleado una arquitectura de N capas para el desarrollo de la aplicación, tenemos la capa de persistencia, servicios, repositorios, commands/queries, controllers.

**Patrones**
 - Inversión de control: El control de las dependencias es dado por el configuration root en lugar de que dependa de la implementación en cada clase, es muy común y prácticamente infaltable en cualquier desarrollo relativamente moderno.
 - CQRS: Dado que se tiene un maestro de productos, generalmente la lectura es más frecuente que la escritura, por lo que resulta conveniente separar los conceptos, en una implementación real, se podría evaluar separar incluso físicamente la persistencia de los datos, con un medio optimizado para lecturas, agregando la complejidad de que habría que sincronizar ambos orígenes de datos.
 - Repositorio: Nos permite tener una capa especializada en el acceso a los datos, y con el ejemplo del anterior patrón, en caso que se quiera cambiar o agregar un nuevo origen de datos, no afectaría a las demás dependencias.
 - Circuit Breaker: Ya que tenemos una dependencia externa, en caso de que esta falle un número mayor de veces al máximo de reintentos admitidos, no se realizarán más llamadas, también nos ayuda a que no se caiga por completo nuestro servicio cuando esto ocurra, pero sí se sufriría una degradación (no sería posible obtener el descuento para el producto.
 

## Uso y observaciones adicionales

Inicialmente pensé en hacerlo con EntityFramework y SQL Server, junto con la librería DbUp para levantar el script de creación automáticamente, lo cual hubiera sido mucho más sencillo, pero iba a ser más compleja la configuración para probarlo tanto en el proyecto principal como en el de pruebas, por lo que opté en implementar mi propia clase de persistencia en JSON, la cual, no es ni remotamente igual de íntegra que una BD. Pero por motivos de simplicidad en la revisión y uso del programa, decidí hacerlo de esa manera, así que no es necesario nada en especial para correr la aplicación.

Idealmente se podría haber integrado autenticación, ya que un API generalmente debería tenerlo, y en este caso como es de productos, pertenecería a algún negocio y no cualquier usuario debería hacer operaciones de escritura. Pero para mantener la simplicidad de ejecución y revisión, se omitió para este ejercicio.