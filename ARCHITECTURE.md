# Decisiones arquitectónicas y alternativas consideradas:

**Algunas de las alternativas que podría haber implementado en la prueba, pros y contras en la toma de deciciones...**

# Base de Datos:
**En la función de búsqueda PL/pgSQL implementé COUNT(*) OVER() por ser una herramienta muy cómoda y que permite hacer una cuenta total de los registros que hacen match en el filtro sin necesidad de hacer segunda query, pero para una tabla con millones de filas y con la necesidad de paginación, no sería la opción más funcional, puesto que para mostrar aunque sean solo los primeros 10 números la base de datos tiene que contar todas las filas que cumplen con el filtro y se tornaría lento.**

**En tablas con millones de filas sería mejor aplicar una arquitectura de Tablas de Metadatos (Counter cache) usando Triggers en la tabla principal que incrementen o decrementen el contador en cada INSERT o DELETE.**